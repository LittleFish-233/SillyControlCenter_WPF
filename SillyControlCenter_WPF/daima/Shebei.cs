using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SillyControlCenter_WPF.daima
{
    public abstract class Shebei_Fu
    {
        /// <summary>
        /// 数据
        /// </summary>
        public Shuju_Shebei Shuju { get; set; } = new Shuju_Shebei();

        /// <summary>
        /// 当前类文件的根目录
        /// </summary>
        protected string Lujing { get { return Environment.CurrentDirectory + "\\SillyFile\\" + Shuju.Weiyi_shibie + "\\"; } }

        protected bool shifou_zhengzai = false;

        /// <summary>
        /// 初始化设备
        /// </summary>
        /// <param name="weiyi_shibie">唯一识别号</param>
        /// <param name="shuju_xin">新数据</param>
        public abstract void Chushihua(string weiyi_shibie);

        /// <summary>
        /// 更新设备数据
        /// </summary>
        /// <param name="shuju_xin"></param>
        public abstract void Gengxin(string shuju_xin);
        /// <summary>
        /// 保存
        /// </summary>
        public abstract void Baocun();
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns>是否成功</returns>
        public abstract void Duqu();

    }
    public class Shebei_PC : Shebei_Fu
    {

        /// <summary>
        /// 初始化设备 
        /// </summary>
        /// <param name="weiyi_shibie">唯一识别号</param>
        /// <param name="shuju_xin">新数据</param>
        public override void Chushihua(string weiyi_shibie)
        {
            Shuju.Weiyi_shibie = weiyi_shibie;
            Duqu();
        }

        /// <summary>
        /// 更新设备数据
        /// </summary>
        /// <param name="shuju_xin"></param>
        public override void Gengxin(string shuju_xin)
        {
            try
            {
                using (TextReader str = new StringReader(shuju_xin))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Shuju = (Shuju_Shebei_PC)serializer.Deserialize(str, typeof(Shuju_Shebei_PC));
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 更新设备数据
        /// </summary>
        /// <param name="shuju_xin"></param>
        public void Gengxin(Shuju_Shebei_PC shuju_Shebei_PC)
        {

            Shuju = shuju_Shebei_PC;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void Baocun()
        {
            if (shifou_zhengzai == true)
            {
                return;
            }
            shifou_zhengzai = true;
            //新建文件夹
            Directory.CreateDirectory(Lujing);

            //序列化数据
            //添加基本数据
            using (StreamWriter file = File.CreateText(Lujing + "shuju.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Shuju);
            }
            shifou_zhengzai = false;
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns>是否成功</returns>
        public override void Duqu()
        {
            try
            {
                using (StreamReader file = File.OpenText(Lujing + "shuju.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Shuju = (Shuju_Shebei_PC)serializer.Deserialize(file, typeof(Shuju_Shebei_PC));
                }
            }
            catch
            {
                Baocun();
            }
        }

        public string Shuju_json()
        {
            string jieguo = "";
            using (TextWriter file = new StringWriter())
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Shuju);
                jieguo = file.ToString();
            }
            return jieguo;
        }

        public void Chushihua()
        {
            Duqu();
        }

    }

    public class Shuju_Shebei
    {
        /// <summary>
        /// 主机名称
        /// </summary>
        public string Mingchen { get; set; }
        /// <summary>
        /// 唯一识别号 16位数字
        /// </summary>
        ///        

        public string Weiyi_shibie { get; set; }
        /// <summary>
        /// 设备识别码 0 PC 1安卓 2 单片机 3命令
        /// </summary>
        public int Shebei_shibie { get; set; } = 0;
    }

    public class Shuju_Shebei_PC : Shuju_Shebei
    {

        /// <summary>
        /// CPU信息
        /// </summary>
        public string Cpu_info { get; set; }
        /// <summary>
        /// GPU信息
        /// </summary>
        public string Gpu_info { get; set; }
        /// <summary>
        /// RAM信息
        /// </summary>
        public string Ram_info { get; set; }
        /// <summary>
        /// 系统信息
        /// </summary>
        public string Sys_info { get; set; }
        /// <summary>
        /// CPU占用率
        /// </summary>
        public float Cpu_use { get; set; }
        /// <summary>
        /// GPU占用率
        /// </summary>
        public float Gpu_use { get; set; }
        /// <summary>
        /// RAM占用率
        /// </summary>
        public float Ram_use { get; set; }
        /// <summary>
        /// HDD占用率
        /// </summary>
        public float Hdd_use { get; set; }
        /// <summary>
        /// 系统运行时间
        /// </summary>
        public string Sys_time { get; set; }
        /// <summary>
        /// CPU温度
        /// </summary>
        public float Cpu_wendu { get; set; }
        /// <summary>
        /// GPU温度
        /// </summary>
        public float Gpu_wendu { get; set; }

    }

    public class Shuju_Shebei_AD : Shuju_Shebei
    {
        /// <summary>
        /// 电量
        /// </summary>

        public float Dianliang { get; set; } = 0;
        /// <summary>
        /// 电池温度
        /// </summary>

        public float Dianchi_Wendu { get; set; } = 0;
        /// <summary>
        /// 内存使用率
        /// </summary>

        public float Ram_use { get; set; } = 0;
        /// <summary>
        /// 储存使用率
        /// </summary>

        public float Hdd_use { get; set; } = 0;



    }
    public class Shebei_AD : Shebei_Fu
    {

        /// <summary>
        /// 初始化设备 
        /// </summary>
        /// <param name="weiyi_shibie">唯一识别号</param>
        /// <param name="shuju_xin">新数据</param>
        public override void Chushihua(string weiyi_shibie)
        {
            Shuju.Weiyi_shibie = weiyi_shibie;
            Duqu();
        }

        /// <summary>
        /// 更新设备数据
        /// </summary>
        /// <param name="shuju_xin"></param>
        public override void Gengxin(string shuju_xin)
        {
            try
            {
                using (TextReader str = new StringReader(shuju_xin))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Shuju = (Shuju_Shebei_AD)serializer.Deserialize(str, typeof(Shuju_Shebei_AD));
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 更新设备数据
        /// </summary>
        /// <param name="shuju_xin"></param>
        public void Gengxin(Shuju_Shebei_AD shuju_Shebei_AD)
        {

            Shuju = shuju_Shebei_AD;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void Baocun()
        {
            if (shifou_zhengzai == true)
            {
                return;
            }
            shifou_zhengzai = true;
            //新建文件夹
            Directory.CreateDirectory(Lujing);

            //序列化数据
            //添加基本数据
            using (StreamWriter file = File.CreateText(Lujing + "shuju.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Shuju);
            }
            shifou_zhengzai = false;
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns>是否成功</returns>
        public override void Duqu()
        {
            try
            {
                using (StreamReader file = File.OpenText(Lujing + "shuju.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Shuju = (Shuju_Shebei_AD)serializer.Deserialize(file, typeof(Shuju_Shebei_AD));
                }
            }
            catch
            {
                Baocun();
            }
        }

        public string Shuju_json()
        {
            string jieguo = "";
            using (TextWriter file = new StringWriter())
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Shuju);
                jieguo = file.ToString();
            }
            return jieguo;
        }

        public void Chushihua()
        {
            Duqu();
        }

    }
}
