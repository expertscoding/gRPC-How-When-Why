using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace WMTServer
{
    public class WindmillTelemeterService : WindmillTelemeter.WindmillTelemeterBase
    {
        private readonly ILogger<WindmillTelemeterService> _logger;
        public WindmillTelemeterService(ILogger<WindmillTelemeterService> logger)
        {
            _logger = logger;
        }

        public override Task<WindmillTelemetryReply> SendTelemetry(WindmillInfoRequest request, ServerCallContext context)
        {
            return Task.FromResult(new WindmillTelemetryReply
            {
                Message = "Hello " + request.WindmillId
            });
        }
    }
}
