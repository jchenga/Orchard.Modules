using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.Caching;
using Orchard.Mvc.Filters;
using Orchard.UI.Admin;
using Orchard.UI.Resources;

namespace Commercan.GoogleAnalytics.Filters
{
    public class GoogleAnalyticsFilter : IResultFilter {
        
        private readonly IOrchardServices _services;
        private readonly IResourceManager _resourceManager;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public GoogleAnalyticsFilter(IResourceManager resourceManager, 
                                     IOrchardServices services,
                                     ICacheManager cacheManager,
                                     ISignals signals) {
            _services = services;
            _resourceManager = resourceManager;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            if (AdminFilter.IsApplied(filterContext.RequestContext)) {
                return;
            }

            if (!(filterContext.Result is ViewResult))
                return;


            var script = _cacheManager.Get("Commercan.GoogleAnalytics.Settings",
                ctx => {
                    ctx.Monitor(_signals.When("Commercan.GoogleAnalytics.Settings"));
                    var settings = _settingsService.Get();
                    return !settings.Enable ? string.Empty : _settingsService.GetScript(settings);
                });

        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
           
        }
    }
}