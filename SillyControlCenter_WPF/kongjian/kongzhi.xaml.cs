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
    /// kongzhi.xaml 的交互逻辑
    /// </summary>
    public partial class kongzhi : UserControl
    {
        public daima.Kongzhi kongzhi_ = null;
        System.Threading.Timer timer;
        public delegate void BoilerLogHandler(daima.Kongzhi status);
        public event BoilerLogHandler BoilerEventLog;
        private void OnBoilerEventLog(daima.Kongzhi message)
        {
            if (BoilerEventLog != null)
            {
                BoilerEventLog(message);
            }
        }

        public kongzhi(daima.Kongzhi kong_)
        {
            kongzhi_ = kong_;
            InitializeComponent();
            Loaded += Kongzhi_Loaded;
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
                        textblock0.Text = kongzhi_.Shuju.Zhujiming;
                        textblock1.Text = "连接设备总数：" + kongzhi_.Shebei_Fus.Count;                   
                    }
                    catch
                    {

                    }
                }
            }));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnBoilerEventLog(kongzhi_);
        }
    }
}
