﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LearningQA.Shared.Entities;

namespace LearningQA.Shared.Interface
{
	public interface IPerson<Tdb> : IDb<Tdb> 
	{
		
		string IdNumber { get; set; }
		string Name { get; set; }
		string Email { get; set; }
		string Phone { get; set; }
		string Address { get; set; }
		string Password { get; set; } 
		Preferance<Tdb> Preferance { get; set; }
	}
}
