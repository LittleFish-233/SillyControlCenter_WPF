using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SillyControlCenter_WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static daima.Peizhi Peizhi_ = new daima.Peizhi();
        public static daima.Mianban Mianban_ = new daima.Mianban();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var wi = System.Security.Principal.WindowsIdentity.GetCurrent();
            var wp = new System.Security.Principal.WindowsPrincipal(wi);

            bool runAsAdmin = wp.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);

            if (!runAsAdmin)
            {
                var processInfo = new ProcessStartInfo(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";
                try
                {
                    Process.Start(processInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("程序自动以管理员身份运行出错，请手动设置以管理员身份运行程序" + ex);
                    throw;
                }
                Environment.Exit(0);
            }

        }
    }
}
