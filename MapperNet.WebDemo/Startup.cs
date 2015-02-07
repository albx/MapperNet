using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MapperNet.WebDemo.Startup))]
namespace MapperNet.WebDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
