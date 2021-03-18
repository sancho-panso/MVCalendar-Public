using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVCalendar.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MVCalendar.CronService
{
 // code is mainly copied from https://codeburst.io/schedule-cron-jobs-using-hostedservice-in-asp-net-core-e17c47ba06
 // used with minimal editting

    public class MyCronJob : CronJobService
    {
        private static readonly NLog.Logger NLogger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ILogger<MyCronJob> _logger;
        private readonly IServiceProvider _provider;

        public MyCronJob(IScheduleConfig<MyCronJob> config,
                         ILogger<MyCronJob> logger,
                         IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _provider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob starts.");
            NLogger.Info("CronJob starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task<Task> DoWork(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = _provider.CreateScope())   // applying using instead of DI to solve conflict on Singleton and OnTransient
            {
                _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob  is working.");
                NLogger.Info($"{DateTime.Now:hh:mm:ss} CronJob  is working.");
                var sms = scope.ServiceProvider.GetRequiredService<SmsController>();
                await sms.SmsReminders();
                return Task.CompletedTask;
            }
              
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob is stopping.");
            NLogger.Info("CronJob is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
