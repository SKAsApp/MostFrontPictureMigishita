using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

// using System.Drawing;
using System.Diagnostics;	// Process
using System.Runtime.InteropServices;	// DLL Import


//次の作業
//公開
//｛（時報機能）＆（どうがさいせいきのう）追加｝＆｛自作メッセージボックス｝
// セカンドディスプレイへの対応

namespace 最前面画像ミギシタ 
{
	/// <summary>
	/// Window2.xaml の相互作用ロジック
	/// </summary>
	public partial class Window2: Window 
	{
		#region SetThreadExecutionState   function
		[FlagsAttribute]
		public enum ExecutionState: uint
		{
			Null = 0,
			SystemRequired = 1,
			DisplayRequired = 2,
			Continuous = 0x80000000,
		}
		[DllImport("kernel32.dll")]
		extern static ExecutionState SetThreadExecutionState(ExecutionState esFlags);
		#endregion

		internal string ImageSource;
		internal long PositionIndex;
		internal bool VideoMode;
		private bool iconMode = true;
		private bool avoidSleepMode = false;
		WindowStateManagement WindowStateManagement;

		public Window2( ) 
		{
			InitializeComponent( );
			WindowStateManagement = new WindowStateManagement(this);
			return;
		}

		private void Window2_ContentRenderd(object sender, EventArgs e) 
		{
			SetPosition( );
			SetImageOrVideo( );
			WindowStateManagement.PreviousWindowState = 1;
			LowPriority( );
			MenuItemDoNotSleep.IsChecked = avoidSleepMode;
			return;
		}

		private void SetPosition( )
		{
			long display_width = (long) SystemParameters.WorkArea.Width;
			long display_height = (long) SystemParameters.WorkArea.Height;
			this.MinWidth = 213;
			this.MinHeight = 120;
			switch(PositionIndex)
			{
				case 1:
					this.Left = display_width - this.Width;
					this.Top = display_height - this.Height;
					break;
				case 2:
					this.Left = display_width - this.Width;
					this.Top = 0;
					break;
				case 3:
					this.Left = 0;
					this.Top = 0;
					break;
				case 4:
					this.Left = 0;
					this.Top = display_height - this.Height;
					break;
				case 5:
					this.Left = (display_width - this.Width) / 2;
					this.Top = (display_height - this.Height) / 2;
					break;
			}
			return;
		}

		private void SetImageOrVideo( )
		{
			if(!VideoMode)
			{
				MediaElement1.Visibility = System.Windows.Visibility.Collapsed;
				SetImage( );
			}
			else
			{
				Image1.Visibility = System.Windows.Visibility.Collapsed;
				SetVideo( );
			}
			return;
		}

		private void SetImage( )
		{
			BitmapImage bitmap1 = new BitmapImage( );
			this.VisualBitmapScalingMode = BitmapScalingMode.Fant;
			bitmap1.BeginInit( );
			bitmap1.UriSource = new Uri(ImageSource, UriKind.Absolute);
			bitmap1.EndInit( );
			Image1.Source = bitmap1;
			return;
		}

		private void SetVideo( )
		{
			this.VisualBitmapScalingMode = BitmapScalingMode.Fant;
			MediaElement1.Source = new Uri(ImageSource, UriKind.Absolute);
			return;
		}

		private void LowPriority( )
		{
			Process thisProcess = System.Diagnostics.Process.GetCurrentProcess( );
			thisProcess.PriorityClass = ProcessPriorityClass.Idle;
			return;
		}

		private void Window2_KeyUp(object sender, KeyEventArgs e)
		{
			switch(WindowStateManagement.PreviousWindowState)
			{
				case 3:
					switch(e.Key)
					{
						case Key.Escape:
							ChangeToFull( );
							break;
					}
					break;
			}
			return;
		}

		private void Window2_StateChanged(object sender, EventArgs e)
		{
			WindowStateManagement.MigishitaWindowStateChanged( );
			MenuItemFullscreen.Header = WindowStateManagement.MenuItemFullscreen;
			return;
		}

		private void ReturnWindow1(object sender, RoutedEventArgs e)
		{
			MainWindow window_1 = new MainWindow( );
			window_1.Show( );
			this.Close( );
			return;
		}

		private void Hide(object sender, RoutedEventArgs e)
		{
			this.WindowState = System.Windows.WindowState.Minimized;
			return;
		}

		private void ChangeToFullscreen(object sender, RoutedEventArgs e)
		{
			ChangeToFull( );
		}

		private void ChangeToFull( )
		{
			if(WindowStateManagement.PreviousWindowState == 1)
			{
				this.WindowState = System.Windows.WindowState.Maximized;
			}
			else if(WindowStateManagement.PreviousWindowState == 3)
			{
				this.WindowState = System.Windows.WindowState.Normal;
			}
			return;
		}

		private void GoBackPosition(object sender, RoutedEventArgs e)
		{
			this.WindowState = System.Windows.WindowState.Normal;
			ResetSize( );
			SetPosition( );
			return;
		}

		private void ResetSize( )
		{
			this.Width = 426;
			this.Height = 240;
			return;
		}

		private void ChangeDoNotSleep(object sender, RoutedEventArgs e)
		{
			ChangeDoNotSleepMode( );
			return;
		}

		internal void ChangeDoNotSleepMode( )
		{
			if(avoidSleepMode)
			{
				SetThreadExecutionState(ExecutionState.Continuous);
				avoidSleepMode = false;
				MenuItemDoNotSleep.IsChecked = avoidSleepMode;
			}
			else
			{
				SetThreadExecutionState(ExecutionState.SystemRequired | ExecutionState.DisplayRequired | ExecutionState.Continuous);
				avoidSleepMode = true;
				MenuItemDoNotSleep.IsChecked = avoidSleepMode;
			}
			return;
		}

		internal void ChangeDoNotSleepMode(bool to)
		{
			if(to)
			{
				SetThreadExecutionState(ExecutionState.SystemRequired | ExecutionState.DisplayRequired | ExecutionState.Continuous);
				avoidSleepMode = true;
				MenuItemDoNotSleep.IsChecked = avoidSleepMode;
			}
			else
			{
				SetThreadExecutionState(ExecutionState.Continuous);
				avoidSleepMode = false;
				MenuItemDoNotSleep.IsChecked = avoidSleepMode;
			}
			return;
		}

		internal void ChangeColorA( )
		{
			Grid1.Background = new SolidColorBrush(Color.FromArgb(0x19, 0x00, 0x00, 0x00) ) ;
			return;
		}

		internal void ChangeColorK( )
		{
			Grid1.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));
			return;
		}

		private void MediaElement1_0dB(object sender, RoutedEventArgs e)
		{
			MediaElement1.Volume = 1.00;
			return;
		}

		private void MediaElement1_18dB(object sender, RoutedEventArgs e)
		{
			MediaElement1.Volume = 0.13;
			return;
		}

		private void MediaElement1_Play(object sender, RoutedEventArgs e)
		{
			MediaElement1.Position = new TimeSpan(0, 0, 0);
			MediaElement1.Play( );
			return;
		}

		private void MediaElement1_Stop(object sender, RoutedEventArgs e)
		{
			MediaElement1.Stop( );
			return;
		}

		private void EndApp(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
			return;
		}

		internal void ChangeIcon( )
		{
			if(iconMode)
			{
				iconMode = false;
				this.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/表示なしアイコン.ico", UriKind.Absolute));
			}
			else
			{
				iconMode = true;
				this.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/表示ありアイコン.ico", UriKind.Absolute));
			}
			return;
		}

		

	}
}
