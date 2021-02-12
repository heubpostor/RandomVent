using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impostor.Api.Plugins;
using Microsoft.Extensions.Logging;

namespace RandomVent
{
    [ImpostorPlugin(
        package: "de.hbch.random-vent", 
        name: "RandomVent", 
        author: "jheubuch", 
        version: "1.0.0")]
    public class RandomVentPlugin : PluginBase
    {
        private readonly ILogger<RandomVentPlugin> _logger;

        public RandomVentPlugin(ILogger<RandomVentPlugin> logger)
        {
            _logger = logger;
        }

        public override ValueTask EnableAsync()
        {
            _logger.LogInformation("RandomVent plugin was enabled");
            return default;
        }

        public override ValueTask DisableAsync()
        {
            _logger.LogInformation("RandomVent plugin was disabled");
            return default;
        }
    }
}
