using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows;
using System.Windows.Input;


namespace 最前面画像ミギシタ
{
	/// <summary>
	/// ウィンドウ状態を管理します。
	/// </summary>
	internal class WindowStateManagement
	{
		Window2 WindowMigishita;
		private long _PreviousWindowState = 0;	// 0：未定義（エラー用），1：通常，2：最小化，3：最大化
		string _MenuItemFullscreen;
		internal long PreviousWindowState
		{
			get
			{
				return _PreviousWindowState;
			}
			set
			{
				_PreviousWindowState = value;
			}
		}
		internal string MenuItemFullscreen
		{
			get
			{
				return _MenuItemFullscreen;
			}
		}

		/// <summary>
		/// WindowStateManagementのインスタンスを生成し，初期化します。
		/// </summary>
		/// <param name="window2">ミギシタのインスタンス</param>
		internal WindowStateManagement(Window2 window2)
		{
			WindowMigishita = window2;
			return;
		}

		/// <summary>
		/// MigishitaのWindowStateが変わったときの処理を行います。
		/// </summary>
		internal void MigishitaWindowStateChanged( )
		{
			// ① さっき最小かつ今は通常または最大　または　② さっき通常または最大かつ今は最小 ⇒ アイコン変更
			if((_PreviousWindowState == 2 && (WindowMigishita.WindowState == System.Windows.WindowState.Normal || WindowMigishita.WindowState == System.Windows.WindowState.Maximized)) || ((_PreviousWindowState == 1 || _PreviousWindowState == 3) && WindowMigishita.WindowState == System.Windows.WindowState.Minimized))
			{
				ChangeIcon( );
			}
			// 最大化 ⇒ マウスカーソル非表示　違ったら　最大化から戻された ⇒ マウスカーソル表示 ＆ スリープ防止 ＆ 背景色
			if(WindowMigishita.WindowState == System.Windows.WindowState.Maximized)
			{
				ChangeToFull( );
			}
			else if(_PreviousWindowState == 3)
			{
				ChangeToNormal( );
			}
			// 今の状態をPreviousWindowStateに反映
			RecordNowWindowState(WindowMigishita.WindowState);
			return;
		}

		private void ChangeIcon( )
		{
			WindowMigishita.ChangeIcon( );
			return;
		}

		private void ChangeToFull( )
		{
			WindowMigishita.ChangeColorK( );
			WindowMigishita.Cursor = Cursors.None;
			WindowMigishita.ChangeDoNotSleepMode(true);
			_MenuItemFullscreen = "通常表示にする";
			return;
		}

		private void ChangeToNormal( )
		{
			WindowMigishita.ChangeColorA( );
			WindowMigishita.Cursor = Cursors.Arrow;
			WindowMigishita.ChangeDoNotSleepMode(false);
			_MenuItemFullscreen = "全画面にする";
			return;
		}

		private void RecordNowWindowState(System.Windows.WindowState windowState)
		{
			if(windowState == System.Windows.WindowState.Normal)
			{
				_PreviousWindowState = 1;
			}
			else if(windowState == System.Windows.WindowState.Minimized)
			{
				_PreviousWindowState = 2;
			}
			else if(windowState == System.Windows.WindowState.Maximized)
			{
				_PreviousWindowState = 3;
			}
			return;
		}


	}
}
