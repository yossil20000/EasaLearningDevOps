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
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace LearningQA.Shared.Extensions
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection AddApplicationSQLightDbConext(this IServiceCollection services, DataBaseConfig dbConfig)
		{
			services.AddDbContext<LearningQAContext>(opt =>
			//opt.UseSqlite(@$"Data Source=.\{dbConfig.DataBaseInfo.Source}",opt => opt.MigrationsAssembly("LearningQA.Shared"))
			opt.UseSqlite(dbConfig.ConnectionString.SqliteConnectionString, opt => opt.MigrationsAssembly("LearningQA.Shared"))
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
			
			services.AddDbContext<LearningQAContext,DataContext>(opt =>
			//opt.UseSqlServer(dbConfig.DataBaseInfo.ConnectionString, option => option.MigrationsAssembly("LearningQA.Shared"))
			opt.UseSqlServer(dbConfig.ConnectionString.SqlServerConnectionString, option => option.MigrationsAssembly("LearningQA.Shared"))
			.UseLazyLoadingProxies()
			.EnableSensitiveDataLogging()
			.EnableDetailedErrors()
			.LogTo(Console.WriteLine, LogLevel.Information)
			);
			return services;
		}
	}


}
