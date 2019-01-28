using geektime_audio;
using System;

namespace geektimedownload
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				InitializeChoiceService();
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex);
				Console.ReadKey();
			}
		}


		static void InitializeChoiceService()
		{
			var downloadAction = new DownloadAction();
			var exitAction = new ExitAction();
			ChoiceService.Instance
				.Register(downloadAction.Handle, downloadAction.Name)
				.Register(exitAction.Handle, exitAction.Name)
				.PrintChoice()
				.Selecting()
				.Action
				.Invoke(null);
			Console.WriteLine("...");
			Console.ReadLine();
		}

	}


}
