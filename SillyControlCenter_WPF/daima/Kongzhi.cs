using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SillyControlCenter_WPF.daima
{
    public class Kongzhi
    {
        /// <summary>
        /// 数据
        /// </summary>
        public Shuju_kongzhi Shuju { get; set; } = new Shuju_kongzhi();
        public Fuwuqi_ip Fuwuqi_Ip_ { get; set; }
        /// <summary>
        /// 设备列表
        /// </summary>
        public List<Shebei_Fu> Shebei_Fus { get; set; } = new List<Shebei_Fu>();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="shuju_Kongzhi"></param>
        public void chushihua(Shuju_kongzhi shuju_Kongzhi,Fuwuqi_ip fuwuqi_Ip)
        {
            Shuju = shuju_Kongzhi;
            Fuwuqi_Ip_ = fuwuqi_Ip;
        }
        /// <summary>
        /// 更新设备数据
        /// </summary>
        /// <param name="json_str">包含所有设备数据的外层JSON</param>
        public void Gengxin(Shuju_chuang shuju_Chuang)
        {
            //更新控制中心数据
            Shuju.Zhujiming = shuju_Chuang.Kongzhi.Zhujiming;
            //更新设备数据
            for (int i = 0; i < shuju_Chuang.shuju_Shebei_PCs.Count; i++)
            {
                Shebei_PC shebei_PC = null;
                //根据唯一识别号匹配设备
                foreach (var item in Shebei_Fus)
                {
                    if (item.Shuju.Weiyi_shibie == shuju_Chuang.shuju_Shebei_PCs[i].Weiyi_shibie)
                    {
                        shebei_PC = item as Shebei_PC;
                        shebei_PC.Gengxin(shuju_Chuang.shuju_Shebei_PCs[i]);
                    }
                }

                //未匹配到设备
                if (shebei_PC == null)
                {
                    shebei_PC = new Shebei_PC();
                    shebei_PC.Chushihua(shuju_Chuang.shuju_Shebei_PCs[i].Weiyi_shibie);
                    Shebei_Fus.Add(shebei_PC);
                }
                
                shebei_PC.Gengxin(shuju_Chuang.shuju_Shebei_PCs[i]);
            }
            //更新设备数据
            for (int i = 0; i < shuju_Chuang.shuju_Shebei_ADs.Count; i++)
            {
                Shebei_AD shebei_AD = null;
                //根据唯一识别号匹配设备
                foreach (var item in Shebei_Fus)
                {
                    if (item.Shuju.Weiyi_shibie == shuju_Chuang.shuju_Shebei_ADs[i].Weiyi_shibie)
                    {
                        shebei_AD = item as Shebei_AD;
                        shebei_AD.Gengxin(shuju_Chuang.shuju_Shebei_ADs[i]);
                    }
                }

                //未匹配到设备
                if (shebei_AD == null)
                {
                    shebei_AD = new Shebei_AD();
                    shebei_AD.Chushihua(shuju_Chuang.shuju_Shebei_PCs[i].Weiyi_shibie);
                    Shebei_Fus.Add(shebei_AD);
                }

                shebei_AD.Gengxin(shuju_Chuang.shuju_Shebei_ADs[i]);
            }

            //删除已经断开连接设备
            for (int k = 0; k < Shebei_Fus.Count; k++)
            {
                bool shifou_cunzai = false;
                //根据唯一识别号匹配设备
                for (int i = 0; i < shuju_Chuang.shuju_Shebei_PCs.Count; i++)
                {
                    if (Shebei_Fus[k].Shuju.Weiyi_shibie== shuju_Chuang.shuju_Shebei_PCs[i].Weiyi_shibie)
                    {
                        shifou_cunzai = true;
                        break;
                    }
                }
                for (int i = 0; i < shuju_Chuang.shuju_Shebei_ADs.Count; i++)
                {
                    if (Shebei_Fus[k].Shuju.Weiyi_shibie== shuju_Chuang.shuju_Shebei_ADs[i].Weiyi_shibie)
                    {
                        shifou_cunzai = true;
                        break;
                    }
                }
                //未匹配到设备
                if (shifou_cunzai == false)
                {
                    Shebei_Fus.RemoveAt(k);
                }
            }

        }
        /// <summary>
        /// 发送命令
        /// </summary>
        public void Send_Mingling(Mingling_Fu mingling_Fu)
        {
            string jieguo = "";
            using (TextWriter file = new StringWriter())
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, mingling_Fu);
                jieguo = file.ToString();
            }
            //发送当前设备数据

            //绑定IP
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(Fuwuqi_Ip_.Ip), Fuwuqi_Ip_.Daunkou);
            //连接服务器
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jieguo);


            server.SendTo(bytes, ip);
            server.Close();

        }
        /// <summary>
        /// 修改控制中心名称
        /// </summary>
        public void Xiugai_mingzi(string new_name)
        {
            Mingling_Fu mingling_Fu = new Mingling_Fu
            {
                Mingling = "修改主机名",
                Canshu = new_name,
                Shebei_shibie = 3
            };

            Send_Mingling(mingling_Fu);
        }
    }

    public class Shuju_kongzhi
    {
        /// <summary>
        /// 主机名称
        /// </summary>
        public string Zhujiming { get; set; }
        /// <summary>
        /// 唯一识别号 16位数字
        /// </summary>
        public string Weiyi_shibie { get; set; }
    }
}
