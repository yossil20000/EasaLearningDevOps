using AutoMapper;
using AutoMapper.Internal;

using Castle.Core.Internal;

using LearningQA.Server.Configuration;
using LearningQA.Server.Infrasructure;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.TestItem.Command;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.Annotations;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningQA.Server.Controllers
{
    public class LearningQADbController :ApiControllerBase
    {
        private const string _loadFromFilePassword = "Tyy2000@Load";
        private readonly LearningQAContext _dbContext;
        DataResourceReader _dataResourceReader;

        public LearningQADbController(DataResourceReader dataResourceReader , ILogger<LearningQADbController> logger, IMediator mediator, IOptions<LeaningConfig> learningConfig, IMapper mapper, LearningQAContext learningQAContext) : base(logger, mediator, mapper)
        {
            _dbContext = learningQAContext;
            _dataResourceReader = dataResourceReader;
        }
        [HttpPost(Name = "/LoadNewFromFile")]
        [SwaggerOperation(
            Summary = "LoadNewFromFile",
            Description = "Load TestITem file in json format and save ti DB, can be also reset database",
            OperationId = "TestItem.Post",
            Tags = new[] { "TestItemEndpoint" })]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "bool", typeof(bool))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, "string", typeof(string))]
        public async Task<IActionResult> LoadNewFromFile(string password = "" ,bool createNewDatabase = false, bool confirmCreate = false , string fileName = @"atpl_oxford\.[0-9A-Z]+\.\w+\.json")
        {
            
            if (createNewDatabase && !confirmCreate || !(password == _loadFromFilePassword) || string.IsNullOrEmpty(fileName))
            {

                return BadRequest("Input createNewDatabase == false while loadAll");
            }
            if(createNewDatabase)
            {
                _dbContext.Database.EnsureDeleted();
                _dbContext.Database.Migrate();
            }
                      
            ICollection<ValidationResult> validationResults = new Collection<ValidationResult>();
            List<TestItem<QUestionSql, int>> filesLoaded;
            var results = await _dataResourceReader.LoadAllJsonAsync(fileName, out filesLoaded);
            await _mediator.Send(new CreateRangeTestItemCommand(filesLoaded, null), cancellationToken);
            var returnMessage = new StringBuilder();
            validationResults.ForAll(s => returnMessage.Append($"{Environment.NewLine}{s.ErrorMessage}"));
            return Ok(returnMessage.ToString());
        }
        [HttpPost(Name = "/WriteDataToJson")]
        public async Task<IActionResult> WriteDataToJson(string password, string source = @"atpl_oxford\.[0-9A-Z]+\.\w+\.json", string desitnation = "")
        {
            if (!(password == _loadFromFilePassword) || string.IsNullOrEmpty(source) || string.IsNullOrEmpty(desitnation))
            {

                return BadRequest("Input ConvertFromJsonToBinary Arguments Not Match");
            }
            List<TestItem<QUestionSql, int>> filesLoaded;
            var results = await _dataResourceReader.LoadAllJsonAsync(source, out filesLoaded);
            var resuttSave = await _dataResourceReader.WriteDataToJsonAsync(filesLoaded, desitnation);
            if(resuttSave)
            {
                return Ok($"Save To {desitnation}  Items Count: {filesLoaded?.Count}");
            }
            return NotFound("SaveJsonAsBinaryAsync Failed");

        }
        [HttpGet(Name = "/ReadDataFromJson")]
        public async Task<IActionResult> ReadDataFromJson(string password, string source = "")
        {
            if (!(password == _loadFromFilePassword) || string.IsNullOrEmpty(source) )
            {

                return BadRequest("Input ConvertFromJsonToBinary Arguments Not Match");
            }
                       
            var resultLoad = await _dataResourceReader.ReadDataFromJsonAsync(source);
            if (resultLoad is not null)
            {
                return Ok(resultLoad);
            }
            return NotFound("SaveJsonAsBinaryAsync Failed");

        }
        [HttpGet(Name = "/LoadImageFromFile")]
        public  ActionResult<string> LoadImageFromFile(string fileName = "")
        {
            var image = _dataResourceReader.LoadImageForDisplay(fileName);
            return image;
        }
    }
}
