using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace 最前面画像ミギシタ 
{
	/// <summary>
	/// App.xaml の相互作用ロジック
	/// </summary>
	/// 

	public partial class App: Application 
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			string[ ] args = e.Args;
			getStartFilePath.StartFilePath = "";
			foreach(string temp in args)
			{
				getStartFilePath.StartFilePath = temp;
			}
			return;
		}

	}

	public class getStartFilePath
	{
		private static string _StartFilePath;
		internal static string StartFilePath
		{
			get
			{
				return _StartFilePath;
			}
			set
			{
				_StartFilePath = value;
			}
		}

	}
}
