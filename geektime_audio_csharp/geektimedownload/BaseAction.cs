using System;
using System.Collections.Generic;
using System.Text;

namespace geektime_audio
{
	public abstract class BaseAction
	{
		private readonly string _actionName;

		public BaseAction(string actionName)
		{
			_actionName = actionName;
		}

		public string Name => _actionName;

		public virtual void Handle(Dictionary<string, object> parameters)
		{
			Console.WriteLine($"------------------------------------- {_actionName} -----------------------------------------");
			Console.WriteLine("Begin execute...");
			try
			{
				DoHandle(parameters);
				Console.WriteLine($"End execute. '{_actionName}' done! ");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Oh no! we have an exception, the action({_actionName}) execute failed! reason is: {ex.Message}\n{ex.StackTrace}");
			}
			Console.WriteLine($"------------------------------------- {_actionName} -----------------------------------------");
		}

		protected abstract void DoHandle(Dictionary<string, object> parameters);

		protected virtual bool ConfimOperation(string message)
		{
			Console.WriteLine($"{message}，请选择 y/n 执行操作，y 为确认， n 为取消！");
			var operation = Console.ReadLine();
			if (string.Equals("y", operation))
			{
				return true;
			}
			else
			{
				Console.WriteLine("用户取消操作！");
				return false;
			}
		}
	}

}
