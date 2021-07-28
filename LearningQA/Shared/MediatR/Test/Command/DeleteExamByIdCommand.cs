using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.RequestWrapper;
using LearningQA.Shared.MediatR.TestItem;
using LearningQA.Shared.MediatR.TestItem.Command;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using ServiceResult;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.Test.Command
{
	public class DeleteExamByIdCommand : BaseRequest, IRequestWrapper<int>
	{
		public int Id { get; set; }
		public DeleteExamByIdCommand(int id)
		{
			Id = id;
		}
	}

	public class DeleteExamByIdCommandHandler : BaseDBContextHandler, IHandlerWrapper<DeleteExamByIdCommand, int>
	{
		public DeleteExamByIdCommandHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{
		}

		public async Task<Result<int>> Handle(DeleteExamByIdCommand request, CancellationToken cancellationToken)
		{
			try
			{
			
				//dbContext.ChangeTracker.Clear();
				var exam = await dbContext.Tests.Include(x => x.Answers)
										.ThenInclude(x => x.QUestionSql)
										.ThenInclude(x => x.Options)
									.Include(x => x.Answers)
										.ThenInclude(x => x.QUestionSql)
										.ThenInclude(x => x.Supplements)
									.Include(x => x.Answers)
										.ThenInclude(x => x.SelectedAnswer).Where(x => x.Id == request.Id).FirstOrDefaultAsync();
									
				dbContext.Answers.RemoveRange(exam.Answers);
				dbContext.Remove(exam);
				var result = await dbContext.SaveChangesAsync();
				if (result > 0)
					return await Task.FromResult(new SuccessResult<int>(result));
				else
					return await Task.FromResult(new ServiceResult.InvalidResult<int>("") { Message = "DeleteExamByIdCommand Failed On Delete" });


			}
			catch (Exception ex)
			{
				return await Task.FromResult(new UnexpectedResult<int>(ex.Message) { Message = "DeleteExamByIdCommand Failed" });
			}
		}
	}
}
