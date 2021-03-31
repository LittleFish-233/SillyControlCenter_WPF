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
using Panuon.UI.Silver;
using System.Windows.Shapes;

namespace SillyControlCenter_WPF
{
    /// <summary>
    /// lianjie_kongzhi.xaml 的交互逻辑
    /// </summary>
    public partial class lianjie_kongzhi : Window
    {
        public lianjie_kongzhi()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Mianban_.Add_kongzhi(new daima.Fuwuqi_ip { Daunkou = int.Parse(textbox2.Text), Ip = textbox1.Text });
                App.Peizhi_.Add_ip(new daima.Fuwuqi_ip { Daunkou = int.Parse(textbox2.Text), Ip = textbox1.Text });
            }
            catch (Exception exc)
            {
                //NoticeX.Show("This is a message.", "Error", MessageBoxIcon.Error, 3000);
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
    }
}
