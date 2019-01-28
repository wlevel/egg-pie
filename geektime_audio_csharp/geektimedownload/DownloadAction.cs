using geektimedownload;
using System;
using System.Collections.Generic;
using System.Text;

namespace geektime_audio
{
	public class DownloadAction : BaseAction
	{
		public DownloadAction() : base(" Download")
		{ }

		protected override void DoHandle(Dictionary<string, object> parameters)
		{
			Console.WriteLine("Please input the file directory path:");
			var confimKey = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(confimKey))
			{
				Console.WriteLine("Please enter the correct path!");
				return;
			}
			DownloadService.Download(confimKey);

		}
	}
}
