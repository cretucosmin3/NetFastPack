
using Example.Base;

namespace Example.Service
{
    public class Startup : ApiStartup
    {
        public Startup(IConfiguration configuration) : base(configuration) { }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }
    }
}