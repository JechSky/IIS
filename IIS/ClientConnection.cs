using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IISFormForFun
{
    public class ClientConnection
    {
        Socket sokClient;
        DGPrintMsg dgPrintMsg;
        Thread trdMsg;//通信线程

        /// <summary>
        /// 创建客户端通信的套接字
        /// </summary>
        /// <param name="sokClient"></param>
        /// <param name="dgPrintMsg"></param>
        public ClientConnection(Socket sokClient, DGPrintMsg dgPrintMsg)
        {
            this.sokClient = sokClient;
            this.dgPrintMsg = dgPrintMsg;

            trdMsg = new Thread(ReceiveMsg);
            trdMsg.IsBackground = true;
            trdMsg.Start();

        }

        bool isRec = true;

        #region 服务端循环接收浏览器发来的消息
        /// <summary>
        /// 服务端循环接收浏览器发来的消息
        /// </summary>
        void ReceiveMsg()
        {
            try
            {
                //while (isRec)
                //{
                if (sokClient != null)
                {
                    //准备接收浏览器发来的消息缓冲区
                    byte[] arrMsg = new byte[1024 * 1024];
                    int realLength = sokClient.Receive(arrMsg);
                    string strRequest = Encoding.UTF8.GetString(arrMsg, 0, realLength);
                    dgPrintMsg(strRequest, MsgType.UserMsg);

                    //将请求报文字符串封装到请求报文实体对象中
                    RequestModel requestModel = new RequestModel(strRequest);
                    //创建处理请求类对象
                    ProcessRequestBus process = new ProcessRequestBus(requestModel);
                    //获得响应报文对象
                    ResponseModel responseModel = process.GetResponseModel();
                    if (responseModel != null)
                    {
                        //发起响应报文头
                        sokClient.Send(responseModel.GetHeader());
                        sokClient.Send(Encoding.UTF8.GetBytes("\r\n\r\n"));
                        //发起响应报文体
                        sokClient.Send(responseModel.GetBody());

                        dgPrintMsg("响应客户端请求完毕！", MsgType.SysMsg);

                        isRec = false;
                        if (sokClient.Connected)
                        {
                            //禁用发送和接受
                            sokClient.Shutdown(SocketShutdown.Both);
                            //关闭套接字，不允许重用
                            sokClient.Disconnect(false);
                        }
                        this.sokClient.Close();
                    }
                    else
                    {
                        dgPrintMsg("异常：服务器无法响应！", MsgType.SysMsg);
                    }

                }

               // }
            }
            catch (Exception ex)
            {
                dgPrintMsg("异常：" + ex.Message, MsgType.SysMsg);
            }
        } 
        #endregion

    }
}
