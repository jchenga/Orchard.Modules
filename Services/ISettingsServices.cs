using Commercan.GoogleAnalytics.Models;
using Commercan.GoogleAnalytics.ViewModels;
using Orchard;

namespace Commercan.GoogleAnalytics.Services
{
    public interface ISettingsServices : IDependency
    {
        SettingsRecord Get();
        bool Set(SettingsViewModel viewModel);
        string GetScript();
    }
}
