using System.Threading.Tasks;
using Grpc.Core;

namespace WMTServer
{
    public class WindmillFarmService : WindmillFarm.WindmillFarmBase
    {
        public override Task<WindmillListResponse> ListRequest(WindmillListRequest request, ServerCallContext context)
        {
            return base.ListRequest(request, context);
        }
    }
}