
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Example.Base;
using NetFastPack;

namespace Example.Service
{
    public class Startup : ApiStartup
    {
        public Startup(IConfiguration configuration) : base(configuration) { }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddNetFastPack();
        }
    }
}