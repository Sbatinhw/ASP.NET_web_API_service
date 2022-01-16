using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Infrastructure.Extensions
{
    internal static class ConfigureCollectionExtensions
    {
        public static void UseConfigureMigration(this IMigrationRunner migrationRunner)
        {
            migrationRunner.MigrateUp();
        }
        public static void UseConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API сервиса менеджера сбора метрик");
            });
        }
    }
}
