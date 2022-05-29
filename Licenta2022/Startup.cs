using Microsoft.Owin;
using Owin;

using Microsoft.AspNetCore.Http;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.ChakraCore;
using React.AspNet;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;


[assembly: OwinStartupAttribute(typeof(Licenta2022.Startup))]
namespace Licenta2022
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
		}

		// public void Configure(IApplicationBuilder app)
        // {
		// 	app.UseReact(config =>
		// 	{
		// 		config
		// 			.SetReuseJavaScriptEngines(true)
		// 			.SetLoadBabel(false)
		// 			.SetLoadReact(false)
		// 			.SetReactAppBuildPath("~/dist");
		// 	});

		// 	app.UseMvc();
		// }

		// public void ConfigureServices(IServiceCollection services)
		// {
		// 	services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName)
		// 		.AddChakraCore();

		// 	services.AddReact();
		// 	services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

		// 	// Build the intermediate service provider then return it
		// 	services.BuildServiceProvider();
		// }
	}
}
