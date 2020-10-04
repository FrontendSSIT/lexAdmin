using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lex_Diary_Admin_Panel.Startup))]
namespace Lex_Diary_Admin_Panel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
