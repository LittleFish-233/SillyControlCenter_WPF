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
    /// shouji.xaml 的交互逻辑
    /// </summary>
    public partial class shouji : Page
    {
        kongjian.shuju_dan dianliang = null;
        kongjian.shuju_dan dianchi_wendu = null;
        kongjian.shuju_dan ram_use = null;
        kongjian.shuju_dan hdd_use = null;

        public daima.Shebei_AD Shebei_AD_ = null;
        System.Threading.Timer timer;

        public shouji(daima.Shebei_AD shebei_AD)
        {
            Shebei_AD_ = shebei_AD;
            Loaded += Kongzhi_Loaded;
            InitializeComponent();
        }
        private void Kongzhi_Loaded(object sender, RoutedEventArgs e)
        {
            //添加卡片到UI界面
            dianliang = new kongjian.shuju_dan("电量", "", "20秒内的电池电量变化", "每秒刷新", ((Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Dianliang*100).ToString("0."), "%", new SolidColorBrush(Color.FromRgb(208, 54, 81)));
            dianchi_wendu = new kongjian.shuju_dan("电池温度", "", "20秒内的电池温度情况", "每秒刷新", (Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Dianchi_Wendu.ToString("0.0"), "℃", new SolidColorBrush(Color.FromRgb(254, 113, 53)));
            ram_use = new kongjian.shuju_dan("内存占用率", "", "20秒内的内存占用情况", "每秒刷新", (Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Ram_use.ToString("0.0"), "%", new SolidColorBrush(Color.FromRgb(251, 195, 74)));
            hdd_use = new kongjian.shuju_dan("储存占用率", "", "20秒内的储存情况", "每秒刷新", ((Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Hdd_use*100).ToString("0.0"), "%", new SolidColorBrush(Color.FromRgb(112, 171, 84)));
            warp.Children.Add(dianliang);
            warp.Children.Add(dianchi_wendu);
            warp.Children.Add(ram_use);
            warp.Children.Add(hdd_use);
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
                        textblock0.Text = Shebei_AD_.Shuju.Mingchen;
                        textblock1.Text = "电量：" + ((Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Dianliang * 100).ToString("0.") + "%";
                        textblock2.Text = "电池温度：" + (Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Dianchi_Wendu.ToString("0.0") + "℃";
                        textblock3.Text = "内存占用率：" + (Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Ram_use.ToString("0.0") + "%";
                        textblock4.Text = "储存占用率：" + ((Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Hdd_use*100).ToString("0.0") + "%";

                        dianliang.Gengxin(((Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Dianliang*100).ToString("0."));
                        dianchi_wendu.Gengxin((Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Dianchi_Wendu.ToString("0.0"));
                        ram_use.Gengxin((Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Ram_use.ToString("0.0"));
                        hdd_use.Gengxin(((Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Hdd_use*100).ToString("0.0"));
                    }
                    catch
                    {

                    }
                }
            }));
        }
    }
}
