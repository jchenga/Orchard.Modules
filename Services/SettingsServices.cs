using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Commercan.GoogleAnalytics.Models;
using Orchard.Caching;
using Orchard.Data;

namespace Commercan.GoogleAnalytics.Services
{
    public class SettingsServices : ISettingsServices {
        private readonly IRepository<SettingsRecord> _repository;
        private readonly IRepository<ScriptCodesRecord> _scriptRepository;
        private readonly ISignals _signals;

        public SettingsServices(IRepository<SettingsRecord> repository,
            IRepository<ScriptCodesRecord> scriptRepository,
            ISignals signals) {
            _repository = repository;
            _scriptRepository = scriptRepository;
            _signals = signals;
        }

        public SettingsRecord Get() {
            var record = _repository.Table.FirstOrDefault();

            if (record == null) {
                record = new SettingsRecord {
                    Enable = false,
                    DomainName = string.Empty,
                    GoogleAnalyticsKey = string.Empty,
                    UseUniversalTracking = false
                };

                _repository.Create(record);
            }

            return record;
        }

        public string GetScript() {
            var settings = Get();

            var scriptCode = GetScriptCode(settings.UseUniversalTracking);

            return string.Format(scriptCode.Script, settings.DomainName, settings.GoogleAnalyticsKey);
        }

        private ScriptCodesRecord GetScriptCode(bool useUniversalTracking) {
            var scriptType = useUniversalTracking ? "universal" : "ga";
            
            return _scriptRepository.Table.FirstOrDefault(r => scriptType.Equals(r.ScriptType, StringComparison.OrdinalIgnoreCase));
        }


        public bool Set(ViewModels.SettingsViewModel viewModel) {
            var settings = Get();

            settings.Enable = viewModel.Enable;
            settings.UseUniversalTracking = viewModel.UseUniversalTracking;
            settings.GoogleAnalyticsKey = viewModel.GoogleAnalyticsKey;
            settings.DomainName = viewModel.DomainName;

            _signals.Trigger("Commercan.GoogleAnalytics.SettingsChanged");

            return true;
        }
    }
}