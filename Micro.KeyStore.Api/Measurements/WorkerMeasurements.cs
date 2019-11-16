using System;
using App.Metrics;

namespace Micro.KeyStore.Api.Measurements
{
    public class WorkerMeasurements
    {
        public static void MeasureTime(IMetrics metrics, TimeSpan duration)
        {
            var timer = new App.Metrics.Timer.TimerOptions
            {
                Name = "KeyArchiveSaveTime",
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds
            };
            metrics?.Provider?.Timer.Instance(timer).Record((long) duration.TotalMilliseconds, TimeUnit.Milliseconds);
        }

        public static void MeasureArchiveOccurrence(IMetrics metrics)
        {
            var meter = new App.Metrics.Meter.MeterOptions
            {
                Name = "KeyArchive",
                MeasurementUnit = Unit.Events,
                RateUnit = TimeUnit.Seconds
            };
            metrics?.Provider?.Meter.Instance(meter).Mark();
        }
        public static void MeasureErrorOccurrence(IMetrics metrics)
        {
            var meter = new App.Metrics.Meter.MeterOptions
            {
                Name = "KeyArchiveError",
                MeasurementUnit = Unit.Events,
                RateUnit = TimeUnit.Seconds
            };
            metrics?.Provider?.Meter.Instance(meter).Mark();
        }
    }
}