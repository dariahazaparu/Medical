using Microsoft.Owin;
using Owin;

using Microsoft.AspNetCore.Http;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.ChakraCore;
using React.AspNet;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Licenta2022.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Licenta2022.Migrations;

[assembly: OwinStartupAttribute(typeof(Licenta2022.Startup))]
namespace Licenta2022
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			//var migration = new Initial();

			//migration.Down();

			ConfigureAuth(app);
			CreateAdminUserAndApplicationRoles();
		}

		private void CreateAdminUserAndApplicationRoles()
		{
			ApplicationDbContext context = new ApplicationDbContext();
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
			var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
			// Se adauga rolurile aplicatiei
			if (!roleManager.RoleExists("Admin"))
			{
				// Se adauga rolul de administrator
				var role = new IdentityRole();
				role.Name = "Admin";
				roleManager.Create(role);
				// se adauga utilizatorul administrator
				var user = new ApplicationUser();
				user.UserName = "admin@gmail.com";
				user.Email = "admin@gmail.com";
				var adminCreated = UserManager.Create(user, "!1Admin");
				if (adminCreated.Succeeded)
				{
					UserManager.AddToRole(user.Id, "Admin");
				}
			}
			if (!roleManager.RoleExists("Pacient"))
			{
				var role = new IdentityRole();
				role.Name = "Pacient";
				roleManager.Create(role);
			}
			if (!roleManager.RoleExists("Doctor"))
			{
				var role = new IdentityRole();
				role.Name = "Doctor";
				roleManager.Create(role);
			}
			if (!roleManager.RoleExists("Receptie"))
			{
				var role = new IdentityRole();
				role.Name = "Receptie";
				roleManager.Create(role);
			}
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

		//public void ConfigureServices(IServiceCollection services)
		//{
		//	services.AddControlers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
		//}
	}
}
