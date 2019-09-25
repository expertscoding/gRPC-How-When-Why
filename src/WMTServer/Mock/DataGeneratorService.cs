using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WMTServer.Mock
{
    public class DataGeneratorService : IHostedService, IDisposable
    {
        private readonly ILogger<DataGeneratorService> logger;
        private readonly WindmillDataStore store;
        private readonly WindDirection windDirection;
        private Timer timer;

        public DataGeneratorService(ILogger<DataGeneratorService> logger, WindmillDataStore store)
        {
            this.logger = logger;
            this.store = store;
            this.windDirection = (WindDirection) new Random().Next((int) WindDirection.N, (int) WindDirection.Nw);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"{nameof(DataGeneratorService)} started.");
            timer = new Timer(GenerateData, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        public void GenerateData(object state)
        {
            store.GenerateValues(windDirection);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"{nameof(DataGeneratorService)} stopped.");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}