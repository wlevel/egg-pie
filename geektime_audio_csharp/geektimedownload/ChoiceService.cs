using System;
using System.Collections.Generic;
using System.Linq;
namespace geektimedownload
{
	public class ChoiceService
	{
		private static readonly IList<ActionInfo> _actions = new List<ActionInfo>();
		public static readonly ChoiceService Instance = new ChoiceService();

		private ChoiceService()
		{ }

		public ChoiceService Register(Action<Dictionary<string, object>> action, string name)
		{
			var lastAction = _actions.OrderByDescending(p => p.Sort).FirstOrDefault();
			var newSort = lastAction == null ? 1 : lastAction.Sort + 1;
			_actions.Add(new ActionInfo(action, name, newSort));
			return this;
		}

		public ChoiceService PrintChoice()
		{
			Console.WriteLine("We have commands:");
			var output = string.Empty;
			foreach (var item in _actions.OrderBy(p => p.Sort))
			{
				output += $"{item.Sort}. {item.Name}    ";
			}
			Console.WriteLine(output);
			Console.WriteLine("Please choice you select...");
			return this;
		}

		public ActionInfo Selecting()
		{

			int selected = 1;// val.ToInt32();
			var actionInfo = _actions.FirstOrDefault(p => p.Sort == selected);
			if (actionInfo == null) Console.WriteLine("Select wrong...");
			else Console.WriteLine($"Your choiced action: {actionInfo.Name}");
			return actionInfo;
		}
	}

	public class ActionInfo
	{
		public ActionInfo(Action<Dictionary<string, object>> action, string name, int sort)
		{
			Action = action;
			Name = name;
			Sort = sort;
		}

		public Action<Dictionary<string, object>> Action { get; set; }

		public string Name { get; set; }

		public int Sort { get; set; }
	}
}
