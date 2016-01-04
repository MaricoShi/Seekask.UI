using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Net.Sockets;

using Newtonsoft.Json;
using System.Collections.Specialized;

namespace Szx.WeiXin.Api
{
    /// <summary>
    /// 
    /// 作者：石忠孝   
    /// 文件名：IRequest
    /// 创建时间：2015/1/1 11:49:15
    /// 修改人：石忠孝
    /// 修改时间：2015/1/1 11:49:15
    /// 说明：
    /// 
    /// </summary>
    public interface IRequestStart<T1, T2>
    {
        T2 Start(T1 arg);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Arg">请求参数</typeparam>
    /// <typeparam name="XmlDoc">返回的数据类型</typeparam>
    public abstract class IRequest<Arg, XmlDoc> where XmlDoc : new()
    {
        /// <summary>
        /// 请求的链接地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 请求内容的类型,text/html,application/json,application/x-www-form-urlencoded
        /// </summary>
        public string contentType = "text/html";
        /// <summary>
        /// 传输数据格式，默认为url方式，暂时扩展支持json格式
        /// </summary>
        public string transType = "text";
        /// <summary>
        /// 编码 默认为UTF8
        /// </summary>
        public Encoding encoding { get; set; }
        /// <summary>
        /// 返回结果字符串
        /// </summary>
        public string resultStr { get; set; }
        /// <summary>
        /// 请求响应超时时间
        /// </summary>
        public int timeOut { get; set; }
        /// <summary>
        /// 获取的返回内容格式
        /// </summary>
        public string format { get; set; }

        Dictionary<string, string> _rsp_headers = new Dictionary<string, string>();
        Dictionary<string, string> _req_headers = new Dictionary<string, string>();

        private Arg _Arg { get; set; }

        private XmlDoc _result { get; set; }


        public IRequest()
        {
            encoding = Encoding.UTF8;
            timeOut = 100000;
            format = Format.xml.ToString();
            contentType = "application/x-www-form-urlencoded";
        }
        /// <summary>
        /// 请求接口
        /// </summary>
        /// <param name="arg">参数</param>
        /// <param name="ac">请求方式</param>
        /// <returns></returns>
        public virtual XmlDoc Start(Arg arg, Action ac)
        {

            if (string.IsNullOrEmpty(this.url))
            {
                throw new Exception("请求地址不能为空");
            }
            try
            {
                _Arg = arg;
                switch (ac)
                {
                    case Action.post:
                        var paraStrPost = transType == "text" ? arg.GetUrlParam<Arg>() 
                            : JsonConvert.SerializeObject(arg);
                        _result = Post(paraStrPost);
                        break;
                    case Action.get:
                        var paraStr = arg.GetUrlParam<Arg>();
                        _result = Get(paraStr);
                        break;
                    default:
                        return new XmlDoc();
                }
                return _result;
            }
            catch (Exception) { }
            finally
            {

            }
            return new XmlDoc();
        }

        public virtual XmlDoc Start(string arg, Action ac)
        {
            if (string.IsNullOrEmpty(this.url))
            {
                throw new Exception("请求地址不能为空");
            }
            try
            {
                switch (ac)
                {
                    case Action.post:
                        _result = Post(arg);
                        break;
                    case Action.get:
                        _result = Get(arg);
                        break;
                    default:
                        return new XmlDoc();
                }
                return _result;
            }
            catch (Exception) { }
            finally
            {

            }
            return new XmlDoc();
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public virtual XmlDoc Get(string paraStr)
        {
            var _url = this.url;

            if (this.url.IndexOf("?") > 0) _url += "&" + paraStr;
            else _url += "?" + paraStr;
            var rq = (HttpWebRequest)WebRequest.Create(_url);
            rq.Method = "GET";
            rq.Timeout = timeOut;
            foreach (string r in rq.Headers.Keys)
            {
                _req_headers.Add(r, rq.Headers[r]);
            }
            var rs = rq.GetResponse();
            foreach (string r in rs.Headers.Keys)
            {
                _rsp_headers.Add(r, rs.Headers[r]);
            }
            var sr = new StreamReader(rs.GetResponseStream(), encoding);
            resultStr = sr.ReadToEnd();
            rs.Close();
            sr.Close();
            rq.Abort();
            rq = null;
            return Decode(resultStr);

        }


        /// <summary>
        /// post
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public virtual XmlDoc Post(string paraStr)
        {
            var _url = this.url;
            var postdata = encoding.GetBytes(paraStr);
            var rq = HttpWebRequest.Create(_url);
            rq.Method = "POST";
            rq.Timeout = timeOut;
            rq.ContentType = contentType;
            rq.ContentLength = postdata.Length;

            var srw = rq.GetRequestStream();
            srw.Write(postdata, 0, postdata.Length);
            srw.Close();
            var rs = rq.GetResponse();

            var srr = new StreamReader(rs.GetResponseStream(), encoding);
            resultStr = srr.ReadToEnd();
            rs.Close();
            srr.Close();
            rq.Abort();
            rq = null;
            return Decode(resultStr);
        }

        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }

        public virtual XmlDoc Decode(string str)
        {
            if (string.IsNullOrEmpty(str)) return new XmlDoc();
            switch (format)
            {
                case "json":
                    return DecodeJson(str);
                default:
                    return new XmlDoc();
            }
        }

        /// <summary>
        /// 根据不同的json对象解析
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public virtual XmlDoc DecodeJson(string str)
        {
            return JsonConvert.DeserializeObject<XmlDoc>(str);
        }



    }


    
}
