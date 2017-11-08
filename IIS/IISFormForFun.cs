using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace IISFormForFun
{
    public partial class IISFormForFun : Form
    {
        public IISFormForFun()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        Socket sokWatch;
        Thread trdWatch;
        private void btnStart_Click(object sender, EventArgs e)
        {
            StartWatch();
        }

        /// <summary>
        /// 启动监听
        /// </summary>
        void StartWatch()
        {
            try
            {
                sokWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(txtIP.Text.Trim()), int.Parse(txtPort.Text.Trim()));
                sokWatch.Bind(endPoint);
                sokWatch.Listen(10);//设置监听队列；同时能够处理的最大请求连接数

                trdWatch = new Thread(WatchPort);
                trdWatch.IsBackground = true;
                trdWatch.Start();

                PrintMsg("服务已启动。。。",MsgType.SysMsg);
            }
            catch (Exception ex)
            {
                PrintMsg("异常："+ex.Message,MsgType.SysMsg);
            }

        }


        bool isWatch = true;
        /// <summary>
        /// 监听退出变量
        /// </summary>
        void WatchPort()
        {
            while (isWatch)
            {
                //监听到浏览器的请求后返回一个与该浏览器通信的套接字
                Socket sokConn = sokWatch.Accept();
                ClientConnection conn = new ClientConnection(sokConn, PrintMsg);

            }
        }

        //void PrintMsg(string strMsg)
        //{
        //    //txtMsg.AppendText(strMsg+"\r\n");
        //    PrintMsg(strMsg, MsgType.UserMsg);
        //}

        //void PrintSysMsg(string strMsg)
        //{
        //    PrintMsg(strMsg, MsgType.SysMsg);
        //}

        void PrintMsg(string strMsg,MsgType type)
        {
            if(type==MsgType.UserMsg)
            {
                txtMsg.AppendText(strMsg + "\r\n");
            }
            else if (type == MsgType.SysMsg)
            {
                txtMsg.AppendText("\r\n**********************\r\n");
                txtMsg.AppendText(strMsg + "\r\n");
                txtMsg.AppendText("\r\n**********************\r\n");
            }

        }

    }
}
