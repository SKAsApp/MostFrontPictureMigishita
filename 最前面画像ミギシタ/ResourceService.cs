using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;	// INotifyPropertyChanged
using System.Runtime.CompilerServices;	// CallerMemberName
using System.Globalization;	// CultureInfo

namespace 最前面画像ミギシタ
{
	/// <summary>
	/// 多言語化されたリソースと、言語の切り替え機能を提供します。
	/// </summary>
	public class ResourceService: INotifyPropertyChanged
	{
		#region singleton members
		private static readonly ResourceService _current = new ResourceService( );
		public static ResourceService Current
		{
			get
			{
				return _current;
			}
		}
		#endregion

		private readonly 最前面画像ミギシタ.Properties.Resources _resources = new 最前面画像ミギシタ.Properties.Resources( );

		/// <summary>
		/// 多言語化されたリソースを取得します。
		/// </summary>
		public 最前面画像ミギシタ.Properties.Resources Resources
		{
			get
			{
				return this._resources;
			}
		}

		#region INotifyPropertyChanged members
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = this.PropertyChanged;
			if(handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion

		/// <summary>
		/// 指定されたカルチャ名を使用して、リソースのカルチャを変更します。
		/// </summary>
		/// <param name="name">カルチャの名前。</param>
		public void ChangeCulture(string name)
		{
			最前面画像ミギシタ.Properties.Resources.Culture = CultureInfo.GetCultureInfo(name);
			this.RaisePropertyChanged("Resources");
		}

	}


}
