using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Timer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Starter.Api.HealthCheck
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

        private static async Task<bool> GetFakeDbHealth()
        {
            try
            {
                return true;
            }
            catch (Exception)
            {
                // todo: log e
                return false;
            }
        }

        private static Task<bool> GetFakeCacheHealth()
        {
            return Task.Run(() => true);
        }

    }
}
