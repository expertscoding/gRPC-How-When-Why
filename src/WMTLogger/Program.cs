using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using WMTServer;

namespace WMTLogger
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new WindmillFarm.WindmillFarmClient(channel);

            var windmills = client.RequestList(new WindmillListRequest());
            var guid = windmills.Windmills.FirstOrDefault()?.WindmillId;

            if (Guid.TryParse(guid, out var windmillId))
            {
                Console.WriteLine($"Windmill info for {guid}");
                await TelemetryStreaming(windmillId, channel);
            }


            Console.WriteLine("Shutting down client");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task TelemetryStreaming(Guid guid, GrpcChannel channel)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(27));

            var client = new WindmillTelemeter.WindmillTelemeterClient(channel);

            using (var call = client.RequestTelemetryStream(new WindmillInfoRequest{WindmillId = guid.ToString()}, cancellationToken: cts.Token))
            {
                try
                {
                    await foreach (var wi in call.ResponseStream.ReadAllAsync())
                    {
                        Console.WriteLine($"Windmill info at {wi.EventTime}: {{RPM={wi.RPM},Power={wi.PowerOutput},Voltage={wi.VoltageOutput}}}");
                    }
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
                {
                    Console.WriteLine("Cancelling telemetry reading.");
                }
            }
        }
    }
}
