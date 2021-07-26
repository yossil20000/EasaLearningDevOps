using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LearningQA.Shared.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LearningQA.Shared.Entities
{
	public  class DataContext : LearningQAContext
	{
		
		public DataContext(IConfiguration configuration/*, IServiceCollection services*/) : base(configuration/*,services*/)
		{
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			DataBaseConfig dbConfig = new DataBaseConfig();
			var a = Configuration.GetSection(DataBaseConfig.ConfigSection);

			optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LearningQAContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
			.UseLazyLoadingProxies()
			.EnableSensitiveDataLogging()
			.EnableDetailedErrors()
			.LogTo(Console.WriteLine, LogLevel.Information);
		}
	}
}
