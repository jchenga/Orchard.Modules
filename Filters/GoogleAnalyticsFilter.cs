using System.Web.Mvc;
using Commercan.GoogleAnalytics.Services;
using Orchard.Caching;
using Orchard.Mvc.Filters;
using Orchard.UI.Admin;
using Orchard.UI.Resources;

namespace Commercan.GoogleAnalytics.Filters
{
    public class GoogleAnalyticsFilter : FilterProvider, IResultFilter {

        private readonly IResourceManager _resourceManager;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private readonly ISettingsServices _settingsServices;

        public GoogleAnalyticsFilter(IResourceManager resourceManager,
                                     ICacheManager cacheManager,
                                     ISignals signals,
                                     ISettingsServices settingsServices) {
            _resourceManager = resourceManager;
            _cacheManager = cacheManager;
            _signals = signals;
            _settingsServices = settingsServices;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            if (AdminFilter.IsApplied(filterContext.RequestContext)) {
                return;
            }

            if (!(filterContext.Result is ViewResult))
                return;


            var script = _cacheManager.Get("Commercan.GoogleAnalytics.Settings",
                ctx => {
                    ctx.Monitor(_signals.When("Commercan.GoogleAnalytics.SettingsChanged"));
                    return  _settingsServices.GetScript();
                });

            if (string.IsNullOrEmpty(script))
                return;

            _resourceManager.RegisterHeadScript(script);
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
           
        }
    }
}