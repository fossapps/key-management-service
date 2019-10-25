using System;
using System.Globalization;
using System.Linq;
using App.Metrics;

namespace Micro.KeyStore.Api.Measurements
{
    public static class ShortShaKey
    {
        public static void Measure(IMetrics metrics, string shortSha, TimeSpan duration)
        {
            MeasureLength(metrics, shortSha);
            MeasureTime(metrics, duration);
        }

        private static void MeasureLength(IMetrics metrics, string shortSha)
        {
            var length = shortSha.Length;
            var startsWith = shortSha.First().ToString();
            var measurement = new App.Metrics.Histogram.HistogramOptions
            {
                Name = "ShortShaKeyLength",
                MeasurementUnit = Unit.Bytes,
                Tags = new MetricTags("starts with", startsWith)
            };
            metrics.Measure.Histogram.Update(measurement, length);
        }

        private static void MeasureTime(IMetrics metrics, TimeSpan timeSpan)
        {
            var timer = new App.Metrics.Timer.TimerOptions
            {
                Name = "ShortShaKeyGenerationTime",
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds,
            };
            metrics.Provider.Timer.Instance(timer).Record((long) timeSpan.TotalMilliseconds, TimeUnit.Milliseconds);
        }
    }
}