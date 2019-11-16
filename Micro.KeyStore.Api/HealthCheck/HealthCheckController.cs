using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Timer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.KeyStore.Api.HealthCheck
{
    [ApiController]
    [Route("api/health")]
    public class HealthCheckController : ControllerBase
    {
        private readonly IMetrics _metrics;

        public HealthCheckController(IMetrics metrics)
        {
            _metrics = metrics;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HealthData>), StatusCodes.Status200OK)]
        public async Task<HealthData> Get()
        {
            using (_metrics.Measure.Timer.Time(new TimerOptions
            {
                Name = "Sample.Timer",
                MeasurementUnit = Unit.Requests,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds
            }))
            {
                await Task.Delay(1000);
                return new HealthData
                {
                    FakeCacheHealth = await GetFakeCacheHealth(),
                    FakeDbHealth = await GetFakeDbHealth()
                };
            }
        }

        private static Task<bool> GetFakeDbHealth()
        {
            return new Task<bool>(() => true);
        }

        private static Task<bool> GetFakeCacheHealth()
        {
            return Task.Run(() => true);
        }

    }
}
