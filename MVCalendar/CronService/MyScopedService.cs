using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVCalendar.CronService
{
 // code is copied from https://codeburst.io/schedule-cron-jobs-using-hostedservice-in-asp-net-core-e17c47ba06

    public interface IMyScopedService
    {
        Task DoWork(CancellationToken cancellationToken);
    }

    public class MyScopedService : IMyScopedService
    {
        private readonly ILogger<MyScopedService> _logger;

        public MyScopedService(ILogger<MyScopedService> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} MyScopedService is working.");
            await Task.Delay(1000 * 20, cancellationToken);
        }
    }
}
