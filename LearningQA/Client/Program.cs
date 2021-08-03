using LearningQA.Client.Model;
using LearningQA.Client.Services;
using LearningQA.Client.ViewModel;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using YLBlazor;


namespace LearningQA.Client
{
	public class Program
	{
		private static async Task DebugDelayAsync()
		{
#if DEBUG
			await Task.Delay(500);
#endif
		}
		public static async Task Main(string[] args)
		{
			await DebugDelayAsync();
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			ConfigurationServices(builder.Services);

			
			
			builder.Services.AddHttpClient("TestItemClient", config => 
			{ 
				config.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
				config.DefaultRequestHeaders.Clear();
				config.Timeout = new TimeSpan(0,0,30);
			});
			builder.Services.AddHttpClient<TestItemClient>(option => option.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
			builder.Services.AddScoped<IHttpClientServiceImplementation, HttpClientServiceImplementation>();
			#region Loggin Builder
			//builder.Logging.SetMinimumLevel(LogLevel.Debug);
			//builder.Logging.AddProvider(new  CustomLoggingProvider());
			#endregion
			await builder.Build().RunAsync();
		}
		private static void ConfigurationServices(IServiceCollection services)
		{
			services.AddYLBlazor();
			services.AddCssCustomProperties();
			services.AddScoped<IFetchDataViewModel, FetchDataViewModel>();
			services.AddScoped<IFetchDataModel, FetchData_Model>();
			services.AddScoped<ITestItemModel, TestItemModel>();
			services.AddScoped<IExamViewModel, ExamViewModel>();
			services.AddScoped<ITestItemViewModel, TestItemViewModel>();
			services.AddSingleton<ExamViewModelPersist>();
			services.AddSingleton<TestItemViewModelPersist>();
			services.AddSingleton<IPersonInfoPersist, PersonInfoPersist>();
			services.AddScoped<IPersonInfoModel, PersonInfoModel>();
			var assemblyAll = AppDomain.CurrentDomain.GetAssemblies();
			var assembly = assemblyAll.Where(a => a.FullName.StartsWith("LearningQA.Client")).FirstOrDefault();

			

		}
	}
}
