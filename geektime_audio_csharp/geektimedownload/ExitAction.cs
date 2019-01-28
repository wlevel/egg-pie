using System;
using System.Collections.Generic;
using System.Text;

namespace geektime_audio
{
	
	public class ExitAction : BaseAction
	{
		public ExitAction() : base("Exit")
		{ }

		protected override void DoHandle(Dictionary<string, object> parameters)
		{
			Console.WriteLine("Exit, Are you serious? (y/n)");
			var confimKey = Console.ReadLine();
			if (!string.Equals("y", confimKey)) return;

			Console.WriteLine("Good luck for you, bye!");
			Environment.Exit(0);
		}
	}
}
