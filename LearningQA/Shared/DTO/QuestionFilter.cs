using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Shared.DTO
{
	[Flags]
	public enum QuestionListFilter
	{
		
		All = 0b10000,
		Marked = 0b00001,
		Wrong = 0b00010,
		NotAnswered = 0b00100,
		[Display(Name = "Marked & Wrong")]
		MarkedAndWrong = 0b00011,
		[Display(Name = "Marked & NotAnswered")]
		MarkedAndNotAnswered = 0b00101,
		[Display(Name = "Not Answered & Wrong")]
		NotAnsweredAndWrong = 0b00110

	}
}
