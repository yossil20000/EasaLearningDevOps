using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LearningQA.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningQA.Shared.Configuration
{
	public class PersonConfiguration : IEntityTypeConfiguration<Person<int>>
	{
		public void Configure(EntityTypeBuilder<Person<int>> builder)
		{
			builder.HasData(
				new Person<int>()
				{
					Id = 1,
					IdNumber = "135792468",
					Name ="yosef Levy",
					Address = "Gilon",
					Phone ="+97249984222",
					Email ="Yos@gmail.com",
					Password = "12345@12345"
				},
				new Person<int>()
				{
					Id = 2,
					IdNumber = "246813579",
					Name = "Yoni Levy",
					Address = "Gilon",
					Phone = "+97249984220",
					Email = "Yoni@gmail.com",
					Password = "12345@12345"
				});
		}
	}
}
