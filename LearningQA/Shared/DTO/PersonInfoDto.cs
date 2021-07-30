using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LearningQA.Shared.Entities;

namespace LearningQA.Shared.DTO
{
	public class PersonInfoDto
	{
		public PersonInfoDto() { }
		[Required]
		public string IdNumber { get; set; } = string.Empty;
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		public string Phone { get; set; }
		public string Address { get; set; } = string.Empty;
		[Required]
		[PasswordPropertyText]
		public string Password { get; set; } = string.Empty;
		public int Id { get; set; }
		public bool IsChanged { get; set; } = false;
		public Preferance<int> Preferance { get; set; } = new Preferance<int>() { HUE = 60, Theme = 42 };

	}
}
