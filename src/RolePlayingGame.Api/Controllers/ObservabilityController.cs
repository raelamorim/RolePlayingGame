using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace RolePlayingGame.Api.Controllers
{
    [ApiController]
    [Route("api/observability")]
    [ExcludeFromCodeCoverage]
    public class ObservabilityController : ControllerBase
    {
        private static readonly ActivitySource ActivitySource = new("RolePlayingGame");

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            });
        }

        [HttpGet("metrics")]
        public IActionResult GetMetrics()
        {
            var processId = Process.GetCurrentProcess().Id;
            var workingSet = Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024); // MB

            return Ok(new
            {
                processId,
                workingSetMb = workingSet,
                uptime = DateTime.UtcNow,
                tracingEnabled = true
            });
        }
    }
}
