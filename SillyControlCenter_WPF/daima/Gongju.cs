using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SillyControlCenter_WPF.daima
{
    public static class Gongju
    {
        /// <summary>
        /// 当前类文件的根目录
        /// </summary>
        private static string Lujing { get { return Environment.CurrentDirectory + "\\SillyFile\\"; } }

        private static Shuju_Shebei_PC shuju_Shebei_PC = null;
        private static UpdateVisitor  updateVisitor = new UpdateVisitor();
        private static Computer computer = null;

        //[DllImport("硬件检测引擎.dll")]
        // public static extern string Hwinfo(string 配置文件路径, string 运行目录);
        /// <summary>
        /// 获取硬件信息和占用
        /// </summary>
        /// <returns></returns>
        public static Shuju_Shebei_PC Huoqu_shebei_xinxi()
        {

            if (shuju_Shebei_PC == null)
            {
                shuju_Shebei_PC = new Shuju_Shebei_PC();
            }
            if(computer==null)
            {
                computer = new Computer();
                computer.Open();
            }

            shuju_Shebei_PC.Cpu_use = SystemInfo.CpuLoad;
            shuju_Shebei_PC.Ram_use = (float)(1-(double)SystemInfo.MemoryAvailable / SystemInfo.PhysicalMemory)*100;
           
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.RAMEnabled = true;

            computer.Accept(updateVisitor);

            float cpu_wendu = 0;
            int cpu_wendu_jishu = 0;

            float gpu_wendu = 0;
            int gpu_wendu_jishu = 0;

            float gpu_use = 0;
            int gpu_use_jishu = 0;


            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                //查找硬件类型为CPU
                if (computer.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        //找到温度传感器
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                        {
                            if (computer.Hardware[i].Sensors[j].Value != null)
                            {
                                cpu_wendu += (float)computer.Hardware[i].Sensors[j].Value;
                                cpu_wendu_jishu++;
                            }
                        }
                    }
                    shuju_Shebei_PC.Cpu_info = computer.Hardware[i].Name;
                }
                else//查找AMD的GPU
                    if (computer.Hardware[i].HardwareType == HardwareType.GpuAti)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        //找到温度传感器
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                        {
                            if (computer.Hardware[i].Sensors[j].Value != null)
                            {
                                gpu_wendu += (float)computer.Hardware[i].Sensors[j].Value;
                                gpu_wendu_jishu++;
                            }
                        }
                        else//找到占用率
                            if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                        {
                            if (computer.Hardware[i].Sensors[j].Value != null)
                            {
                                gpu_use += (float)computer.Hardware[i].Sensors[j].Value;
                                gpu_use_jishu++;
                            }
                        }
                    }
                    shuju_Shebei_PC.Gpu_info = computer.Hardware[i].Name;
                }
                else//查找AMD的GPU
                    if (computer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        //找到温度传感器
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                        {
                            if (computer.Hardware[i].Sensors[j].Value != null)
                            {
                                gpu_wendu += (float)computer.Hardware[i].Sensors[j].Value;
                                gpu_wendu_jishu++;
                            }
                        }
                        else//找到占用率
                            if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                        {
                            if (computer.Hardware[i].Sensors[j].Value != null)
                            {
                                gpu_use += (float)computer.Hardware[i].Sensors[j].Value;
                                gpu_use_jishu++;
                            }
                        }
                        shuju_Shebei_PC.Gpu_info = computer.Hardware[i].Name;
                    }
                }
                else//查找AMD的GPU
                    if (computer.Hardware[i].HardwareType == HardwareType.RAM)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        shuju_Shebei_PC.Ram_info = computer.Hardware[i].Name;
                    }
                }
            }

            //格式化字符串
            if (gpu_use_jishu != 0)
            {
                shuju_Shebei_PC.Gpu_use = (gpu_use / gpu_use_jishu);
            }
            if (cpu_wendu_jishu != 0)
            {
                shuju_Shebei_PC.Cpu_wendu = (cpu_wendu / cpu_wendu_jishu);
            }
            if (gpu_wendu_jishu != 0)
            {
                shuju_Shebei_PC.Gpu_wendu = (gpu_wendu / gpu_wendu_jishu);
            }
            shuju_Shebei_PC.Ram_info += " "+((double)SystemInfo.PhysicalMemory / (1024 * 1024*1024)).ToString(".0")+" GB";

            shuju_Shebei_PC.Sys_info = GetOSFriendlyName();
            shuju_Shebei_PC.Sys_time = ((Environment.TickCount / 0x3e8) / 60).ToString() + "分钟";
            return shuju_Shebei_PC;
        }
        public static string GetOSFriendlyName()
        {
            string result = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }
            return result;
        }
        /// <summary>
        /// 获取后缀名
        /// </summary>
        /// <param name="wenjianming">文件名</param>
        /// <returns></returns>
        public static string huoqu_kuozhang(string wenjianming)
        {
            string jieguo = "";
            for (int i = wenjianming.Length - 1; i >= 0; i--)
            {
                if (wenjianming[i] == '.')
                {
                    break;
                }
                jieguo += wenjianming[i];
            }
            string linshi = "";
            for (int i = jieguo.Length - 1; i >= 0; i--)
            {
                linshi += jieguo[i];
            }
            return linshi;
        }
    }
}
