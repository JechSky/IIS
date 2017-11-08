using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IISFormForFun
{
    /// <summary>
    /// 处理请求操作类
    /// </summary>
    public class ProcessRequestBus
    {
        /// <summary>
        /// 请求报文对象
        /// </summary>
        RequestModel requestModel;
        public ProcessRequestBus(RequestModel requestModel)
        {
            this.requestModel = requestModel;
        }

        #region 根据请求实体获得响应报文实体
        /// <summary>
        /// 根据请求实体获得响应报文实体
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetResponseModel()
        {
            //获得请求的页面后缀
            string strExtenion = Path.GetExtension(requestModel.Path);
            ResponseModel model = ProcessWithExtiontion(strExtenion);

            return model;
        } 
        #endregion

        #region 根据被请求文件后缀来调用不同方法处理
        /// <summary>
        /// 根据被请求文件后缀来调用不同方法处理
        /// </summary>
        /// <param name="strExtention"></param>
        /// <returns></returns>
        ResponseModel ProcessWithExtiontion(string strExtention)
        {
            ResponseModel model = null;
            switch (strExtention)
            {
                //处理4种字符串静态文本
                case ".html":
                    model = ProcessStatic();
                    break;
                case ".htm":
                    //model = ProcessStatic();
                    break;
                case ".js":
                    break;
                case ".css":
                    model = ProcessStatic();
                    break;

                //处理动态页面
                case ".aspx":
                    break;
                case ".php":
                    break;
                case ".jsp":
                    break;
                case ".ashx":
                    model = ProcessDym();
                    break;

                //处理图片
                case ".jpg":
                    model = ProcessImg();
                    break;
                case ".ico":
                    model = ProcessImg();
                    break;
                default:
                    break;


            }
            return model;
        }
        #endregion

        #region 处理静态页面
        /// <summary>
        /// 处理静态页面
        /// </summary>
        /// <returns></returns>
        ResponseModel ProcessStatic()
        {
            //获得当前程序集的文件夹路径
          string dataDir = AppDomain.CurrentDomain.BaseDirectory;
            //获得被请求文件的物理路径
            dataDir += "" + requestModel.Path;
            //
            string strFileContent = File.ReadAllText(dataDir);
            byte[] arrFileBody = Encoding.UTF8.GetBytes(strFileContent);
            ResponseModel model = new ResponseModel(requestModel, arrFileBody);

            return model;
        }
        #endregion

        #region 处理图片
        /// <summary>
        /// 处理图片
        /// </summary>
        /// <returns></returns>
        ResponseModel ProcessImg()
        {
            //获得当前程序集的文件夹路径
            string dataDir = AppDomain.CurrentDomain.BaseDirectory;
            //获得被请求文件的物理路径
            dataDir += "" + requestModel.Path;
            using (FileStream fs=new FileStream (dataDir,FileMode.Open))
            {
                byte[] arrImg = new byte[1024 * 1024 * 2];
                int realLength= fs.Read(arrImg, 0, arrImg.Length);
                byte[] arrImgNew = new byte[realLength];
                //将图片数组的数据存入新数组中
                Buffer.BlockCopy(arrImg, 0, arrImgNew, 0, realLength);

                ResponseModel model = new ResponseModel(requestModel, arrImgNew);

                return model;
            }
            
        }
        #endregion

        #region 处理动态页面
        /// <summary>
        /// 处理动态页面
        /// </summary>
        /// <returns></returns>
        ResponseModel ProcessDym()
        {
            //获得文件名（不包含后缀）
            string strClassName = Path.GetFileNameWithoutExtension(requestModel.Path);
            //获得正在执行的程序集
            Assembly ase = Assembly.GetExecutingAssembly();
            //获得程序集的名字
            string strNameSpace = ase.GetName().Name;
            //根据类的名称创建类型对象
            Type t = ase.GetType(strNameSpace + "." + strClassName);
                //Assembly.GetExecutingAssembly().GetType(strNameSpace+"."+strClassName);
            //根据类型对象，创建对应类的对象
            object o= Activator.CreateInstance(t);
            //将对象转成Index类
            IHttpHandler indexObj = o as IHttpHandler;
            string strBody = indexObj.ProcessRequest();
            byte[] arrBody = Encoding.UTF8.GetBytes(strBody);
            return new ResponseModel(requestModel, arrBody);

        }
        #endregion

    }
}
