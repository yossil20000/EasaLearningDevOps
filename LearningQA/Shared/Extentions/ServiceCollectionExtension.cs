using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using LearningQA.Shared.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using LearningQA.Shared.Configuration;

namespace LearningQA.Shared.Extensions
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection AddApplicationSQLightDbConext(this IServiceCollection services, DataBaseConfig dbConfig)
		{
			services.AddDbContext<LearningQAContext>(options =>
			options.UseSqlite(@$"Data Source=.\{dbConfig.DataBaseInfo.Source}")
			.UseLazyLoadingProxies()
			.EnableSensitiveDataLogging()
			.EnableDetailedErrors()
			.LogTo(Console.WriteLine,LogLevel.Information)
			);
			return services;
		}
		public static IServiceCollection AddApplicationSQLServerDbConext(this IServiceCollection services, DataBaseConfig dbConfig)
		{

			//services.AddDbContext<LearningQAContext,DataContext>(options =>
			//options.UseSqlServer(dbConfig.DataBaseInfo.ConnectionString)
			//.UseLazyLoadingProxies()
			//.EnableSensitiveDataLogging()
			//.EnableDetailedErrors()
			//.LogTo(Console.WriteLine, LogLevel.Information)
			//);
			//return services;
			services.AddDbContext<LearningQAContext,DataContext>(options =>
			options.UseSqlServer(dbConfig.DataBaseInfo.ConnectionString)
			.UseLazyLoadingProxies()
			.EnableSensitiveDataLogging()
			.EnableDetailedErrors()
			.LogTo(Console.WriteLine, LogLevel.Information)
			);
			return services;
		}
	}

	public class DbConfig
	{
	}
}
