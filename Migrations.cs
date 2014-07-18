using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Commercan.GoogleAnalytics.Models;
using Orchard.Data;
using Orchard.Data.Migration;
using Orchard.Data.Migration.Schema;

namespace Commercan.GoogleAnalytics {
    public class Migrations : DataMigrationImpl {
        private readonly IRepository<ScriptCodesRecord> _repository;

        public Migrations(IRepository<ScriptCodesRecord> repository) {
            _repository = repository;
        }

        public int Create() {
            SchemaBuilder.CreateTable("SettingsRecord",
                table => table.Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<bool>("Enable", column => column.NotNull().WithDefault(false))
                    .Column<bool>("UseUniversalTracking", column => column.NotNull().WithDefault(false))
                    .Column<string>("GoogleAnalyticsKey"));

            SchemaBuilder.CreateTable("ScriptCodesRecord",
                table => table.Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("ScriptType", column => column.NotNull().WithLength(50))
                    .Column<string>("Script", column => column.NotNull().Unlimited()));
            
            InsertUniversalAnalyticsScript();

            InsertGAScript();

            return 1;
        }

        private void InsertUniversalAnalyticsScript() {
            var script = new ScriptCodesRecord {
                ScriptType = "universal",
                Script = @"<!-- Google Analytics -->
                            <script>
                            (function(i,s,o,g,r,a,m){{i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){{
                            (i[r].q=i[r].q||[]).push(arguments)}},i[r].l=1*new Date();a=s.createElement(o),
                            m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
                            }})(window,document,'script','//www.google-analytics.com/analytics.js','ga');

                            ga('create', '{0}', 'auto');
                            ga('send', 'pageview');

                            </script>
                            <!-- End Google Analytics -->"
            };

            _repository.Create(script);
        }

        private void InsertGAScript()
        {
            var script = new ScriptCodesRecord
            {
                ScriptType = "ga",
                Script = @"<script type=""text/javascript"">
                          var _gaq = _gaq || [];
                          _gaq.push(['_setAccount', '{0}']);
                          _gaq.push(['_trackPageview']);

                          (function() {{
                            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
                            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
                            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
                          }})();

                        </script>"
            };

            _repository.Create(script);
        }
    }
}