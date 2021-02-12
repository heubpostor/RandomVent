using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impostor.Api.Events;
using Impostor.Api.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomVent.EventListeners;

namespace RandomVent
{
    public class RandomVentStartup : IPluginStartup
    {
        public void ConfigureHost(IHostBuilder host)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEventListener, RandomVentListener>();
        }
    }
}
