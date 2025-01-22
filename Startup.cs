using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sell_Your_Books.Startup))]
namespace Sell_Your_Books
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
