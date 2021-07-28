using LearningQA.Server.Configuration;
using LearningQA.Server.Infrasructure;
using LearningQA.Shared;
using LearningQA.Shared.Configuration;
using LearningQA.Shared.Entities;
using LearningQA.Shared.Extensions;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LearningQA.Server
{
	
	public class Startup
	{
		//By using IOption
		
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			var config = Configuration.GetSection("LeaningConfig").GetSection("Question").GetValue<int>("PassingSquer");
			//in line
			config = Configuration.GetValue<int>("LeaningConfig:Question:PassingSquer");
			//Binding
			var configBinding = new LeaningConfig();
			Configuration.GetSection("LeaningConfig").Bind(configBinding);
			//ByService, can injected to the control
			services.Configure<LeaningConfig>(Configuration.GetSection(LeaningConfig.ConfigSection));
            var dbConfig = new DataBaseConfig();
            Configuration.GetSection(DataBaseConfig.ConfigSection).Bind(dbConfig);
            services.Configure<DataBaseConfig>(Configuration.GetSection(DataBaseConfig.ConfigSection));
			//Add swagger
			services.AddSwaggerGen();
			//
			//Mediator

			//MediatR Pipeline support
			services.AddHttpContextAccessor();
			//The order is the pipe order
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExampleMediatRPipe<,>));
			//
			//AddMediatR(Assembly.GetExecutingAssembly()
			//provide the assambly where the handler exist
			services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

			//

			//
			//AutoMapper
			services.AddAutoMapper(typeof(Startup));
            //

            //Applicatiob DBContext
            //https://github.com/dotnet/EntityFramework.Docs/blob/main/samples/core/Schemas/TwoProjectMigrations/WorkerService1/Program.cs
            // If you want to inherit From other Context
            //This technic use  lib for each migration
            var provider = Configuration.GetValue("DbConfig:Provider", "");
            services.AddDbContext<LearningQAContext>(
                options => _ = provider switch
                {
                    "UseSqlite" => options.UseSqlite(
                        Configuration.GetValue<string>("DbConfig:ConnectionString:SqliteConnectionString"),
                        x => x.MigrationsAssembly("LearningContextSqlightMigrations")),

                    "UseSqlServer" => options.UseSqlServer(
                        Configuration.GetValue<string>("DbConfig:ConnectionString:SqlServerConnectionString"),
                        x => x.MigrationsAssembly("LearningContextSqlServerMigrations")),

                    _ => throw new Exception($"Unsupported provider: {provider}")
                });
            //
            services.AddControllersWithViews();
			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env,LearningQAContext dbContext)
		{
            //Apply Migration
            dbContext.Database.Migrate();
            //
            
			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();
			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			//https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});
			//
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
				endpoints.MapFallbackToFile("index.html");
			});
		}
	}
}
