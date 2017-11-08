using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISFormForFun
{
    /// <summary>
    /// 请求报文数据实体
    /// </summary>
    public class RequestModel
    {
        private string path;
        string strRequest;

        /// <summary>
        /// 用户请求的页面路径
        /// </summary>
        public string Path { get => path; set => path = value; }
        /// <summary>
        /// 用户浏览器的请求报文
        /// </summary>
        public string StrRequest { get => strRequest; set => strRequest = value; }

        /// <summary>
        /// 创建请求报文的同时分析请求报文
        /// </summary>
        /// <param name="strRequest"></param>
        public RequestModel(string strRequest)
        {
            this.strRequest = strRequest;
            // \r\n 
            string[] arrStr = strRequest.Replace("\r\n", "錒").Split('錒'); //strRequest.Split(new char[4] { '\\', 'r', '\\', 'n' },200);
             //strRequest.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);

            //获得请求报文里的第一行
            string strLineOne = arrStr[0];

            //获得第一行中的请求路径
            path = strLineOne.Split(' ')[1].Replace("/", "");

            
        }

        
    }
}
