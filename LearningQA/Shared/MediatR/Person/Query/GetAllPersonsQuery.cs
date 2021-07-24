using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.RequestWrapper;
using LearningQA.Shared.MediatR.TestItem.Command;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using ServiceResult;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearningQA.Shared.MediatR.Person.Query
{
	public class GetAllPersonsQuery : IRequestWrapper<PersonInfoDto[]>
	{

	}

	[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public class GetAllPersonsHandler : BaseDBContextHandler, IHandlerWrapper<GetAllPersonsQuery, PersonInfoDto[]>
	{
		public GetAllPersonsHandler(LearningQAContext context, ILogger<BaseDBContextHandler> logger) : base(context, logger)
		{
		}

		public async Task<Result<PersonInfoDto[]>> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var result = await dbContext.Person.AsNoTracking().ToArrayAsync();
				PersonInfoDto[] personInfoDto = new PersonInfoDto[result.Count()];
				for(var i=0; i < result.Count();i++)
				{
					PersonInfoDto p = new()
					{
						Id = result[i].Id,
						IdNumber = result[i].IdNumber,
						Name = result[i].Name,
						Email = result[i].Email,
						Phone = result[i].Phone,
						Address = result[i].Address,
						Password = result[i].Password
					};
					personInfoDto[i] = p;
				}
				return new SuccessResult<PersonInfoDto[]>(personInfoDto);
			}
			catch(Exception ex)
			{
				return new UnexpectedResult<PersonInfoDto[]>(ex.Message);
			}
		}

		private string GetDebuggerDisplay()
		{
			return ToString();
		}
	}
}
