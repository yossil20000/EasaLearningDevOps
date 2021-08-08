using LearningQA.Client.ViewModel;
using LearningQA.Shared.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Cryptography;
using ObjectTExtensions;
using System.Diagnostics;
using LearningQA.Shared.Entities;
using System.Reflection;

namespace LearningQA.Client.PageBase
{
	public enum RegisterEvent
	{
		SelectedSupplement
	}
	public class PersistanceBase : PersistanceEventBase, IDisposable, IViewPersistanceBase
	{
		
		public PersistanceBase()
		{
			events.Add(PageBase.RegisterEvent.SelectedSupplement, new List<Task>());
		}
		protected List<TestItemInfo> testItemInfos = new List<TestItemInfo>();
		public List<TestItemInfo> TestItemInfos
		{
			get => testItemInfos;
			set { testItemInfos = value; Initialize = true; ProcessTestItemInfo(); }
		}
		TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
		private string _selectedCategory = "";
		public string SelectedCategory
		{
			get => _selectedCategory;
			set { _selectedCategory = value; OnCategoryChanged(); }
		}
		public List<string> Categories { get; set; } = new List<string>();
		//Subject
		private string _selectedSubjecte = "";
		public string SelectedSubjecte
		{
			get => _selectedSubjecte;
			set { _selectedSubjecte = value; OnSubjectChanged(); }
		}
		private void OnSubjectChanged()
		{
			//Chapteres = testItemInfos.Where(x => x.Subject == SelectedSubjecte).Select(x => x.Chapter).Distinct().OrderBy(x => TestTitleFilter(x)).ToList();
			//var querySubject = testItemInfos.Where(x => x.Subject.Contains( SelectedSubjecte,StringComparison.CurrentCultureIgnoreCase));
			Chapteres = testItemInfos.Where(x => x.Subject.Equals(SelectedSubjecte, StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Chapter).Distinct().ToList();
			TitleComparer.ElimanateLast = true;
			Chapteres.Sort(new TitleComparer());
			Debug.WriteLine("OnSubjectChanged");
			SelectedChapter = Chapteres.FirstOrDefault();
			Changed();
		}
		private void OnCategoryChanged()
		{
			//Subjectes = testItemInfos.Where(x => x.Category == SelectedCategory).Select(x => x.Subject).Distinct().OrderBy(x => TestTitleFilter(x)).ToList();
			Subjectes = testItemInfos.Where(x => x.Category.Equals(SelectedCategory,StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Subject).Distinct().ToList();
			TitleComparer.ElimanateLast = true;
			Subjectes.Sort(new TitleComparer() );
			Console.WriteLine("OnCategoryChanged");
			SelectedSubjecte = Subjectes.FirstOrDefault();
			Changed();
		}
		protected int TestTitleFilter(string title)
		{
			var str = title.Split(".");
			if (str.Length > 0)
			{
				
				if(int.TryParse(str[0], out var index))
				return index;
			}
			return 0;

		}
		public List<string> Subjectes { get; set; } = new List<string>();
		//Chapter
		private string _selectedChapter = "";
		public string SelectedChapter { get => _selectedChapter; set { _selectedChapter = value; Changed(); } }
		public List<string> Chapteres { get; set; } = new List<string>();
		public bool Initialize { get; set; } = false;


		public void Dispose()
		{
			
		}
		protected void ProcessTestItemInfo()
		{
			try
			{
				 List<TestItemInfo> itemToRemove = new List<TestItemInfo>();
				for (int i = 0; i < testItemInfos.Count; i++)
				{
					if (string.IsNullOrEmpty(testItemInfos[i].Category) ||
						string.IsNullOrEmpty(testItemInfos[i].Subject) ||
						string.IsNullOrEmpty(testItemInfos[i].Chapter))
						itemToRemove.Add(testItemInfos[i]);
				}
				foreach(var item in itemToRemove)
				{
					testItemInfos.Remove(item);
				}
				itemToRemove = null;
				for (int i = 0; i < testItemInfos.Count; i++)
				{
					if (string.IsNullOrEmpty(testItemInfos[i].Category) ||
						string.IsNullOrEmpty(testItemInfos[i].Subject) ||
						string.IsNullOrEmpty(testItemInfos[i].Chapter))
						continue;
					testItemInfos[i].Category = myTI.ToTitleCase(testItemInfos[i].Category);
					testItemInfos[i].Subject = myTI.ToTitleCase(testItemInfos[i].Subject);
					testItemInfos[i].Chapter = myTI.ToTitleCase(testItemInfos[i].Chapter);
				}

				Categories = testItemInfos.Select(x => x.Category).Distinct().OrderBy(x => TestTitleFilter(x)).ToList();

				Console.WriteLine($"ToTitleCase: {Categories == null}");

				Subjectes = testItemInfos.Select(x => x.Subject).Distinct().OrderBy(x => TestTitleFilter(x)).ToList();

				Chapteres = testItemInfos.Select(x => x.Chapter).Distinct().OrderBy(x => TestTitleFilter(x)).ToList();

				Changed();
			}
			catch (Exception ex)
			{

				Debug.WriteLine($"Exception in {MethodInfo.GetCurrentMethod().Name} {ex.Message}");
			}
		}
	}
}
