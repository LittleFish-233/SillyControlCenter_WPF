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
    /// Caozong_Kongzhi.xaml 的交互逻辑
    /// </summary>
    public partial class Caozong_Kongzhi : Window
    {
        daima.Kongzhi kongzhi_ = null;
        public Caozong_Kongzhi(daima.Kongzhi kongzhi)
        {
            kongzhi_ = kongzhi;
            InitializeComponent();
            Loaded += Caozong_Kongzhi_Loaded;
        }

        private void Caozong_Kongzhi_Loaded(object sender, RoutedEventArgs e)
        {
            textbox2.Text = kongzhi_.Shuju.Zhujiming;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            kongzhi_.Xiugai_mingzi(textbox2.Text);
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
    }
}
