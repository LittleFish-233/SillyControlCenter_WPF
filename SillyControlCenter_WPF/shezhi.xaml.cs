using Microsoft.Win32;
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

namespace SillyControlCenter_WPF
{
    /// <summary>
    /// shezhi.xaml 的交互逻辑
    /// </summary>
    public partial class shezhi : Window
    {
        public shezhi()
        {
            InitializeComponent();
            Loaded += Shezhi_Loaded;
        }

        private void Shezhi_Loaded(object sender, RoutedEventArgs e)
        {
            textbox1.Text = App.Peizhi_.Shuju.Zhujiming;
            textbox2.Text = App.Peizhi_.Shuju.Beijing;
            textbox3.Text = App.Peizhi_.Shuju.Touxiang;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //保存数据
                App.Peizhi_.Shuju.Zhujiming = textbox1.Text;
                App.Peizhi_.Baocun_jiben();

                App.Peizhi_.Xiugai_beijing(textbox2.Text);
                App.Peizhi_.Xiugai_touxiang(textbox3.Text);

            }
            catch(Exception exc)
            {

            }     
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {


            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        int i = 0;
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            i += 1;
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };
            timer.IsEnabled = true;

            if (i % 2 == 0)
            {
                timer.IsEnabled = false;
                i = 0;
                this.WindowState = this.WindowState == WindowState.Maximized ?
                              WindowState.Normal : WindowState.Maximized;
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_she_Click(object sender, RoutedEventArgs e)
        {
            shezhi shezhi_ = new shezhi();
            shezhi_.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //打开文件
            var dialog = new OpenFileDialog();
            dialog.Filter = "图像文件(*.bmp, *.jpg,*png,*jpeg)|*.bmp;*.jpg;*png;*jpeg|所有文件(*.*)|*.*";
            if (dialog.ShowDialog(this) == false) return;
            string _fileName = dialog.FileName;
            //初始化图片
            BitmapImage tempImage = new BitmapImage();
            tempImage.BeginInit();
            tempImage.UriSource = new Uri(_fileName, UriKind.RelativeOrAbsolute);
            tempImage.EndInit();

            //imageCavas为窗口中设置的Image控件
            // imageCavas.Source = tempImage;  
            textbox2.Text = _fileName;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //打开文件
            var dialog = new OpenFileDialog();
            dialog.Filter = "图像文件(*.bmp, *.jpg,*png,*jpeg)|*.bmp;*.jpg;*png;*jpeg|所有文件(*.*)|*.*";
            if (dialog.ShowDialog(this) == false) return;
            string _fileName = dialog.FileName;
            //初始化图片
            BitmapImage tempImage = new BitmapImage();
            tempImage.BeginInit();
            tempImage.UriSource = new Uri(_fileName, UriKind.RelativeOrAbsolute);
            tempImage.EndInit();

            //imageCavas为窗口中设置的Image控件
            // imageCavas.Source = tempImage;  
            textbox3.Text = _fileName;
        }
    }
}
