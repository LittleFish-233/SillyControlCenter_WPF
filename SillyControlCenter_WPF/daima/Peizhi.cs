using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SillyControlCenter_WPF.daima
{
    public class Peizhi
    {
        public delegate void BoilerLogHandler(string status);
        public event BoilerLogHandler Beijing_change;
        public event BoilerLogHandler Touxiang_change;

        /// <summary>
        /// 版本
        /// </summary>
        public static int banben = 1;

        /// <summary>
        /// 数据
        /// </summary>
        public Shuju_peizhi Shuju { get; set; } = new Shuju_peizhi();

        private static bool shifou_zhengzai = false;

        /// <summary>
        /// 当前类文件的根目录
        /// </summary>
        private string lujing { get { return Environment.CurrentDirectory + "\\SillyFile\\"; } }

        /// <summary>
        /// 保存基本信息
        /// </summary>
        public void Baocun_jiben()
        {
            if (shifou_zhengzai == true)
            {
                return;
            }
            shifou_zhengzai = true;
            //新建文件夹
            Directory.CreateDirectory(lujing);

            //序列化数据
            //添加基本数据
            using (StreamWriter file = File.CreateText(lujing+"peizhi.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Shuju);
            }
            shifou_zhengzai = false;
        }
        /// <summary>
        /// 读取基本信息
        /// </summary>
        /// <returns>是否成功</returns>
        public void Duqu_jiben()
        {
            try
            {
                using (StreamReader file = File.OpenText(lujing + "peizhi.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Shuju = (Shuju_peizhi)serializer.Deserialize(file, typeof(Shuju_peizhi));
                }
            }
            catch
            {
                Baocun_jiben();
            }
        }
        /// <summary>
        /// 添加服务器IP
        /// </summary>
        /// <param name="fuwuqi_Ip"></param>
        public void Add_ip(Fuwuqi_ip fuwuqi_Ip)
        {
            foreach(var item in Shuju.Fuwuqi_Ips)
            {
                if(item.Ip==fuwuqi_Ip.Ip&&item.Daunkou==fuwuqi_Ip.Daunkou)
                {
                    return;
                }
            }

            Shuju.Fuwuqi_Ips.Add(fuwuqi_Ip);
            Baocun_jiben();
        }
        /// <summary>
        /// 修改背景图片
        /// </summary>
        /// <param name="file"></param>
        public void Xiugai_beijing(string file)
        {
            //复制文件
           // string linshi = lujing + "beijing." + Gongju.huoqu_kuozhang(file);
           // File.Copy(file, linshi,true);
            //保存路径
            Shuju.Beijing = file;
            Baocun_jiben();
            OnBeijing_change(file);
        }
        /// <summary>
        /// 修改头像图片
        /// </summary>
        /// <param name="file"></param>
        public void Xiugai_touxiang(string file)
        {
            //复制文件
            //string linshi = lujing + "touxiang." + Gongju.huoqu_kuozhang(file);
            //File.Copy(file, linshi, true);
            //保存路径
            Shuju.Touxiang = file;
            Baocun_jiben();
            OnTouxiang_change(file);
        }
        private void OnBeijing_change(string message)
        {
            if (Beijing_change != null)
            {
                Beijing_change(message);
            }
        }
        private void OnTouxiang_change(string message)
        {
            if (Touxiang_change != null)
            {
                Touxiang_change(message);
            }
        }

        public void Remove_ip(Fuwuqi_ip fuwuqi_Ip)
        {
            try
            {
                foreach(var item in Shuju.Fuwuqi_Ips)
                {
                    if (item.Ip == fuwuqi_Ip.Ip && item.Daunkou == fuwuqi_Ip.Daunkou)
                    {
                        Shuju.Fuwuqi_Ips.Remove(item);
                        break;
                    }
                }
                
                Baocun_jiben();
            }
            catch(Exception exc)
            {

            }
        }
    }

    /// <summary>
    /// 配置信息
    /// </summary>
    public class Shuju_peizhi
    {
        private string zhujiming = "";
        /// <summary>
        /// 主机名称
        /// </summary>
        public string Zhujiming
        {
            get
            {
                if (string.IsNullOrEmpty(zhujiming) == true)
                {
                    zhujiming = System.Environment.GetEnvironmentVariable("ComputerName");
                }
                return zhujiming;
            }
            set
            {
                zhujiming = value;
            }
        }
        private string weiyi_shibie = "";
        /// <summary>
        /// 唯一识别号 16位数字
        /// </summary>
        ///        

        public string Weiyi_shibie
        {
            get
            {
                if (string.IsNullOrEmpty(weiyi_shibie) == true)
                {
                    Random rd = new Random();

                    weiyi_shibie = rd.Next(10000000, 99999999).ToString() + rd.Next(10000000, 99999999).ToString();
                }
                return weiyi_shibie;
            }
            set
            {
                weiyi_shibie = value;
            }
        }
        /// <summary>
        /// 控制中心列表
        /// </summary>
        public List<Fuwuqi_ip> Fuwuqi_Ips { get; set; } = new List<Fuwuqi_ip>();
        /// <summary>
        /// 背景图片
        /// </summary>
        public string Beijing { get; set; }
        /// <summary>
        /// 主机头像
        /// </summary>
        public string Touxiang { get; set; }

    }

    public class Fuwuqi_ip
    {
        /// <summary>
        /// ip
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Daunkou { get; set; }
    }
}
