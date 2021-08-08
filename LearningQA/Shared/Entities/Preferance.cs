using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LearningQA.Shared.Interface;

namespace LearningQA.Shared.Entities
{
	public class Preferance<Tdb> : IPreferance<Tdb>
	{
		public Tdb? Id { get; set; }
		public int Theme { get; set; } = 42;
		public int HUE { get; set; } = 60;

	}
}
