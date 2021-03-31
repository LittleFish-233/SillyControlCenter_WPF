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

namespace SillyControlCenter_WPF.kongjian
{
    /// <summary>
    /// dainnao.xaml 的交互逻辑
    /// </summary>
    public partial class dainnao : UserControl
    {
        public daima.Shebei_PC Shebei_PC_ = null;
        System.Threading.Timer timer;
        public delegate void BoilerLogHandler(daima.Shebei_PC status);
        public event BoilerLogHandler BoilerEventLog;

        public SeriesCollection LastHourSeries { get; set; } = new SeriesCollection
            {
                new LineSeries
                {
                    AreaLimit = 0,
                    PointGeometrySize=0,
                    Title="Cpu利用率",
                    Values = new ChartValues<ObservableValue>()

                },
                new LineSeries
                {
                    AreaLimit = 0,
                    PointGeometrySize=0,
                    Title="Gpu利用率",
                    Values = new ChartValues<ObservableValue>()

                },
                new LineSeries
                {
                    AreaLimit = 0,
                    PointGeometrySize=0,
                    Title="Ram利用率",
                    Values = new ChartValues<ObservableValue>()

                }

        };

        public dainnao(daima.Shebei_PC shebei_PC)
        {
          
            InitializeComponent(); 
            Shebei_PC_ = shebei_PC;
            Loaded += Kongzhi_Loaded;
            DataContext = this;
        }
        public dainnao()
        {
            InitializeComponent();
        }
        private void OnBoilerEventLog(daima.Shebei_PC message)
        {
            if (BoilerEventLog != null)
            {
                BoilerEventLog(message);
            }
        }
        private void Kongzhi_Loaded(object sender, RoutedEventArgs e)
        {
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
                        textblock1.Text = Shebei_PC_.Shuju.Mingchen;
                        textblock2.Text = "CPU利用率：" + (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Cpu_use.ToString("0.0") + "%";
                        textblock3.Text = "GPU占用率：" + (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Gpu_use.ToString("0.0") + "%";
                        textblock4.Text = "内存使用率：" + (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Ram_use.ToString("0.0") + "%";
                        textblock5.Text = "磁盘活动：" + (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Hdd_use.ToString("0.0") + "%";
                        textblock6.Text = "CPU温度：" + (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Cpu_wendu.ToString("0.0") + "%";
                        textblock7.Text = "GPU温度：" + (Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Gpu_wendu.ToString("0.0") + "%";
                        //填充图表
                        //添加数据到表格中
                        LastHourSeries[0].Values.Add(new ObservableValue((Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Cpu_use));
                        LastHourSeries[1].Values.Add(new ObservableValue((Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Gpu_use));
                        LastHourSeries[2].Values.Add(new ObservableValue((Shebei_PC_.Shuju as daima.Shuju_Shebei_PC).Ram_use));
                        //判断是否超过20
                        if (LastHourSeries[0].Values.Count >= 40)
                        {
                            LastHourSeries[0].Values.RemoveAt(0);
                        }
                        if (LastHourSeries[1].Values.Count >= 40)
                        {
                            LastHourSeries[1].Values.RemoveAt(0);
                        }
                        if (LastHourSeries[2].Values.Count >= 40)
                        {
                            LastHourSeries[2].Values.RemoveAt(0);
                        }
                    }
                    catch
                    {

                    }
                }
            }));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnBoilerEventLog(Shebei_PC_);
        }

    }
}
