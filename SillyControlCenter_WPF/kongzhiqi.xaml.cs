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
    /// kongzhiqi.xaml 的交互逻辑
    /// </summary>
    public partial class kongzhiqi : Page
    {
        public daima.Kongzhi kongzhi_ = null;
        System.Threading.Timer timer;

        public kongzhiqi(daima.Kongzhi kong_)
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


                        //循环界面列表 查找不在的控制器将其移除
                        foreach (var item in warp.Children)
                        {
                            string weiyi = null;
                            bool shifou = false;

                            //判断是否是控件
                            if (item is kongjian.kongzhi)
                            {
                                kongjian.kongzhi kongzhi_ui = item as kongjian.kongzhi;
                                weiyi = kongzhi_ui.kongzhi_.Shuju.Weiyi_shibie;
                            }
                            else
                            {
                                if (item is kongjian.dainnao)
                                {
                                    kongjian.dainnao kongzhi_ui = item as kongjian.dainnao;
                                    weiyi = kongzhi_ui.Shebei_PC_.Shuju.Weiyi_shibie;
                                }
                                else
                                {
                                    if (item is kongjian.shouji)
                                    {
                                        kongjian.shouji kongzhi_ui = item as kongjian.shouji;
                                        weiyi = kongzhi_ui.Shebei_AD_.Shuju.Weiyi_shibie;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                
                            }

                            foreach (var nei in kongzhi_.Shebei_Fus)
                            {
                                //找到则直接下一个
                                if (weiyi == nei.Shuju.Weiyi_shibie)
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
                        foreach (var nei in kongzhi_.Shebei_Fus)
                        {
                            bool shifou = false;
                            foreach (var item in warp.Children)
                            {
                                string weiyi = null;
                                //判断是否是控件
                                if (item is kongjian.kongzhi)
                                {
                                    kongjian.kongzhi kongzhi_ui = item as kongjian.kongzhi;
                                    weiyi = kongzhi_ui.kongzhi_.Shuju.Weiyi_shibie;
                                }
                                else
                                {
                                    if (item is kongjian.dainnao)
                                    {
                                        kongjian.dainnao kongzhi_ui = item as kongjian.dainnao;
                                        weiyi = kongzhi_ui.Shebei_PC_.Shuju.Weiyi_shibie;
                                    }
                                    else
                                    {
                                        if (item is kongjian.shouji)
                                        {
                                            kongjian.shouji kongzhi_ui = item as kongjian.shouji;
                                            weiyi = kongzhi_ui.Shebei_AD_.Shuju.Weiyi_shibie;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                                //找到则直接下一个
                                if (weiyi == nei.Shuju.Weiyi_shibie)
                                {
                                    shifou = true;
                                    break;
                                }
                            }
                            if (shifou == false)
                            {
                                //未找到匹配的
                                switch (nei.Shuju.Shebei_shibie)
                                {
                                    case 0:
                                        kongjian.dainnao linshi = new kongjian.dainnao(nei as daima.Shebei_PC);
                                        linshi.BoilerEventLog += Linshi_BoilerEventLog;
                                        warp.Children.Insert(0, linshi);
                                        break;
                                    case 1:
                                        kongjian.shouji linshi_ = new kongjian.shouji(nei as daima.Shebei_AD);
                                        linshi_.BoilerEventLog += Linshi_BoilerEventLog_;
                                        warp.Children.Insert(0, linshi_);
                                        break;
                                }
                                

                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }));
        }

        private void Linshi_BoilerEventLog_(daima.Shebei_AD status)
        {
            NavigationService.Navigate(new shouji(status));
        }

        private void Linshi_BoilerEventLog(daima.Shebei_PC status)
        {
            NavigationService.Navigate(new shebei(status));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Caozong_Kongzhi caozong_Kongzhi = new Caozong_Kongzhi(kongzhi_);
            caozong_Kongzhi.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            App.Mianban_.Remove_fu(kongzhi_);
            NavigationService.GoBack();
        }
    }
}
