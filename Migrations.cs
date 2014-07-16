using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Data.Migration;
using Orchard.Data.Migration.Schema;

namespace Commercan.GoogleAnalytics
{
    public class Migrations : DataMigrationImpl
    {
        
        public int Create() {
            SchemaBuilder.CreateTable("SettingsRecord",
                table => table.Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<bool>("Enabled", column => column.NotNull().WithDefault(false))
                    .Column<bool>("UseUniversalTracking", column => column.NotNull().WithDefault(false))
                    .Column<string>("DomainName")
                    .Column<string>("GoogleAnalyticsKey"));

            SchemaBuilder.CreateTable("ScriptCodesRecord",
                table => table.Column<int>("Id", column => column.PrimaryKey().Identity())
                        .Column<string>("ScriptType", column => column.NotNull().WithLength(50))
                        .Column<string>("Script", column => column.NotNull().Unlimited()));
            return 1;
        }
    }
}