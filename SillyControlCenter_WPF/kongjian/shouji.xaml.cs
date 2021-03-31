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
    /// shouji.xaml 的交互逻辑
    /// </summary>
    public partial class shouji : UserControl
    {
        public daima.Shebei_AD Shebei_AD_ = null;
        System.Threading.Timer timer;
        public delegate void BoilerLogHandler(daima.Shebei_AD status);
        public event BoilerLogHandler BoilerEventLog;

        public SeriesCollection LastHourSeries { get; set; } = new SeriesCollection
            {
                new LineSeries
                {
                    AreaLimit = 0,
                    PointGeometrySize=0,
                    Title="电量",
                    Values = new ChartValues<ObservableValue>()

                },
                new LineSeries
                {
                    AreaLimit = 0,
                    PointGeometrySize=0,
                    Title="电池温度",
                    Values = new ChartValues<ObservableValue>()

                },
                new LineSeries
                {
                    AreaLimit = 0,
                    PointGeometrySize=0,
                    Title="储存占用率",
                    Values = new ChartValues<ObservableValue>()

                }

        };

        public shouji(daima.Shebei_AD shebei_AD)
        {
            InitializeComponent();
            Shebei_AD_ = shebei_AD;
            Loaded += Kongzhi_Loaded;
            DataContext = this;
        }

        private void Kongzhi_Loaded(object sender, RoutedEventArgs e)
        {
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
                        textblock1.Text = Shebei_AD_.Shuju.Mingchen;
                        textblock2.Text = "电量：" +( (Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Dianliang*100).ToString("0.") + "%";
                        textblock3.Text = "电池温度：" + (Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Dianchi_Wendu.ToString("0.0") + "%";
                        textblock4.Text = "内存占用率：" + (Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Ram_use.ToString("0.0") + "%";
                        textblock5.Text = "储存占用率：" + ((Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Hdd_use*100).ToString("0.0") + "%";

                        //填充图表
                        //添加数据到表格中
                        LastHourSeries[0].Values.Add(new ObservableValue((Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Dianliang));
                        LastHourSeries[1].Values.Add(new ObservableValue((Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Dianchi_Wendu));
                        LastHourSeries[2].Values.Add(new ObservableValue((Shebei_AD_.Shuju as daima.Shuju_Shebei_AD).Ram_use));
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
            OnBoilerEventLog(Shebei_AD_);
        }
        private void OnBoilerEventLog(daima.Shebei_AD message)
        {
            if (BoilerEventLog != null)
            {
                BoilerEventLog(message);
            }
        }

    }
}
