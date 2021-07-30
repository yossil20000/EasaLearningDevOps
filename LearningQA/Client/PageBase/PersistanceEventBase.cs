using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.PageBase
{
    public class PersistanceEventBase
    {
		#region Action Events
		protected readonly List<Action> registration = new List<Action>();
		protected readonly Dictionary<RegisterEvent, List<Task>> events = new Dictionary<RegisterEvent, List<Task>>();
		public  void Changed()
		{
			registration.ForEach(a => a());

		}
		public void RegisterEvent(RegisterEvent registerEvent, Task callBack)
		{
			events[registerEvent].Add(callBack);
		}
		public void URegisterEvent(RegisterEvent registerEvent, Task callBack)
		{
			events[registerEvent].Remove(callBack);
		}

		public void OnEventChanged(RegisterEvent registerEvent)
		{
			//events[registerEvent].ForEach(a => Task.Run(() => a));
			var tasks = from a in events[PageBase.RegisterEvent.SelectedSupplement] select a;
			Task.WhenAll(tasks.ToList());
		}
		public void OnChanged(Action callBack)
		{
			registration.Add(callBack);
		}
		public void OnUnChanged(Action callBack)
		{
			try
			{
				registration.Remove(callBack);
			}
			catch (Exception ex)
			{

			}
		}
		#endregion
	}
}
