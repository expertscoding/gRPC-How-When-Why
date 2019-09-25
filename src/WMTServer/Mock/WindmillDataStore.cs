using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WMTServer.Mock
{
    public class WindmillDataStore
    {
        public ReadOnlyCollection<Guid> Windmills { get; }

        protected Dictionary<Guid, SortedDictionary<long, WindmillTelemetryResponse>> TelemetryValues { get; set; } = new Dictionary<Guid, SortedDictionary<long, WindmillTelemetryResponse>>();

        public WindmillDataStore()
        {
            var guids = new List<Guid>();
            for (var i = 0; i < 10; i++)
            {
                guids.Add(Guid.NewGuid());
            }
            Windmills = new ReadOnlyCollection<Guid>(guids);
            guids.ForEach(g => TelemetryValues.Add(g, new SortedDictionary<long, WindmillTelemetryResponse>()));
        }

        public Task GenerateValues(WindDirection windDirection)
        {
            return Task.Run(() =>
            {
                var now = DateTime.UtcNow;
                var rnd = new Random();
                foreach (var windmillId in Windmills)
                {
                    TelemetryValues[windmillId].Add(now.Ticks, new WindmillTelemetryResponse
                    {
                        WindmillId = windmillId.ToString(),
                        EventTime = now.ToString("O"),
                        PowerOutput = rnd.Next(5137, 5873),
                        RPM = rnd.Next(65, 97),
                        VoltageOutput = rnd.Next(125, 131),
                        WindDirection = windDirection,
                        WindSpeed = rnd.Next(27, 36)
                    });
                }
            });
        }

        public bool ExistsWindmill(Guid windmillId)
        {
            return Windmills.Any(w => w == windmillId);
        }

        public ReadOnlyDictionary<long, WindmillTelemetryResponse> GetTelemetryValues(Guid windmillId)
        {
            return TelemetryValues.ContainsKey(windmillId)
                ? new ReadOnlyDictionary<long, WindmillTelemetryResponse>(TelemetryValues[windmillId])
                : new ReadOnlyDictionary<long, WindmillTelemetryResponse>(new Dictionary<long, WindmillTelemetryResponse>());
        }
    }
}