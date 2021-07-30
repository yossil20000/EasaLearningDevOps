using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using LearningQA.Client.Model;
using LearningQA.Client.PageBase;
using LearningQA.Shared.DTO;
using LearningQA.Shared.Entities;

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
		public Preferance<int> Preferance { get; set; }
		public void OnChanged(Action callBack);

		public void OnUnChanged(Action callBack);
	}
	public class PersonInfoPersist : PersistanceEventBase, IPersonInfoPersist
	{
		private PersonInfoDto _selectedPerson;
		public PersonInfoDto SelectedPerson { get { return _selectedPerson; } set { _selectedPerson = value; Changed(); } }
		private List<PersonInfoDto>  _Persons;
		public List<PersonInfoDto> Persons { 
			get { return _Persons; }
			set { _Persons = value; SelectedPerson = _Persons.FirstOrDefault();  } }
		public bool IsSelecedAll { get; set; }
		private Preferance<int> _preferance;
		public Preferance<int> Preferance { 
			get { return _selectedPerson == null ? new Preferance<int>() : _selectedPerson.Preferance; } 
			set { 
				if(_selectedPerson != null)
				{
					_selectedPerson.Preferance.Theme = value.Theme;
					_selectedPerson.Preferance.HUE = value.HUE;
					Changed();
				}
			}
		}
	}
	public class PersonInfoModel :  IPersonInfoModel
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
