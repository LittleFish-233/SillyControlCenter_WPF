using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SillyControlCenter_WPF.daima
{
    public class Mingling_Fu
    {
        /// <summary>
        /// 设备识别码 0 PC 1安卓 2 单片机 3命令
        /// </summary>
        public int Shebei_shibie { get; set; } = 3;
        /// <summary>
        /// 传递的命令
        /// </summary>
        public string Mingling { get; set; }
        /// <summary>
        /// 传递的参数
        /// </summary>
        public string Canshu { get; set; }

    }
}
