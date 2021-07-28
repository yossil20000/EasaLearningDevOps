using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.RequestWrapper;
using LearningQA.Shared.MediatR.TestItem.Command;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using ObjectTExtensions;

using ServiceResult;

namespace LearningQA.Shared.MediatR.Test.Query
{
	public class GetExamCombineByIdCommand : IRequestWrapper<ExamModel>
	{
		public List<int> TestsIds { get; private set; }
		public QuestionListFilter QuestionListFilter { get; private set; }

		public GetExamCombineByIdCommand(List<int> testsIds , QuestionListFilter questionListFilter)
		{
			TestsIds = testsIds;
			QuestionListFilter = questionListFilter;
		}
	}
	public class GetExamCombineByIdCommandHandler : BaseDBContextHandler, IHandlerWrapper<GetExamCombineByIdCommand, ExamModel>
	{
		public GetExamCombineByIdCommandHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{
		}

		public async Task<Result<ExamModel>> Handle(GetExamCombineByIdCommand request, CancellationToken cancellationToken)
		{
			
			var cNA = request.QuestionListFilter & QuestionListFilter.NotAnswered;
			var cWr = request.QuestionListFilter & QuestionListFilter.Wrong;
			var bNotAnswered = request.QuestionListFilter.IsFlagSet(QuestionListFilter.NotAnswered);
			var bWrong = request.QuestionListFilter.IsFlagSet(QuestionListFilter.Wrong);
			var bMarked = request.QuestionListFilter.IsFlagSet(QuestionListFilter.Marked);
			var bAll = request.QuestionListFilter.IsFlagSet(QuestionListFilter.All);
			

			try
			{
				var result =  (from test in dbContext.Tests
									.Include(x => x.Answers)
										.ThenInclude(x => x.QUestionSql)
										.ThenInclude(x => x.Options)
									.Include(x => x.Answers)
										.ThenInclude(x => x.QUestionSql)
										.ThenInclude(x => x.Supplements)
									.Include(x => x.Answers)
										.ThenInclude(x => x.SelectedAnswer)
							   where request.TestsIds.Contains(test.Id)
									join testItem in dbContext.TestItems
									on test.TestItemId equals testItem.Id
									select new ExamModel() { Test = test, Duration = testItem.Duration, Title = testItem.GeTestItemTitle() });
				var query = await result
					.SelectMany(test => test.Test.Answers,(test,Answere) =>   Answere)
					.Where(x =>
					bAll || (
					bNotAnswered ? !x.IsAnswered : true &&
					bWrong ? !x.IsCorrect : true  &&
					bMarked ? x.IsMarked : true )).ToListAsync() ;
				var Test = new Test<QUestionSql, int>() { Answers = query.DistinctBy(x => x.QUestionSql.Id).ToList(), TestItemId = result.FirstOrDefault().Test.TestItemId };
				return new SuccessResult<ExamModel>(new ExamModel() { Test = Test,Duration = result.FirstOrDefault().Duration, Title = result.FirstOrDefault().Title });
			}
			catch(Exception ex)
			{
				return new UnexpectedResult<ExamModel>(ex.Message);
			}
		}
		private bool IsAnswareForSelect(Answer<int> answare , QuestionListFilter questionListFilter)
		{
			var notAnswared = !answare.IsAnswered && questionListFilter.IsFlagSet(QuestionListFilter.NotAnswered);
			var notCorected = !answare.IsCorrect && questionListFilter.IsFlagSet(QuestionListFilter.Wrong);
			var marked = answare.IsMarked && questionListFilter.IsFlagSet(QuestionListFilter.Marked);
			return notAnswared || notCorected || marked;
		}
	}
}
