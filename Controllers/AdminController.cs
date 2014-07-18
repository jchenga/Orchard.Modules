using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commercan.GoogleAnalytics.Services;
using Commercan.GoogleAnalytics.ViewModels;
using Orchard;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace Commercan.GoogleAnalytics.Controllers
{
    [Admin]
    public class AdminController : Controller {
        private readonly ISettingsServices _settingsServices;
        private readonly IOrchardServices _orchardServices;

        public AdminController(ISettingsServices settingsServices, IOrchardServices orchardServices) {
            _orchardServices = orchardServices;
            _settingsServices = settingsServices;
            T = NullLocalizer.Instance;
        }

        public ActionResult Index() {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Only Site Owner has the permission to manage Google Analytics"))) {
                return new HttpUnauthorizedResult();
            }

            var settings = _settingsServices.Get();

            var viewModel = new SettingsViewModel {
                Enable =  settings.Enable,
                GoogleAnalyticsKey = settings.GoogleAnalyticsKey,
                UseUniversalTracking = settings.UseUniversalTracking
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(SettingsViewModel viewModel) {
            if (!_orchardServices.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Only Site Owner has the permission to manage Google Analytics"))) {
                return new HttpUnauthorizedResult();
            }

            if (_settingsServices.Set(viewModel)) {
                _orchardServices.Notifier.Information(T("Google Analytics settings successfully saved"));
            }

            return View(viewModel);
        }

        public Localizer T { get; set; }
    }
}