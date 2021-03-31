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
    /// shebei.xaml 的交互逻辑
    /// </summary>
    public partial class shebei : Page
    {
        kongjian.shuju_dan cpu_use = null;
        kongjian.shuju_dan cpu_wendu = null;
        kongjian.shuju_dan gpu_use = null;
        kongjian.shuju_dan gpu_wendu = null;
        kongjian.shuju_dan ram_use = null;
        public daima.Shebei_PC Shebei_PC_ = null;
        System.Threading.Timer timer;

        public shebei(daima.Shebei_PC shebei_PC)
        {
            Shebei_PC_ = shebei_PC;
            Loaded += Kongzhi_Loaded;
            InitializeComponent();
        }
        private void Kongzhi_Loaded(object sender, RoutedEventArgs e)
        {
            //添加卡片到UI界面
            cpu_use = new kongjian.shuju_dan("CPU使用率", "4核8线程", "20秒内的CPU使用情况", "每秒刷新", (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Cpu_use.ToString("0.0"), "%",new SolidColorBrush(Color.FromRgb(208,54,81)));
            cpu_wendu = new kongjian.shuju_dan("CPU温度", (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Cpu_info, "20秒内的CPU温度情况", "每秒刷新", (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Cpu_wendu.ToString("0.0"), "℃", new SolidColorBrush(Color.FromRgb(254, 113, 53)));
            gpu_use = new kongjian.shuju_dan("GPU使用率", "", "20秒内的GPU使用情况", "每秒刷新", (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Gpu_use.ToString("0.0"), "%", new SolidColorBrush(Color.FromRgb(251, 195, 74)));
            gpu_wendu = new kongjian.shuju_dan("CPU温度", (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Gpu_info, "20秒内的GPU温度情况", "每秒刷新", (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Gpu_wendu.ToString("0.0"), "℃", new SolidColorBrush(Color.FromRgb(112, 171, 84)));
            ram_use = new kongjian.shuju_dan("RAM使用率", (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Ram_info, "20秒内的RAM使用情况", "每秒刷新", (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Cpu_use.ToString("0.0"), "%", new SolidColorBrush(Color.FromRgb(35, 149, 213)));
            warp.Children.Add(cpu_use);
            warp.Children.Add(cpu_wendu);
            warp.Children.Add(gpu_use);
            warp.Children.Add(gpu_wendu);
            warp.Children.Add(ram_use);
            //启动定时器
            timer = new System.Threading.Timer(new System.Threading.TimerCallback(Gengxin), null, 0, 1000);//1S定时器
        }
        public void Gengxin(object a)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                lock (this)
                {
                    try
                    {
                        //刷新前台数据
                        textblock0.Text = Shebei_PC_.Shuju.Mingchen;
                        textblock1.Text = "处理器：" + (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Cpu_info;
                        textblock2.Text = "显卡：" + (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Gpu_info;
                        textblock3.Text = "内存：" + (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Ram_info;
                        textblock4.Text = "系统：" + (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Sys_info;
                        textblock5.Text = "磁盘活动时间比：" + (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Hdd_use.ToString("0.0") + "%";
                        textblock6.Text = "系统连续开机时间：" + (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Sys_time;

                        cpu_use.Gengxin((Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Cpu_use.ToString("0.0"));
                        cpu_wendu.Gengxin((Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Cpu_wendu.ToString("0.0"));
                        gpu_use.Gengxin((Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Gpu_use.ToString("0.0"));
                        gpu_wendu.Gengxin((Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Gpu_wendu.ToString("0.0"));
                        ram_use.Gengxin((Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Ram_use.ToString("0.0"));
                    }
                    catch
                    {

                    }
                }
            }));
        }
    }
}
