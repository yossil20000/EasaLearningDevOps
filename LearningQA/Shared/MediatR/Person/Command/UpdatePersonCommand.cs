using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.RequestWrapper;
using LearningQA.Shared.MediatR.TestItem.Command;

using Microsoft.Extensions.Logging;

using ServiceResult;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace LearningQA.Shared.MediatR.Person.Command
{
	public class UpdatePersonCommand : IRequestWrapper<PersonInfoDto>
	{
		public PersonInfoDto Person { get; private set; }

		public UpdatePersonCommand(PersonInfoDto person)
		{
			Person = person;
		}
	}

	public class UpdatePersonHandler : BaseDBContextHandler, IHandlerWrapper<UpdatePersonCommand, PersonInfoDto>
	{
		public UpdatePersonHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{
		}

		public async Task<Result<PersonInfoDto>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
		{
			try
			{
				if(request.Person.Id == 0)
				{
					return new ServiceResult.NotFoundResult<PersonInfoDto>("Person index should nt be zero");
				}
				else
				{
					var person = dbContext.Person.Find(request.Person.Id);
					person.IdNumber = request.Person.IdNumber;
					person.Name = request.Person.Name;
					person.Phone = request.Person.Phone;
					person.Email = request.Person.Email;
					person.Address = request.Person.Address;
					person.Password = request.Person.Password;
					 dbContext.Person.Update(person);
					var result = await dbContext.SaveChangesAsync();
					return new SuccessResult<PersonInfoDto>(request.Person);
				}
			}
			catch(Exception ex)
			{
				return new UnexpectedResult<PersonInfoDto>(ex.Message);
			}
		}
	}
}
