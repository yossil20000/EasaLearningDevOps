using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningQA.Shared.Entities;
using LearningQA.Shared.Interface;

using Microsoft.Extensions.Logging;

using LearningQA.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearningQA.Shared.Entities
{
	public class LearningQAContext : DbContext
	{
		protected readonly IConfiguration Configuration;
		//protected readonly IServiceCollection services;
		
		public LearningQAContext(IConfiguration configuration/*, IServiceCollection service*/) 
		{
			Configuration = configuration;
			//this.services = service;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			DataBaseConfig dbConfig = new DataBaseConfig();
			var a = Configuration.GetSection(DataBaseConfig.ConfigSection);

			optionsBuilder.UseSqlite(@$"Data Source=.\LearningQAContext1.db")
			.UseLazyLoadingProxies()
			.EnableSensitiveDataLogging()
			.EnableDetailedErrors()
			.LogTo(Console.WriteLine, LogLevel.Information);
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Person<int>>().HasIndex(person => person.IdNumber).IsUnique();
			modelBuilder.Entity<TestItem<QUestionSql, int>>().HasIndex(testItem => new { testItem.Category, testItem.Chapter, testItem.Subject, testItem.Version }).IsUnique();
			modelBuilder.ApplyConfiguration(new PersonConfiguration());
			//modelBuilder.Entity<QUestionSql>().HasIndex(qUestionSql => qUestionSql.QuestionNumber).IsUnique();
		}

		public virtual DbSet<Person<int>> Person { get; set; }
		public virtual DbSet<AnswareOption<int>> AnswareOptions { get; set; }
		public virtual DbSet<QuestionOption<int>> QuestionOptions { get; set; }
		public virtual DbSet<Answer<int>> Answers { get; set; }
		public virtual DbSet<Supplement<int>> Supplements { get; set; }
		public virtual DbSet<Category<int>> Categories { get; set; }
		public virtual DbSet<TestItem<QUestionSql, int>> TestItems { get; set; }
		public virtual DbSet<QUestionSql> QUestionSqls { get; set; }
		public virtual DbSet<Test<QUestionSql, int>> Tests { get; set; }

	}
	

	
}
