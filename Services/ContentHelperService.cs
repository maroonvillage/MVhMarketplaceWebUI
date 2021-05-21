using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using webcoreapp.Enumerators;
using webui.Interfaces;
using webui.Models;

namespace webui.Services
{
    public class ContentHelperService : IContentHelperService 
    {

        private const string NoHtmlContextFound = "The html context is missing.";
        //private const string SiteContentblockIsNullMessage = "<span style='color: red !important;font-weight:bold !important;'>Model.SiteContentBlock IS NULL</span>";
        private const string SiteContentblockIsNullMessage = "[No Data]";
        //private const string SiteContentBlockDataIsNullOrMissingFormattedMessage = "<span style='color: red !important;font-weight:bold !important;'>Model.SiteContentBlock[\"{0}\"] IS MISSING OR NULL</span>";
        private const string SiteContentBlockDataIsNullOrMissingFormattedMessage = "";
        private const string SiteContentBlockDataInvalidFeedUrlFormattedMessage = "<span style='color: red !important;font-weight:bold !important;'>Model.SiteContentBlock[\"{0}\"] has an invalid feed url [{1}]</span>";
        private const string PartialViewNotFoundFormattedMessage = "The partial view '{0}' was not found";
        private const string DefaultFeedPartialViewName = "GenericFeed";


        private readonly string _message = "Hello, world from the Content Helper Service.";

        private IHtmlHelper _htmlHelper;
        private readonly IServiceProvider _serviceProvider;

        public ContentHelperService()
        {

        }

        public ContentHelperService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IHtmlContent> SayHello(IHtmlHelper html)
        {

            return await html.PartialAsync("_Testone.cshtml");
        }

        public async Task<IHtmlContent> SiteContentBlock(string key, DefaultModel model, IHtmlHelper htmlHelper)
        {

            if(htmlHelper == null) return new HtmlString(NoHtmlContextFound);

            _htmlHelper = htmlHelper;

            var siteContentBlock = model.SiteContentBlock;
            if (siteContentBlock == null)
            {
                return new HtmlString(SiteContentblockIsNullMessage);
            }

            if (!siteContentBlock.TryGetValue(key, out SiteContent content) || content == null)
            {
                return new HtmlString(string.Format(SiteContentBlockDataIsNullOrMissingFormattedMessage, key));
            }

            return await SiteContent(content, model);

        }


        public async Task<IHtmlContent> SiteContent(SiteContent content, DefaultModel model)// where T : DefaultModel
        {
            if ((bool)content.IsFeed)
            {
                if (Uri.IsWellFormedUriString(content.ContentValue, UriKind.Absolute))
                {
                    return await SiteContentFeed(model,content);
                }

                return new HtmlString(string.Format(SiteContentBlockDataInvalidFeedUrlFormattedMessage, content.Block.BlockMachineName, content.ContentValue));
            }

            if (content.ContentType != DynamicContentType.Unknown)
            {
                return await SiteContentDynamicContent(content, model);
            }

            return SiteContentHtml(content);
        }

        private static IHtmlContent SiteContentHtml(SiteContent content)
        {

            var contentValue = string.IsNullOrWhiteSpace(content.ContentValue) ? string.Empty : content.ContentValue;


            if (Debugger.IsAttached)
            {
                contentValue = string.Format("<!-- {0}{1}{0} -->{2}", Environment.NewLine, content.ToString().Replace("-->", "--&gt;"), contentValue);
            }

            return new HtmlString(contentValue);
        }

        private async Task<IHtmlContent> SiteContentDynamicContent(SiteContent content, DefaultModel model) //where T : DefaultModel
        {

            if (content.ContentType == DynamicContentType.None)
            {
                return await PartialAsync(model, content);
            }

            //dynamic data = content.ServiceProvider.GetService(typeof(IDynamicContentService));
            
           // ServiceExtensions.GetServiceProvider();
            //var serviceProvider = _serviceCollection.BuildServiceProvider();
            
            //dynamic data2 = ActivatorUtilities.CreateInstance<IDynamicContentService>(serviceProvider);
            dynamic data2 = _serviceProvider.GetRequiredService<IDynamicContentService>().GetData(content);

            if (data2 == null)
            {
                var value = string.Empty;
                if (Debugger.IsAttached)
                {
                    value = string.Format("<!-- {0}No items returned.{0}{1}{0} -->", Environment.NewLine, content.ToString().Replace("-->", "--&gt;"));
                }

                return new HtmlString(value);
            }

            model.Data = data2;
            

            return await PartialAsync(model, content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="content"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<IHtmlContent> PartialAsync(DefaultModel model, SiteContent content) //where T : DefaultModel
        {
            var siteContentBlock = model.SiteContent = content;

            try
            {
                return await _htmlHelper.PartialAsync(content.ContentName, model);
            }
            catch (InvalidOperationException e)
            {
                if (string.IsNullOrWhiteSpace(e.Message))
                {
                    throw;
                }

                if (e.Message.IndexOf(string.Format(PartialViewNotFoundFormattedMessage, content.ContentName), StringComparison.OrdinalIgnoreCase) > -1)
                {
                    return (IHtmlContent)await _htmlHelper.PartialAsync(DefaultFeedPartialViewName, model);
                }

                throw;
            }

        }

        private async Task<IHtmlContent> SiteContentFeed(DefaultModel model, SiteContent content)
        {
            //dynamic feed = null;/// DependencyResolver.Current.GetService<IFeedService>().GetFeed(content);

            //ddynamic feedSvc = content.ServiceProvider.GetService(typeof(IFeedService));

            //dynamic serviceProvider = _serviceCollection.BuildServiceProvider();

            // This is where DI magic happens:
            //var feedService = ActivatorUtilities.CreateInstance<IFeedService>(serviceProvider);
            var feedService = _serviceProvider.GetService<IFeedService>().GetFeed(content);

            if (feedService == null)
            {
                var value = string.Empty;
                if (Debugger.IsAttached)
                {
                    value = string.Format("<!-- {0}No items returned.{0}{1}{0} -->", Environment.NewLine, content.ToString().Replace("-->", "--&gt;"));
                }

                return new HtmlString(value);
            }

            var feedModel = new FeedModel
            {
                Feed = feedService,
                SiteContent = content
            };//.CopyPropertiesFrom(html.ViewData.Model);

            return await _htmlHelper.PartialAsync(content.ContentName, model);
            //return await feedModel.PartialAsync(content.ContentName, model);
        }

       
    }
}
