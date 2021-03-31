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
    public class Mianban
    {
        /// <summary>
        /// 当前类文件的根目录
        /// </summary>
        private string lujing { get { return Environment.CurrentDirectory + "\\SillyFile\\"; } }
        /// <summary>
        /// 控制中心列表
        /// </summary>
        public List<Kongzhi> Kongzhis { get; set; } = new List<Kongzhi>();

        /// <summary>
        /// 当前主机设备数据
        /// </summary>
        public Shebei_PC Shebei_dangqian { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="weiyishibie">唯一识别号</param>
        public void Chushihua(string weiyishibie)
        {
            Shebei_dangqian = new Shebei_PC();
            Shebei_dangqian.Chushihua(weiyishibie);
        }
        /// <summary>
        /// 添加新的控制中心
        /// </summary>
        /// <param name="fuwuqi_Ip"></param>
        public void Add_kongzhi(Fuwuqi_ip fuwuqi_Ip)
        {
            //检查
            foreach(var ips in App.Peizhi_.Shuju.Fuwuqi_Ips)
            {
                if(ips.Daunkou==fuwuqi_Ip.Daunkou&&ips.Ip==fuwuqi_Ip.Ip)
                {
                    return;
                }
            }

            //尝试连接
            Lianjie(fuwuqi_Ip);
        }


        /// <summary>
        /// 连接控制中心
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="duankou">端口</param>
        public void Lianjie(Fuwuqi_ip fuwuqi_Ip)
        {
            //发送当前设备数据

            //绑定IP
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(fuwuqi_Ip.Ip), fuwuqi_Ip.Daunkou);
            //连接服务器
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //格式化当前设备数据
            string str = Shebei_dangqian.Shuju_json();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);


            server.SendTo(bytes, ip);
            server.Close();
        }

        /// <summary>
        /// 调用此方法更新本地主机数据 
        /// </summary>
        public void Gengxin_bendi()
        {
            //获取数据
            Shuju_Shebei_PC shuju_Shebei_ = Gongju.Huoqu_shebei_xinxi();
            shuju_Shebei_.Shebei_shibie = Shebei_dangqian.Shuju.Shebei_shibie;
            shuju_Shebei_.Mingchen = App.Peizhi_.Shuju.Zhujiming;
            shuju_Shebei_.Weiyi_shibie = App.Peizhi_.Shuju.Weiyi_shibie;
            Shebei_dangqian.Shuju = shuju_Shebei_;
            Shebei_dangqian.Baocun();
        }
        /// <summary>
        /// 向每一个控制中心传递当前数据
        /// </summary>
        /// <param name="fuwuqi_Ips"></param>
        public void Send_shuju(List<Fuwuqi_ip> fuwuqi_Ips)
        { 
            foreach (var item in fuwuqi_Ips)
            {
                Lianjie(item);
            }

        }
        /// <summary>
        /// 建立UDP服务器监听 尝试异步使用
        /// </summary>
        public void Udp_jianting()
        {
            UdpClient udpclient = new UdpClient(22334);
            IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Any, 22334);
            try
            {
                while (true)
                {
                    //接收数据
                    byte[] bytes = udpclient.Receive(ref ipendpoint);
                    string strIP = "信息来自" + ipendpoint.Address.ToString();
                    string strInfo = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    //分析控制中心
                    Fengxi_shuju(strInfo,new Fuwuqi_ip() { Ip = ipendpoint.Address.ToString(), Daunkou = ipendpoint.Port });
                }
            }
            catch (Exception e)
            {

            }
            
        }
        
        /// <summary>
        /// 移除服务器
        /// </summary>
        /// <param name="kongzhi"></param>
        public void Remove_fu(Kongzhi kongzhi)
        {
            try
            {
                App.Peizhi_.Remove_ip(kongzhi.Fuwuqi_Ip_);
                Kongzhis.Remove(kongzhi);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 分析从控制中心获取的数据
        /// </summary>
        /// <param name="json_str"></param>
        public void Fengxi_shuju(string json_str,Fuwuqi_ip fuwuqi_Ip)
        {
            try
            {
                Shuju_chuang shuju_Chuang = null;
                Kongzhi kongzhi = null;

                using (TextReader str = new StringReader(json_str))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    shuju_Chuang = (Shuju_chuang)serializer.Deserialize(str, typeof(Shuju_chuang));
                }
                if(shuju_Chuang!=null)
                {
                    //查找是否存在控制中心
                    foreach(var item in Kongzhis)
                    {
                        if(item.Shuju.Weiyi_shibie==shuju_Chuang.Kongzhi.Weiyi_shibie)
                        {
                            kongzhi = item;
                        }
                    }
                    if(kongzhi==null)
                    {
                        kongzhi = new Kongzhi();
                        kongzhi.chushihua(shuju_Chuang.Kongzhi,fuwuqi_Ip);
                        Kongzhis.Add(kongzhi);
                    }

                    //调用控制中心更新
                    kongzhi.Gengxin(shuju_Chuang);
                }
            }
            catch (Exception exc)
            {

            }
        }
    }
}
