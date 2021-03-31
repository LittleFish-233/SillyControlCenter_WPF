using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
    /// zhuye.xaml 的交互逻辑
    /// </summary>
    public partial class zhuye : Page
    {
        System.Threading.Timer timer;
        public SeriesCollection LastHourSeries { get; set; } = new SeriesCollection
            {
                new LineSeries
                {
                    AreaLimit = 0,
                    PointGeometrySize=0,
                    Title="Cpu温度",
                    Values = new ChartValues<ObservableValue>()

                },
                new LineSeries
                {
                    AreaLimit = 0,
                    PointGeometrySize=0,
                    Title="Gpu温度",
                    Values = new ChartValues<ObservableValue>()

                }

        };

        public zhuye()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += Zhuye_Loaded;
            App.Peizhi_.Touxiang_change += Peizhi__Touxiang_change;
        }

        private void Peizhi__Touxiang_change(string status)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                //修改前台头像
                if (string.IsNullOrEmpty(App.Peizhi_.Shuju.Touxiang) == false)
                {
                    touxiang.Source = new BitmapImage(new Uri(App.Peizhi_.Shuju.Touxiang));
                }
            }));
        }

        private void Zhuye_Loaded(object sender, RoutedEventArgs e)
        {
            //启动定时器
            timer = new System.Threading.Timer(new System.Threading.TimerCallback(Gengxin), null, 0, 1000);//1S定时器 
            //修改前台头像
            if (string.IsNullOrEmpty(App.Peizhi_.Shuju.Touxiang) == false)
            {
                try
                {
                    touxiang.Source = new BitmapImage(new Uri(App.Peizhi_.Shuju.Touxiang));

                }
                catch
                {

                }
            }
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
                        daima.Shuju_Shebei_PC shuju_Shebei_PC = App.Mianban_.Shebei_dangqian.Shuju as daima.Shuju_Shebei_PC;
                        textblock0.Text = shuju_Shebei_PC.Mingchen;
                        textblock1.Text = "处理器：" + shuju_Shebei_PC.Cpu_info;
                        textblock2.Text = "显卡：" + shuju_Shebei_PC.Gpu_info;
                        textblock3.Text = "内存：" + shuju_Shebei_PC.Ram_info;
                        textblock4.Text = "系统：" + shuju_Shebei_PC.Sys_info;
                        textblock5.Text = "CPU利用率：" + shuju_Shebei_PC.Cpu_use.ToString("0.0") + "%";
                        textblock6.Text = "GPU占用率：" + shuju_Shebei_PC.Gpu_use.ToString("0.0") + "%";
                        textblock7.Text = "内存使用率：" + shuju_Shebei_PC.Ram_use.ToString("0.0") + "%";
                        textblock8.Text = "磁盘活动时间比：" + shuju_Shebei_PC.Hdd_use.ToString("0.0") + "%";
                        textblock9.Text = "系统连续开机时间：" + shuju_Shebei_PC.Sys_time;

                       

                        //填充图表
                        //添加数据到表格中
                        LastHourSeries[0].Values.Add(new ObservableValue(shuju_Shebei_PC.Cpu_wendu));
                        LastHourSeries[1].Values.Add(new ObservableValue(shuju_Shebei_PC.Gpu_wendu));
                        //判断是否超过20
                        if (LastHourSeries[0].Values.Count >= 40)
                        {
                            LastHourSeries[0].Values.RemoveAt(0);
                        }
                        if (LastHourSeries[1].Values.Count >= 40)
                        {
                            LastHourSeries[1].Values.RemoveAt(0);
                        }

                        //循环界面列表 查找不在的控制器将其移除
                        foreach (var item in warp.Children)
                        {
                            bool shifou = false;
                            kongjian.kongzhi kongzhi_ui = null;
                            //判断是否是控件
                            if (item is kongjian.kongzhi)
                            {
                                kongzhi_ui = item as kongjian.kongzhi;
                            }
                            else
                            {
                                continue;
                            }
                            
                            foreach (var nei in App.Mianban_.Kongzhis)
                            {
                                //找到则直接下一个
                                if (kongzhi_ui.kongzhi_.Shuju.Weiyi_shibie == nei.Shuju.Weiyi_shibie)
                                {
                                    shifou = true;
                                    break;
                                }
                            }
                            //未找到匹配的
                            if (shifou == false)
                            {
                                warp.Children.Remove((UIElement)item);
                            }
                        }
                        //循环后台控制器列表 查找不在ui的将其添加
                        foreach (var nei in App.Mianban_.Kongzhis)
                        {
                            bool shifou = false;
                            foreach (var item in warp.Children)
                            {
                                kongjian.kongzhi kongzhi_ui = null;
                                //判断是否是控件
                                if (item is kongjian.kongzhi)
                                {
                                    kongzhi_ui = item as kongjian.kongzhi;
                                }
                                else
                                {
                                    continue;
                                }
                                //找到则直接下一个
                                if (kongzhi_ui.kongzhi_.Shuju.Weiyi_shibie == nei.Shuju.Weiyi_shibie)
                                {
                                    shifou = true;
                                }
                            }
                            if (shifou == false)
                            {
                                //未找到匹配的
                                kongjian.kongzhi linshi = new kongjian.kongzhi(nei);
                                linshi.BoilerEventLog += Linshi_BoilerEventLog;
                                warp.Children.Insert(0,linshi);

                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }));
        }

        private void Linshi_BoilerEventLog(daima.Kongzhi status)
        {
            NavigationService.Navigate(new kongzhiqi(status));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lianjie_kongzhi lianjie_ = new lianjie_kongzhi();
            lianjie_.Show();
        }

    }
}
