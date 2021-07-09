using System;
using System.Collections.Generic;
using webui.Enums;
using webui.Models;

namespace webui.Interfaces
{
    public interface IModelBuilder
    {

       T LoadModelData<T>(T model, Action<T> action = null, string pageMachineName = null,
            SitePageType? page = null) where T : IPageModel;

       T CreateModel<T>(Action<T> action = null, string pageMachineName = null, SitePageType? page = null) where T : IPageModel, new();

       IDictionary<string, SiteContent> GetSiteContent(string pageMachineName = null);

    }
}
