using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using webui.Models;

namespace webui.Interfaces
{
    public interface IContentHelperService
    {
        Task<IHtmlContent> SayHello(IHtmlHelper html);

        //Task<IHtmlContent> SiteContentBlock<T>(string key, IHtmlHelper<T> html);// where T : DefaultModel;

        // Task<IHtmlContent> SiteContent<T>(SiteContent content, IHtmlHelper<T> html);// where T : DefaultModel;

        Task<IHtmlContent> SiteContentBlock(string key, IPageModel model, IHtmlHelper html);// where T : DefaultModel;

        Task<IHtmlContent> SiteContent(SiteContent content, IPageModel model);// where T : DefaultModel;
    }
}
