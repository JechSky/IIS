using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISFormForFun
{
    public class List: IHttpHandler
    {
        public string ProcessRequest()
        {
            List<Person> list = new PersonList().GetData();
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<table style='border:1px dashed #000;'>");

            foreach (Person p in list)
            {
                sbHtml.Append("<tr><td>" + p.Name + "</td><td>" + p.Age + "</td><td>" + p.Gender + "</td></tr>");
            }

            sbHtml.Append("</table>");

            return @"<html>
                    <head><title>我是动态页面哦！！！</title></head>
                    <body>世界大部分都是s，对，是的，就是这样！"
                    + sbHtml.ToString()+
                    "</body> </html>";
        }
    }
}
