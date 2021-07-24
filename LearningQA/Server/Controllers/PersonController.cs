using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using LearningQA.Shared.DTO;
using LearningQA.Shared.MediatR.Person.Command;
using LearningQA.Shared.MediatR.Person.Query;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ServiceResult.ApiExtensions;

namespace LearningQA.Server.Controllers
{
    public class PersonController : ApiControllerBase
    {
        public PersonController(ILogger<ApiControllerBase> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }
        [HttpGet(Name = "/PersonsInfo")]
        public async Task<ActionResult<PersonInfoDto[]>> PersonsInfo()
        {
            var result = await _mediator.Send(new GetAllPersonsQuery());
            return this.FromResult(result);
        }

        [HttpPut(Name = "/UpdatePerson")]
        public async Task<ActionResult<PersonInfoDto>> UpdatePerson([FromBody] PersonInfoDto person)
        {
            var result = await _mediator.Send(new UpdatePersonCommand(person));
            return this.FromResult(result);
        }
        [HttpPost(Name ="/Create")]
        public async Task<ActionResult<int>> Create([FromBody] PersonInfoDto person)
        {
            var  result = await _mediator.Send(new CreatePersonCommand(person));
            return this.FromResult(result);

        }
    }
}
