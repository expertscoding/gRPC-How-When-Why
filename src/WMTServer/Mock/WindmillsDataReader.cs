using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WMTServer.Mock
{
    public class WindmillsDataReader
    {
        private readonly WindmillDataStore store;

        public WindmillsDataReader(WindmillDataStore store)
        {
            this.store = store;
        }

        public List<WindmillInfo> GetWindmills()
        {
            return store.Windmills.Select(GetWindmillInfo).ToList();
        }

        public WindmillInfo GetWindmillInfo(Guid windmillId)
        {
            if (!store.ExistsWindmill(windmillId))
            {
                return null;
            }

            var values = store.GetTelemetryValues(windmillId);
            var utcNowTicks = DateTime.UtcNow.Ticks;
            var minuteLapse = utcNowTicks - TimeSpan.FromMinutes(1).Ticks;
            var hourLapse = utcNowTicks - TimeSpan.FromHours(1).Ticks;
            return new WindmillInfo
            {
                WindmillId = windmillId.ToString(),
                AvgPowerGeneratedLastMinute = values.Where(v => v.Key > minuteLapse).Average(v => v.Value.PowerOutput),
                AvgPowerGeneratedLastHour = values.Where(v => v.Key > hourLapse).Average(v => v.Value.PowerOutput)
            };
        }

        public ReadOnlyCollection<WindmillTelemetryResponse> GetTelemetryValues(Guid windmillId)
        {
            return store.GetTelemetryValues(windmillId).Values.ToList().AsReadOnly();
        }
    }
}