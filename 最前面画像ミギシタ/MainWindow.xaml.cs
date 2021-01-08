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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;	//ファイルダイアログ
using System.Text.RegularExpressions;	// 正規表現
using System.IO;	// File存在


namespace 最前面画像ミギシタ 
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>

	public partial class MainWindow: Window 
	{
		public MainWindow( ) 
		{
			InitializeComponent( );
		}

		private long count = 0;
		private static string imageSource = "";
		private bool VideoMode;

		private void Window1_ContentRenderd(object sender, EventArgs e)
		{
			count += 1;
			if(getStartFilePath.StartFilePath == "")
			{
				if(imageSource == "")
				{
					Keyboard.Focus(TextBoxInput);
				}
				else
				{
					TextBoxInput.Text = imageSource;
				}
			}
			else
			{
				TextBoxInput.Text = getStartFilePath.StartFilePath;
				if(count >= 2)
				{
					getStartFilePath.StartFilePath = "";
				}
				else
				{
					ButtonRun_Clicked( );
				}
			}
			return;
		}
		
		private void ChangeToJapanese(object sender, RoutedEventArgs e)
		{
			ResourceService.Current.ChangeCulture("ja");
			return;
		}

		private void ChangeToEnglish(object sender, RoutedEventArgs e)
		{
			ResourceService.Current.ChangeCulture("en");
			return;
		}

		private void ViewVersion(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(Properties.Resources.About, Properties.Resources.Version2, MessageBoxButton.OK, MessageBoxImage.Information);
			return;
		}

		private void ButtonInput_Click(object sender, RoutedEventArgs e) 
		{
			//OpenFileDialogクラスのインスタンスを作成
			OpenFileDialog ofd = new OpenFileDialog( );
			ofd.Filter = "画像ファイル (*.png;*.jpg;*.bmp;*.gif)|*.png;*.PNG;*.jpg;*.JPG;*.jpeg;*.JPEG;*.bmp;*.BMP;*.gif;*.GIF|動画ファイル (*.mp4;*.m2ts;*.mkv;*.mpeg)|*.mp4;*.MP4;*.m2ts;*.M2TS;*.MTS;*.mkv;*.MKV;*.mpeg;*.MPEG;*.mpg;*.MPG|Portable Network Graphics (*.png)|*.png;*.PNG|Joint Photographic Experts Group (*.jpg)|*.jpg;*.JPG;*.jpeg;*.JPEG|Microsoft Windows Bitmap Image (*.bmp)|*.bmp;*.BMP|Graphics Interchange Format (*.gif)|*.gif;*.GIF";
			ofd.CheckFileExists = true;
			ofd.CheckPathExists = true;
			//ダイアログを表示する
			if(ofd.ShowDialog( ) == true) 
			{
				//OKボタンがクリックされたとき、選択されたファイル名を表示する
				TextBoxInput.Text = ofd.FileName;
				return;
			}
			return;
		}

		private void ButtonRun_Click(object sender, RoutedEventArgs e) 
		{
			ButtonRun_Clicked( );
			return;
		}

		private void ButtonRun_Clicked( )
		{
			long positionIndex = 0;
			if(TextBoxInput.Text == "")
			{
				// エラーメッセージ「画像ファイルを指定してください。」
				MessageBox.Show(Properties.Resources.specifyImage, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if(checkImage( ) || checkFile(TextBoxInput.Text))
			{
				// エラーメッセージ「認識できません。別のファイルを試してください。」
				MessageBox.Show(Properties.Resources.cannotImport, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			imageSource = TextBoxInput.Text;
			if(ComboBoxPosition.SelectedIndex >= 0 && ComboBoxPosition.SelectedIndex <= 4)
			{
				positionIndex = ComboBoxPosition.SelectedIndex + 1;
			}
			if(imageSource == "" || positionIndex < 1 || positionIndex > 5)
			{
				// エラーメッセージ「画像か位置が取得できません。もう1度試してください。」
				MessageBox.Show(Properties.Resources.tryAgain, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
			}
			Window2 Window_2 = new Window2( );
			Window_2.ImageSource = imageSource;
			Window_2.PositionIndex = positionIndex;
			Window_2.VideoMode = VideoMode;
			Window_2.Show( );
			this.Close( );
			return;
		}

		private bool checkImage( )
		{
			Regex imageExpression = new Regex(".(png|PNG|bmp|BMP|jpg|JPG)");
			Regex videoExpression = new Regex(".(gif|GIF|mp4|MP4|m2ts|M2TS|MTS|mkv|MKV|mpeg|MPEG|mpg|MPG)");
			if(videoExpression.IsMatch(TextBoxInput.Text))
			{
				VideoMode = true;
			}
			return !imageExpression.IsMatch(TextBoxInput.Text) && !videoExpression.IsMatch(TextBoxInput.Text);
		}

		private bool checkFile(string filePath)
		{
			if(!File.Exists(filePath))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private void endApp1(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
			return;
		}

		

	}
}
