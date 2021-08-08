using LearningQA.Client.Model;
using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LearningQA.Client.ViewModel
{

	public interface IExamViewModel
	{
		ExamViewModelPersist ExamViewModelPersist { get; set; }
		Task RetriveTestItemInfos(int testItemId);
		
		Task<List<ExamInfoModel>> RetriveExamInfoModels(TestItemInfo testItemInfo);
		Task OnLoadCommand();
		Task OnExamLoad(int testId);
		Task OnCombineExamLoad(int testId);
		Task OnExamDelete(int testId);

	}
	public class ExamViewModel : IExamViewModel
	{
		public ITestItemModel testItemModel;
		public ExamViewModelPersist ExamViewModelPersist { get; set; }
		private IPersonInfoPersist _personInfoPersist; 
		public ExamViewModel(ITestItemModel testItemModel, ExamViewModelPersist examViewModelPersist, IPersonInfoPersist personInfoPersist)
		{
			this.testItemModel = testItemModel;
			ExamViewModelPersist = examViewModelPersist;
			_personInfoPersist = personInfoPersist;
		}
		public async Task<List<ExamInfoModel>> RetriveExamInfoModels(TestItemInfo testItemInfo)
		{
			var ressult = await testItemModel.RetriveExamInfoModels(testItemInfo,_personInfoPersist.SelectedPerson.Id);
			return ressult;
		}
		public async Task RetriveTestItemInfos(int testItemId)
		{
			if (ExamViewModelPersist.Initialize)
				return;
			await testItemModel.RetriveTestItemInfos();
			ExamViewModelPersist.TestItemInfos = testItemModel.TestItemInfos.ToList();
		}
		public async Task OnExamDelete(int id)
		{
			var result = await testItemModel.DeleteExam(id);
			if(result.IsSucced)
			{
				var exam = ExamViewModelPersist.ExamInfoModels.Where(x => x.TestId == id).FirstOrDefault();
				ExamViewModelPersist.ExamInfoModels.Remove(exam);
			}
			else
			{
				//show message
			}
		}
		public async Task OnExamLoad(int testId)
		{
			try
			{
				var result = await testItemModel.LoadTest(testId);
				if (result != null)
				{
					await ProccessLoadExamCommand(result);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{MethodInfo.GetCurrentMethod().Name} {ex.Message}");
			}
		}
		public async Task OnCombineExamLoad(int testId)
		{
			try
			{
				var testItemId = ExamViewModelPersist.ExamInfoModels.Where(x => x.TestId == testId).FirstOrDefault().TestItemId;
				ListOfIds<int> listOfIds = new ListOfIds<int>();
				
				var l = ExamViewModelPersist.ExamInfoModels.Where(x => x.TestItemId == testItemId).ToList();
				l.ForEach((x) => listOfIds.Ids.Add(x.TestId));
				var result = await testItemModel.LoadCombineTests(listOfIds, ExamViewModelPersist.GetExamQuestionFilter);
				if (result != null)
				{
					await ProccessLoadExamCommand(result);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{MethodInfo.GetCurrentMethod().Name} {ex.Message}");
			}
		}
		private async Task ProccessLoadExamCommand(ExamModel result)
		{
			var testItemInfo = await testItemModel.RetriveTestItemInfo(result.Test.TestItemId);
			if (testItemInfo != null)
			{
				
				
				ExamViewModelPersist.SelectedCategory = testItemInfo.Category;
				ExamViewModelPersist.SelectedSubjecte = testItemInfo.Subject;
				ExamViewModelPersist.SelectedChapter = testItemInfo.Chapter;
				ExamViewModelPersist.CurrentTest = result.Test;
				ExamViewModelPersist.CurrentTest.Answers = result.Test.Answers.OrderBy(x => int.Parse(x.QUestionSql.QuestionNumber)).ToList();
				for (int i = 0; i < ExamViewModelPersist.CurrentTest.Answers.Count(); i++)
				{
					var item = ExamViewModelPersist.CurrentTest.Answers.ElementAt(i).SelectedAnswer;
					if (item == null)
					{
						item = new List<AnswareOption<int>>();
						ExamViewModelPersist.CurrentTest.Answers.ElementAt(i).SelectedAnswer = item;
					}
				}
				ExamViewModelPersist.FilteredAnsware = result.Test.Answers;
				ExamViewModelPersist.CurrentQuestion = 1;
				ExamViewModelPersist.CurrentTest.Duration = result.Duration;
				ExamViewModelPersist.SelectedQuestion = ExamViewModelPersist.CurrentTest.Answers.ElementAt(0).QUestionSql;
				ExamViewModelPersist.Changed();
			}
		}
		

		public async Task OnLoadCommand()
		{
			try
			{
				TestItemInfo testItemInfo = new TestItemInfo()
				{
					Category = ExamViewModelPersist.SelectedCategory,
					Subject = ExamViewModelPersist.SelectedSubjecte,
					Chapter = ExamViewModelPersist.SelectedChapter
				};
				var result = await testItemModel.RetriveExamInfoModels(testItemInfo, _personInfoPersist.IsSelecedAll ? 0 :_personInfoPersist.SelectedPerson.Id);
				if(result != null)
				{
					ExamViewModelPersist.ExamInfoModels = result;
					
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine($"ExamVieModel.OnLoadCommand {ex.Message}");
			}
			 await Task.CompletedTask;
		}
	}
}
