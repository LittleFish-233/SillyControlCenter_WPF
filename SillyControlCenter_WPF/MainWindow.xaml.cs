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

namespace SillyControlCenter_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Threading.Timer timer;
        public MainWindow()
        {
            //读取配置
            App.Peizhi_.Duqu_jiben();
            App.Mianban_.Chushihua(App.Peizhi_.Shuju.Weiyi_shibie);
            //启动定时器
            timer = new System.Threading.Timer(new System.Threading.TimerCallback(Gengxin), null,0, 700);//1S定时器
            //启动对控制器监听
            Task task = new Task(() =>
            {
                App.Mianban_.Udp_jianting();
            });
            task.Start();

            InitializeComponent();
            Loaded += MainWindow_Loaded;
            App.Peizhi_.Beijing_change += Peizhi__Beijing_change;
        }

        private void Peizhi__Beijing_change(string status)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                //修改前台背景
                if (string.IsNullOrEmpty(App.Peizhi_.Shuju.Beijing) == false)
                {
                    beijing.Source = new BitmapImage(new Uri(App.Peizhi_.Shuju.Beijing));
                }
            }));
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            frame_main.Navigate(new zhuye());
            //修改前台头像
            if (string.IsNullOrEmpty(App.Peizhi_.Shuju.Beijing) == false)
            {
                beijing.Source = new BitmapImage(new Uri(App.Peizhi_.Shuju.Beijing));
            }

            //App.Peizhi_.Shuju.Zhujiming = "asdjkkk";
            //App.Peizhi_.Shuju.Fuwuqi_Ips.Add(new daima.Fuwuqi_ip { Daunkou = "123", Ip = "192.168.1.2" });
            //App.Peizhi_.Shuju.Fuwuqi_Ips.Add(new daima.Fuwuqi_ip { Daunkou = "1283", Ip = "192.168.1.32" });
            //App.Peizhi_.Baocun_jiben();
        }

        public void Gengxin(object a)
        {
            lock (this)
            {
                // your code here
                //刷新本地数据
                App.Mianban_.Gengxin_bendi();
                //向控制中心发送数据
                App.Mianban_.Send_shuju(App.Peizhi_.Shuju.Fuwuqi_Ips);

            }
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
            Application.Current.Shutdown();
        }

        private void btn_she_Click(object sender, RoutedEventArgs e)
        {
            shezhi shezhi_ = new shezhi();
            shezhi_.Show();
        }

        private void btn_xiao_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; //设置窗口最小化
        }

        private void btn_fan_Click(object sender, RoutedEventArgs e)
        {
            if(frame_main.CanGoBack)
            {
                frame_main.GoBack();
            }
        }

        private void frame_main_Navigated(object sender, NavigationEventArgs e)
        {
            if(frame_main.CanGoBack)
            {
                btn_sh.Visibility = Visibility.Visible;
            }
            else
            {
                btn_sh.Visibility = Visibility.Collapsed;
            }
        }
    }
}
