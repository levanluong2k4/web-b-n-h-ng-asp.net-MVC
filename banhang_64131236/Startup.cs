using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(banhang_64131236.Startup))]
namespace banhang_64131236
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
