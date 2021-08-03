using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using MediatR;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.TestItem.Command
{
	public class CreateRangeTestItemCommand : IRequest<int>
	{
		public List<TestItem<QUestionSql, int>> _testItems;
		public Person<int> _person;
		public CreateRangeTestItemCommand(List<TestItem<QUestionSql, int>> testItems , Person<int> person = null)
		{
			_testItems = testItems;
			_person = person;

		}
	}

	public class CreateRangeTestItemCommandHandler : IRequestHandler<CreateRangeTestItemCommand, int>
	{
		private readonly LearningQAContext dbContext;
		ILogger<CreateRangeTestItemCommand> logger;

		public CreateRangeTestItemCommandHandler(LearningQAContext context, ILogger<CreateRangeTestItemCommand> logger)
		{
			dbContext = context;
			this.logger = logger;
		}

		public async  Task<int> Handle(CreateRangeTestItemCommand request, CancellationToken cancellationToken)
		{
			try
			{
				if(request._person != null && request._person.Id == 0)
				{
					if(!dbContext.Person.Where(x => x.IdNumber == request._person.IdNumber).Any())
						dbContext.Person.Add(request._person);
				}
				
				dbContext.TestItems.AddRange(request._testItems);
				var result = await dbContext.SaveChangesAsync();
				return result;
				

			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Exception in file{request._testItems.FirstOrDefault().GeTestItemTitle()} ");
				logger.LogError(ex.Message);
				return 0;
			}
			
		}

		
	}
}
