using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalR.Core.Startup))]
namespace SignalR.Core
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);


            app.MapSignalR();
        }
    }
}
