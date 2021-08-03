using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.RequestWrapper;
using LearningQA.Shared.MediatR.TestItem.Command;

using MediatR;

using Microsoft.Extensions.Logging;

using ServiceResult;

namespace LearningQA.Shared.MediatR.Person.Command
{
	public class CreatePersonCommand : IRequestWrapper<int>
	{
		public PersonInfoDto person;
		public CreatePersonCommand(PersonInfoDto person)
		{
			this.person = person;
		}
	}
	public class CreatePersonCommandHandler : BaseDBContextHandler, IHandlerWrapper<CreatePersonCommand, int>
	{
		public CreatePersonCommandHandler(LearningQAContext context, ILogger<CreatePersonCommandHandler> logger) : base(context, logger)
		{
		}

		public async Task<Result<int>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
		{
			Person<int> p = new Person<int>()
			{
				Id = request.person.Id,
				IdNumber = request.person.IdNumber,
				Name = request.person.Name,
				Email = request.person.Email,
				Phone = request.person.Phone,
				Address = request.person.Address,
				Password = request.person.Password
			};
			var resultPerson = await dbContext.Person.AddAsync(p);
			var result = await dbContext.SaveChangesAsync();
			return new SuccessResult<int>(result);
		}
	}
}
