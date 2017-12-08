using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImprovementOpportunityApp.Startup))]
namespace ImprovementOpportunityApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
