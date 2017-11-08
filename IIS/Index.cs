using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISFormForFun
{
    public class Index: IHttpHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ProcessRequest()
        {

            return @"<html>
                    <head><title>我是动态页面哦！！！</title></head>
                    <body>世界大部分都是s</body>
                    </html>";

        }

    }
}
