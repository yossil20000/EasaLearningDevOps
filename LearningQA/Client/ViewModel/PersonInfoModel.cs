using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using LearningQA.Client.Model;
using LearningQA.Shared.DTO;

namespace LearningQA.Client.ViewModel
{
	public interface IPersonInfoModel
	{
		Task InitializeAsync();
		Task LoadPersonsInfoAsync();
		Task<Response> DeletePersonInfoAsync(PersonInfoDto personInfoModel);
		Task<int> AddPersonInfoAsync(PersonInfoDto personInfoModel);
		Task SaveAsync();
		
	}
	public interface IPersonInfoPersist
	{
		public bool IsSelecedAll { get; set; }
		public PersonInfoDto SelectedPerson { get; set; }
		public List<PersonInfoDto> Persons { get; set; }
	}
	public class PersonInfoPersist : IPersonInfoPersist
	{
		public PersonInfoDto SelectedPerson { get; set; }
		public List<PersonInfoDto> Persons { get; set; }
		public bool IsSelecedAll { get; set; }
	}
	public class PersonInfoModel : IPersonInfoModel
	{
		private HttpClient _httpClient;
		private IPersonInfoPersist _personInfoPersist;
		public PersonInfoModel(HttpClient httpClient, IPersonInfoPersist personInfoPersist )
		{
			_httpClient = httpClient;
			_personInfoPersist = personInfoPersist;
		}
		public async Task  InitializeAsync()
		{
			
			await LoadPersonsInfoAsync();
			
		}
		public async Task LoadPersonsInfoAsync()
		{
			
			
			var result =  await _httpClient.GetFromJsonAsync<PersonInfoDto[]>($"api/Person/PersonsInfo");
			_personInfoPersist.Persons = result.ToList();
			_personInfoPersist.SelectedPerson = _personInfoPersist?.Persons.FirstOrDefault();
		}
		public async Task<Response> DeletePersonInfoAsync(PersonInfoDto personInfoModel)
		{
			return new Response(false, "Not Implemented");
		}
		public async Task<int> AddPersonInfoAsync(PersonInfoDto personInfoModel)
		{
			_personInfoPersist.Persons.Add(personInfoModel);
			return -1;
		}
		public async Task SaveAsync()
		{
			var personToAdd = _personInfoPersist.Persons.Where(x => x.Id == 0);
			foreach(var p in personToAdd)
			{
				await _httpClient.PostAsJsonAsync<PersonInfoDto>($"api/Person/Create", p);
			}
			var personToUpdate = _personInfoPersist.Persons.Where(x => x.IsChanged);
			foreach (var p in personToUpdate)
			{
				await _httpClient.PutAsJsonAsync<PersonInfoDto>($"api/Person/UpdatePerson", p);
			}
		}
		
	}
}
