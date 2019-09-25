using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using WMTServer.Mock;

namespace WMTServer
{
    public class WindmillFarmService : WindmillFarm.WindmillFarmBase
    {
        private readonly ILogger<WindmillFarmService> logger;
        private readonly WindmillsDataReader windmillsDataReader;

        public WindmillFarmService(ILogger<WindmillFarmService> logger, WindmillsDataReader windmillsDataReader)
        {
            this.logger = logger;
            this.windmillsDataReader = windmillsDataReader;
        }

        public override Task<WindmillListResponse> RequestList(WindmillListRequest request, ServerCallContext context)
        {
            var windmillInfos = windmillsDataReader.GetWindmills();
            var response = new WindmillListResponse
            {
                TotalCount = windmillInfos.Count,
                AvgPowerGeneratedLastMinute = windmillInfos.Sum(wi => wi.AvgPowerGeneratedLastMinute),
                AvgPowerGeneratedLastHour = windmillInfos.Sum(wi => wi.AvgPowerGeneratedLastHour)
            };
            response.Windmills.Add(windmillInfos);
            return Task.FromResult(response);
        }

        public override Task<WindmillInfo> RequestWindmillStatus(WindmillStatusRequest request, ServerCallContext context)
        {
            return Task.FromResult(windmillsDataReader.GetWindmillInfo(Guid.Parse(request.WindmillId)));
        }
    }
}