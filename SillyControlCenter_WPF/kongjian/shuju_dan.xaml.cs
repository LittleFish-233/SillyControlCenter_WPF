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
    /// shuju_dan.xaml 的交互逻辑
    /// </summary>
    public partial class shuju_dan : UserControl
    {
        public SeriesCollection LastHourSeries { get; set; } = new SeriesCollection
            {
                new LineSeries
                {
                    AreaLimit = 0,
                    Values = new ChartValues<ObservableValue>()

                }
        };
        public shuju_dan(string biaoti, string biaoti_fu, string jianjie, string jianjie_fu, string shuju, string danwei, Brush color)
        {

            InitializeComponent();
            DataContext = this;
            textblock1.Text = biaoti;
            textblock2.Text = biaoti_fu;
            textblock3.Text = jianjie;
            textblock4.Text = jianjie_fu;
            textblock5.Text = shuju;
            textblock6.Text = danwei;
            beijing.Background = color;
            Loaded += Shuju_dan_Loaded;

        }

        private void Shuju_dan_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void Gengxin(string str)
        {
            textblock5.Text = str;
            //添加数据到表格中
            LastHourSeries[0].Values.Add(new ObservableValue(double.Parse(str)+10));
            //判断是否超过20
            if(LastHourSeries[0].Values.Count>=40)
            {
                LastHourSeries[0].Values.RemoveAt(0);
            }
        }
    }
}
