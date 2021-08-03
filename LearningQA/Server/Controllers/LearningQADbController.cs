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
        public async Task<IActionResult> LoadNewFromFile(string password = "" ,bool createNewDatabase = false, bool confirmCreate = false , string fileName = "")
        {
            string[] filestoLoad = null;
            if (createNewDatabase && !confirmCreate || !(password == _loadFromFilePassword) || string.IsNullOrEmpty(fileName))
            {

                return BadRequest("Input createNewDatabase == false while loadAll");
            }
            if(createNewDatabase)
            {
                _dbContext.Database.EnsureDeleted();
                _dbContext.Database.Migrate();
            }
            filestoLoad = _dataResourceReader.GetAllJsonFiles(fileName);
            Person<int> person = new Person<int>()
            {
                IdNumber = "059828391",
                Name = "Yosef Levy",
                Email = "yos@gmail.com",
                Address = "Gilon, Israel 2010300",
                Phone = "054999888777"

            };
            BlockingCollection<List<TestItem<QUestionSql, int>>> testItemsLoader = new BlockingCollection<List<TestItem<QUestionSql, int>>>();
            bool mode = true;
            ICollection<ValidationResult> validationResults = new Collection<ValidationResult>();
            var sw = Stopwatch.StartNew();
           
            if (mode)
            {
                foreach (var file in filestoLoad)
                {

                    var result = _dataResourceReader.LoadJsonFullName<TestItem<QUestionSql, int>>(file);
                    if (result == null)
                        continue;
                    if (result.Where(x => string.IsNullOrEmpty(x.Category) || string.IsNullOrEmpty(x.Chapter) || string.IsNullOrEmpty(x.Subject)).Any())
                        continue;
                    if (!ConverSupplement(result))
                        return Ok("ConverSupplement Failed");
                    var supp =result.SelectMany(x => x.Questions).Where(x => x.Supplements.Count > 0).Select(x => x.Supplements.FirstOrDefault());
                    foreach(var s in supp)
                    {
                        _logger.LogTrace($"Test item supplemnts: {s.OriginalContent} {s.Content}");
                    }
                    await _mediator.Send(new CreateRangeTestItemCommand(result, person), cancellationToken);
                    createNewDatabase = false;
                    Debug.WriteLine($"Finished File{file} ");
                }
                sw.Stop();
                _logger.LogTrace($"Without Parallel Tooks {sw.ElapsedMilliseconds} ");
            }
            else
            {


                sw.Restart();
                ParallelLoopResult parallelLoopResult = Parallel.ForEach(filestoLoad, file => _dataResourceReader.LoadJsonFullName<TestItem<QUestionSql, int>>(file, testItemsLoader));
                foreach (var result in testItemsLoader)
                {
                    //int parseResult;

                    //if (result.Where(x => string.IsNullOrEmpty(x.Category) || string.IsNullOrEmpty(x.Chapter) || string.IsNullOrEmpty(x.Subject)).Any())
                    //    continue;
                    int previousValidationResult = validationResults.Count();
                    result.ForEach(x => Validator.TryValidateObject(x, new System.ComponentModel.DataAnnotations.ValidationContext(x), validationResults, true));

                    if (!ConverSupplement(result))
                        validationResults.Add(new ValidationResult($"ConverSupplement failed"));
                    if (validationResults.Count > previousValidationResult)
                        continue;
                    //var test = result.Where(x =>
                    // x.Questions.Where(q => !int.TryParse(q.QuestionNumber, out parseResult)).Any()
                    // );
                    //if (test.Count() > 0)
                    //{
                    //    foreach (var item in test)
                    //    {
                    //        var q = item.Questions.Select(x => x.QuestionNumber).Aggregate((s1,s2) => s1 + " | " + s2);
                    //        Debug.WriteLine($"{item.GeTestItemTitle()} question: {string.Join(" ! ", q) }");
                    //    }

                    //    //return Ok("Question number not valis"); 
                    //}

                    await _mediator.Send(new CreateRangeTestItemCommand(result, person) , cancellationToken);
                    createNewDatabase = false;
                }
                sw.Stop();
                _logger.LogTrace($"With Parallel Tooks {sw.ElapsedMilliseconds}");
            }


            //var testitems = DataResourceReader.LoadJson<TestItem<QUestionSql, int>>(fileName);

            //ConverSupplement(testitems);
            //await _mediator.Send(new CreateRangeTestItemCommand(testitems,person) { CreateNewDatabase = createNewDatabase}, cancellationToken);
            var returnMessage = new StringBuilder();
            validationResults.ForAll(s => returnMessage.Append($"{Environment.NewLine}{s.ErrorMessage}"));
            return Ok(returnMessage.ToString());
        }
        [HttpGet(Name = "/LoadImageFromFile")]
        public  ActionResult<string> LoadImageFromFile(string fileName = "")
        {
            var image = _dataResourceReader.LoadImageForDisplay(fileName);
            return image;
        }
        private bool ConverSupplement(List<TestItem<QUestionSql, int>> items)
        {
            try
            {
                //The you can add aditional info that you can put and it latter can be filter, some third party can useit
                //Don't use string interpolation
                _logger.LogTrace(100,"ConverSupplement: start at {time}" , DateTime.UtcNow);
                for (int item = 0; item < items.Count; item++)
                {
                    _logger.LogTrace($"ConverSupplement: loop on {items.ElementAt(item).GeTestItemTitle()}");
                    for (int qIndex = 0; qIndex < items.ElementAt(item).Questions.Count; qIndex++)
                    {
                        var toRemove = items.ElementAt(item).Questions.ElementAt(qIndex).Supplements.Where(x => x.OriginalContent.IsNullOrEmpty() && x.Title.IsNullOrEmpty()).ToList();
                        _logger.LogTrace($"ConverSupplement: ToRemove Count:  {toRemove.Count}");
                        var supplement = items.ElementAt(item).Questions.ElementAt(qIndex).Supplements;
                        _logger.LogTrace($"ConverSupplement: Supplements Count:  {supplement.Count}");

                        toRemove.ForEach(item => supplement.Remove(item));
                        for (int suppIndex = 0; suppIndex < supplement.Count; suppIndex++)
                        {
                            _logger.LogTrace($"ConverSupplement: Supplements After remove Count:  {supplement.Count} suppIndex: {suppIndex}");
                            switch (supplement.ElementAt(suppIndex).OriginalcontentType)
                            {
                                case ContentType.ImageFileName:
                                case ContentType.ImageFileNameExplain:
                                    {
                                        _logger.LogTrace($"ConverSupplement: item:{item}, Question:{items.ElementAt(item).Questions.ElementAt(qIndex).QuestionNumber} Supp:{supplement.ElementAt(suppIndex).OriginalContent}");
                                        var content = supplement.ElementAt(suppIndex).OriginalContent.Split(";");
                                        if (content.Length > 0)
                                        {
                                            var src = content[0].Split(":")[1];
                                            var srcString = _dataResourceReader.LoadImageForDisplay(src);
                                            _logger.LogTrace($"ConverSupplement: scrString: {srcString}");
                                            supplement.ElementAt(suppIndex).Content = srcString;
                                            supplement.ElementAt(suppIndex).ContentType = ContentType.ImageBase64String;
                                            _logger.LogTrace($"ConverSupplement: scrString: {supplement.ElementAt(suppIndex).Content}");

                                        }

                                    }
                                    break;
                            }
                            //if (supplement.ElementAt(suppIndex).OriginalcontentType == ContentType.ImageFileName)
                            //{
                            //    Console.WriteLine($"ConverSupplement: item:{item}, Question:{items.ElementAt(item).Questions.ElementAt(qIndex).QuestionNumber} Supp:{supplement.ElementAt(suppIndex).OriginalContent}");
                            //    var content = supplement.ElementAt(suppIndex).OriginalContent.Split(";");
                            //    if (content.Length > 0)
                            //    {
                            //        var src = content[0].Split(":")[1];
                            //        supplement.ElementAt(suppIndex).Content = DataResourceReader.LoadImageForDisplay(src);
                            //        supplement.ElementAt(suppIndex).ContentType = ContentType.ImageBase64String;
                            //    }

                            //}
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return false;
            }

        }
    }
}
