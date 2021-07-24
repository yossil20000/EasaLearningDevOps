using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.DTO
{
	public class PersonInfoDto
	{
		public PersonInfoDto() { }
		[Required]
		public string IdNumber { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		public string Phone { get; set; }
		public string Address { get; set; }
		[Required]
		[PasswordPropertyText]
		public string Password { get; set; }
		public int Id { get; set; }
		public bool IsChanged { get; set; } = false;

	}
}
