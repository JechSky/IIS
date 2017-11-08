using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISFormForFun
{
    /// <summary>
    /// 响应报文实体类
    /// </summary>
    public class ResponseModel
    {
        //GET /1.jpg HTTP/1.1
        //Host: 127.0.0.1:50001
        //Connection: keep-alive
        //User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36
        //Accept: image/webp,image/*,*/*;q=0.8
        //Referer: http://127.0.0.1:50001/1.html
        //Accept-Encoding: gzip, deflate, sdch, br
        //Accept-Language: zh-CN,zh;q=0.8

        RequestModel requestModel;
        byte[] bodyArr;
        public ResponseModel(RequestModel requestModel, byte[] content)
        {
            this.requestModel = requestModel;
            this.bodyArr = content;
        }

        #region 获得响应报文头的字节数据
        /// <summary>
        /// 获得响应报文头的字节数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetHeader()
        {
            StringBuilder sbHeader = new StringBuilder("HTTP/1.1 200 ok\r\n");
            sbHeader.Append("Content-Type:" + GetContentType() + ";charset=utf-8\r\n");
            sbHeader.Append("Content-Length" + bodyArr.Length);

            byte[] arrHeader = Encoding.UTF8.GetBytes(sbHeader.ToString());

            return arrHeader;
        }
        #endregion

        #region 获得响应报文体的类型
        /// <summary>
        /// 获得响应报文体的类型
        /// </summary>
        /// <returns></returns>
        string GetContentType()
        {
            string strExtention = Path.GetExtension(requestModel.Path);
            switch (strExtention)
            {
                //处理4种字符串静态文本
                case ".html":
                    break;
                case ".htm":
                    return "text/html";
                    break;
                case ".js":
                    return "text/javascript";
                    break;
                case ".css":
                    return "text/css";
                    break;

                //处理动态页面
                case ".aspx":
                    break;
                case ".php":
                    break;
                case ".jsp":
                    break;
                case ".ashx":
                    return "text/html";
                    break;


                //处理图片
                case ".jpg":
                    return "image/jpeg";
                    break;
                case ".ico":
                    return "image/ico";
                    break;

                default:
                    return "text/plain";
                    break;

            }
            return "";

        }
        #endregion

        #region 获得响应报文体的字节数据
        /// <summary>
        /// 获得响应报文体的字节数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetBody()
        {

            return bodyArr;
        }
        #endregion

    }
}
