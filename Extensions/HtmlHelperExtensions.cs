using System;
using System.Diagnostics;
using System.ServiceModel.Syndication;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using webcoreapp.Enumerators;
using webui.Interfaces;
using webui.Models;
using webui.Services;

namespace webui.Extensions
{
    public static class HtmlHelperExtensions
    {
        //private const string SiteContentblockIsNullMessage = "<span style='color: red !important;font-weight:bold !important;'>Model.SiteContentBlock IS NULL</span>";
        private const string SiteContentblockIsNullMessage = "";
        //private const string SiteContentBlockDataIsNullOrMissingFormattedMessage = "<span style='color: red !important;font-weight:bold !important;'>Model.SiteContentBlock[\"{0}\"] IS MISSING OR NULL</span>";
        private const string SiteContentBlockDataIsNullOrMissingFormattedMessage = "";
        private const string SiteContentBlockDataInvalidFeedUrlFormattedMessage = "<span style='color: red !important;font-weight:bold !important;'>Model.SiteContentBlock[\"{0}\"] has an invalid feed url [{1}]</span>";
        private const string PartialViewNotFoundFormattedMessage = "The partial view '{0}' was not found";
        private const string DefaultFeedPartialViewName = "GenericFeed";

        #region SiteContentBlock

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IHtmlContent SiteContentBlock<T>(this HtmlHelper<T> html, string key)
             where T : DefaultModel
        {
            var siteContentBlock = html.ViewData.Model.SiteContentBlock;
            if (siteContentBlock == null)
            {
                return new HtmlString(SiteContentblockIsNullMessage);
            }

            SiteContent content;
            if (!siteContentBlock.TryGetValue(key, out content) || content == null)
            {
                return new HtmlString(string.Format(SiteContentBlockDataIsNullOrMissingFormattedMessage, key));
            }

            return html.SiteContent(content);
        }

        /// <summary>
        /// Renders div tags to display sprite
        /// </summary>
        /// <param name="html"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static IHtmlContent ContentForSprite<T>(this HtmlHelper<T> html, string key, string criteria)
            where T : DefaultModel
        {
            var contentForSprite = html.ViewData.Model.SiteContentBlock;
            if (contentForSprite == null)
            {
                return new HtmlString(SiteContentblockIsNullMessage);
            }

            SiteContent content;
            if (!contentForSprite.TryGetValue(key, out content) || content == null)
            {
                return new HtmlString(string.Format(SiteContentBlockDataIsNullOrMissingFormattedMessage, key));
            }

            //check for the make in the mark-up
            if (!content.ContentValue.ToLower().Contains(criteria.ToLower()))
            {
                criteria = "default";
            }


            //strip unncessary tokens from the mark-up
            //delimiters
            int startIndex = content.ContentValue.LastIndexOf('>');
            string subString;

            try
            {
                subString = content.ContentValue.Substring(0, content.ContentValue.Length - (content.ContentValue.Length - (startIndex + 1)));
            }
            catch (IndexOutOfRangeException ioore)
            {
                //TODO: Log the error here
                return new HtmlString(ioore.Message);
            }// end try-catch

            return new HtmlString(string.Format(subString, criteria.ToLower().Replace(" ", "_")));

        }// end method


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static IHtmlContent SiteContent<T>(this HtmlHelper<T> html, SiteContent content)
            where T : DefaultModel
        {
            if ((bool)content.IsFeed)
            {
                if (Uri.IsWellFormedUriString(content.ContentValue, UriKind.Absolute))
                {
                    return html.SiteContentFeed(content);
                }

                return new HtmlString(string.Format(SiteContentBlockDataInvalidFeedUrlFormattedMessage, content.Block.BlockMachineName, content.ContentValue));
            }

            if (content.ContentType != DynamicContentType.Unknown)
            {
                return html.SiteContentDynamicContent(content);
            }

            return html.SiteContentHtml(content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static IHtmlContent SiteContentHtml<T>(this HtmlHelper<T> html, SiteContent content)
            where T : DefaultModel
        {

            var contentValue = string.IsNullOrWhiteSpace(content.ContentValue) ? string.Empty : content.ContentValue;

            
            if (Debugger.IsAttached)
            {
                contentValue = string.Format("<!-- {0}{1}{0} -->{2}", Environment.NewLine, content.ToString().Replace("-->", "--&gt;"), contentValue);
            }

            return new HtmlString(contentValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static IHtmlContent SiteContentFeed<T>(this HtmlHelper<T> html, SiteContent content)
            where T : DefaultModel
        {
            dynamic feed = null;/// DependencyResolver.Current.GetService<IFeedService>().GetFeed(content);

            if (feed == null || !feed.Items.Any())
            {
                var value = string.Empty;
                if (Debugger.IsAttached)
                {
                    value = string.Format("<!-- {0}No items returned.{0}{1}{0} -->", Environment.NewLine, content.ToString().Replace("-->", "--&gt;"));
                }

                return new HtmlString(value);
            }

            var model = new FeedModel
            {
                Feed = feed,
                SiteContent = content
            };//.CopyPropertiesFrom(html.ViewData.Model);

            return html.Partial(content, model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static IHtmlContent SiteContentDynamicContent<T>(this HtmlHelper<T> html, SiteContent content)
            where T : DefaultModel
        {

            if (content.ContentType == DynamicContentType.None)
            {
                return html.Partial(content, html.ViewData.Model);
            }

            dynamic data = null;// Current.GetService<IDynamicContentService>().GetData(content);

            if (data == null)
            {
                var value = string.Empty;
                if (Debugger.IsAttached)
                {
                    value = string.Format("<!-- {0}No items returned.{0}{1}{0} -->", Environment.NewLine, content.ToString().Replace("-->", "--&gt;"));
                }

                return new HtmlString(value);
            }

            html.ViewData.Model.Data = data;

            return html.Partial(content, html.ViewData.Model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="content"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private static IHtmlContent Partial(this HtmlHelper html, SiteContent content, DefaultModel model)
        {
            model.SiteContent = content;

            try
            {
                return (IHtmlContent)html.PartialAsync(content.ContentName, model);
            }
            catch (InvalidOperationException e)
            {
                if (string.IsNullOrWhiteSpace(e.Message))
                {
                    throw;
                }

                if (e.Message.IndexOf(string.Format(PartialViewNotFoundFormattedMessage, content.ContentName), StringComparison.OrdinalIgnoreCase) > -1)
                {
                    return (IHtmlContent)html.PartialAsync(DefaultFeedPartialViewName, model);
                }

                throw;
            }
        }

        #endregion

    }
}
