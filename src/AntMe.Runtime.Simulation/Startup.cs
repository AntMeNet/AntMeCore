using Microsoft.Owin.Cors;
using Owin;

namespace AntMe.Runtime.Simulation
{
   public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseCors(CorsOptions.AllowAll);
            appBuilder.MapSignalR();
        }

    }
}
