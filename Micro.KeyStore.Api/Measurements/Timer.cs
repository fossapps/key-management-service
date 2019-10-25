using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Micro.KeyStore.Api.Measurements
{
    public static class Timer
    {
        public static (TimeSpan, T) Measure<T>(Func<T> fn)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = fn();
            var elapsed = stopwatch.Elapsed;
            return (elapsed, result);
        }

        public static async Task<(TimeSpan, T)> MeasureAsync<T>(Func<Task<T>> fn)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = await fn();
            var elapsed = stopwatch.Elapsed;
            return (elapsed, result);
        }
    }
}