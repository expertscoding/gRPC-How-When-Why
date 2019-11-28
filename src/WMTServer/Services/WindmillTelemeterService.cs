using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using WMTServer.Mock;

namespace WMTServer
{
    public class WindmillTelemeterService : WindmillTelemeter.WindmillTelemeterBase
    {
        private readonly ILogger<WindmillTelemeterService> _logger;
        private readonly WindmillsDataReader windmillsDataReader;

        public WindmillTelemeterService(ILogger<WindmillTelemeterService> logger, WindmillsDataReader windmillsDataReader)
        {
            _logger = logger;
            this.windmillsDataReader = windmillsDataReader;
        }

        public override Task<WindmillTelemetryResponse> RequestTelemetry(WindmillInfoRequest request, ServerCallContext context)
        {
            return Task.FromResult(windmillsDataReader.GetTelemetryValues(Guid.Parse(request.WindmillId)).LastOrDefault());

        }

        public override async Task RequestTelemetryStream(WindmillInfoRequest request, IServerStreamWriter<WindmillTelemetryResponse> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                //TODO add business logic to receive last event only once
                var last = windmillsDataReader.GetTelemetryValues(Guid.Parse(request.WindmillId)).LastOrDefault();
                _logger.LogInformation($"Sending windmill info for {last?.WindmillId} at {last?.EventTime}.");

                await responseStream.WriteAsync(last);

                // simulemos más trabajo
                await Task.Delay(1000);
            }
        }
    }
}
