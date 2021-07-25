using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;
using LearningQA.Shared.MediatR.Test.Command;

using ServiceResult;
using WebExtention;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LearningQA.Client.ViewModel;
using System.Web;

namespace LearningQA.Client.Model
{
	public record Response(bool IsSucced,string Message);
	public interface ITestItemModel
	{
		IEnumerable<TestItemInfo> TestItemInfos { get;  }
		Task RetriveTestItemInfos();
		Task<TestItemInfo> RetriveTestItemInfo(int testItemId);
		Task<TestItem<QUestionSql,int>> RetriveTestItem(TestItemInfo testItemInfo);
		Task<List<ExamInfoModel>> RetriveExamInfoModels(TestItemInfo testItemInfo, int personId);
		Task<ExamModel> RetriveTest(TestItemInfo testItemInfo);
		Task<Response> SaveTest(Test<QUestionSql, int> test, int personId);
		Task<ExamModel> LoadTest(int testId);
		Task<ExamModel> LoadCombineTests(ListOfIds<int> testIds, QuestionListFilter questionListFilter);
		Task<Response> DeleteExam(int testId);

	}
	public class TestItemModel : ITestItemModel
	{
		private readonly HttpClient httpClient;
		List<TestItemInfo> testItemInfos;
		public IEnumerable<TestItemInfo> TestItemInfos { get => testItemInfos; private set => testItemInfos = value.ToList(); }
		
		public TestItemModel(HttpClient httpClient)
		{
			this.httpClient = httpClient;

		}


		
		public async Task<List<ExamInfoModel>> RetriveExamInfoModels(TestItemInfo testItemInfo, int personId)
		{
			try
			{
				var result = await httpClient.GetFromJsonAsync<List<ExamInfoModel>>($"api/Exam/GetExamList?category={testItemInfo.Category}&subject={testItemInfo.Subject}&chapter={testItemInfo.Chapter}&personId={personId}");
				return result;
			}
			catch(Exception ex)
			{
				Console.WriteLine($"RetriveExamInfoModels {ex.Message}");
				return new List<ExamInfoModel>();
			}
		}
		public async  Task RetriveTestItemInfos()
		{
			try
			{
				var result = await httpClient.GetFromJsonAsync<IEnumerable<TestItemInfo>>("api/TestItem/TestItemsInfo");
				testItemInfos = result.ToList();
			}
			catch (Exception ex)
			{

				Console.WriteLine($"RetriveTestItemInfos {ex.Message}");
			}
		}

		public async Task<TestItemInfo> RetriveTestItemInfo(int testItemId)
		{
			try
			{
				var result = await httpClient.GetFromJsonAsync<TestItemInfo>($"api/TestItem/TestItemInfo?testItemId={testItemId}");
				return result;
			}
			catch (Exception ex)
			{

				Console.WriteLine($"RetriveTestItemInfo({testItemId}) {ex.Message}");
			}
			return null;
		}
		public async Task<TestItem<QUestionSql, int>> RetriveTestItem(TestItemInfo testItemInfo)
		{
			try
			{
				var result = await httpClient.GetFromJsonAsync<TestItem<QUestionSql, int>>($"api/TestItem/TestItem?category={testItemInfo.Category}&subject={testItemInfo.Subject}&chapter={testItemInfo.Chapter}");

				return result;
			}
			catch (Exception ex)
			{

				Console.WriteLine($"RetriveTestItem({testItemInfo?.Category}) {ex.Message}");
			}
			return null;
		}
		public async Task<ExamModel> RetriveTest(TestItemInfo testItemInfo)
		{
			ExamModel examModel = new ExamModel();
			try
			{

				
				//examModel = await httpClient.GetFromJsonAsync<ExamModel>($"api/Exam/CreateExamByTitle?category={testItemInfo.Category}&subject={testItemInfo.Subject}&chapter={testItemInfo.Chapter}");
				var result = await httpClient.PostAsJsonAsync($"api/Exam/CreateExamByTitle",testItemInfo);
				examModel = await result.Content.ReadFromJsonAsync<ExamModel>();

			}
			catch(NotSupportedException ex)
			{
				Console.WriteLine($"RetriveTest({testItemInfo?.Category}) {ex.Message}");
			}
			catch(Exception ex)
			{
				Console.WriteLine($"RetriveTest({testItemInfo?.Category}) {ex.Message}");
			}
			
			return examModel;
		}
		public async Task<Response> SaveTest(Test<QUestionSql,int> test, int personId)
		{
			try
			{
				// One way
				var response = await httpClient.PutAsJsonAsync($"api/Exam/UpdateExam", new UpdateExamCommand(test, personId));
				if(!response.IsSuccessStatusCode)
				{
					return new Response(false, $"Failed To Save Test: Due To:{response.StatusCode} ");
				}
				return new Response(true, $"Failed To Save Test: Due To:{response.StatusCode} ");

				//Way 2
				var postString = JsonSerializer.Serialize(test); //Need Manually serialized now we use json, we can use other typs
																 //Need to buld the content we will use StringContent
				var postContent = new StringContent(postString, Encoding.UTF8, "application/json"); //We can use "application/xml"

				response = await httpClient.PostAsync($"api/Exam/UpdateExam", postContent);
				var errors = await response.GetErrosIfExistAsync();
				Console.WriteLine(await response.GetPrintableErrosIfExistAsync());
			}
			catch(Exception ex)
			{
				return new Response(false, $"Failed To Save Test: Due To:{ex.Message} ");
			}

		}
		public async Task<ExamModel> LoadTest(int testId)
		{
			
			try
			{
				//var result = await httpClient.GetFromJsonAsync<ExamModel>($"api/Exam/Get?testId={testId}");
				var response = await httpClient.GetAsync($"api/Exam/Get?testId={testId}");
				if(!response.IsSuccessStatusCode)
				{
					return null;
				}
				var responseString = await response.Content.ReadAsStringAsync();
				var examModel = JsonSerializer.Deserialize<ExamModel>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
				return examModel;
			}
			catch(Exception ex)
			{
				Console.WriteLine($"LoadTest({testId}) {ex.Message}");
				return null;
			}
		}

		public async Task<ExamModel> LoadCombineTests(ListOfIds<int> testIds , QuestionListFilter questionListFilter)
		{

			try
			{
				
				NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
				
				foreach(var item in testIds.Ids)
				{
					queryString.Add(nameof(testIds.Ids), item.ToString());
					
				}
				queryString.Add("questionListFilter", questionListFilter.ToString());
				var quer = queryString.ToString();
				HttpUtility httpUtility = new HttpUtility();
				
				HttpUtilityExtentions.Clear();
				HttpUtilityExtentions.BuildeUriQueryCollection<int>(httpUtility ,testIds.Ids, "Ids");
				HttpUtilityExtentions.BuildeUriQueryCollection(httpUtility,"questionListFilter", questionListFilter.ToString());
				var qq = HttpUtilityExtentions.QueryString();
				Console.WriteLine($"With HTTPUtilityExtention: {qq}");
				HttpUtilityExtentions.Clear();
				httpUtility.BuildeUriQueryCollection<int>( testIds.Ids, "Ids")
					.BuildeUriQueryCollection("questionListFilter", questionListFilter.ToString());
				var qq2 = HttpUtilityExtentions.QueryString();
				Console.WriteLine($"With httputil oblect: {qq2}");
				//api/Exam/CombineExamByIds?Ids=11&Ids=23 
				var result = await httpClient.GetFromJsonAsync<ExamModel>($"api/Exam/CombineExamByIds?{qq2}");
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"LoadTest({testIds}) {ex.Message}");
				return null;
			}
		}
		public async Task<Response> DeleteExam(int examId)
		{

			try
			{
				//using native
				var response = await httpClient.DeleteAsync($"api/Exam/DeleteExamById?id={examId}");
				if(!response.IsSuccessStatusCode)
				{
					return new Response(false, response.ReasonPhrase);
					//do something
				}
				var returnStatuse = response.StatusCode switch
				{
					HttpStatusCode.BadRequest => new Response(false, response.ReasonPhrase),
					_ => new Response(true, response.ReasonPhrase)
				};
				//var result = await httpClient.DeleteAsync($"api/Exam/DeleteExamById?id={examId}");
				//result.EnsureSuccessStatusCode();
				return returnStatuse;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"LoadTest({examId}) {ex.Message}");
				return new Response(true, $"Delete Exam Id :{examId}");
				
			}
		}
		
	}
}
