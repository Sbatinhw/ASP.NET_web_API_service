using AutoMapper;
using FluentMigrator.Runner;
using MetricsManager;
using MetricsManager.Client;
using MetricsManager.DAL.Interfaces.Info;
using MetricsManager.DAL.Interfaces.Metrics;
using MetricsManager.DAL.Repositories;
using MetricsManager.Infrastructure.Mapper;
using MetricsManager.Jobs;
using MetricsManager.Jobs.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Polly;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Infrastructure.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IAgentsInfoRepository, AgentsInfoRepository>();
            services.AddSingleton<IRamMetricsRepository, RamMetricRepository>();
            services.AddSingleton<IHddMetricsRepository, HddMetricRepository>();
            services.AddSingleton<ICpuMetricsRepository, CpuMetricRepository>();
        }
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }
        public static void ConfigureSqlLiteConnection(this IServiceCollection services)
        {
            const string connectionString = @"Data Source=metrics.db; Version=3;";
            
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();

            services.AddSingleton(mapper);

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // добавляем поддержку SQLite 
                    .AddSQLite()
                    // устанавливаем строку подключения
                    .WithGlobalConnectionString(connectionString)
                    // подсказываем где искать классы с миграциями
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                ).AddLogging(lb => lb
                    .AddFluentMigratorConsole());
        }
        public static void ConfigureQuartz(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();
        }

        public static void ConfigureJobs(this IServiceCollection services)
        {
            services.AddSingleton<RequestCpuMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RequestCpuMetricJob),
                cronExpression: "0/5 * * * * ?"));

            services.AddSingleton<RequestHddMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RequestHddMetricJob),
                cronExpression: "0/5 * * * * ?"));

            services.AddSingleton<RequestRamMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RequestRamMetricJob),
                cronExpression: "0/5 * * * * ?"));

        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API сервиса менеджера сбора метрик",
                    TermsOfService = new Uri("https://github.com/sebatinpets"),
                    Contact = new OpenApiContact
                    {
                        Name = "SEBatin",
                        Email = "sebatin1995@gmail.com",
                        Url = new Uri("https://github.com/sebatinpets"),
                    }
                });
            });
        }

        public static void ConfigureHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>()
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
        }


    }
}
