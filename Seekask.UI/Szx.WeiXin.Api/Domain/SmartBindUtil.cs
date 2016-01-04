using Ivony.Html;
using Ivony.Html.Parser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Szx.WeiXin.Api.Models;

namespace Szx.WeiXin.Api.Domain
{
    /// <summary>
    /// 
    /// 作者：石忠孝   
    /// 文件名：SmartBindUtil
    /// 创建时间：2015/1/1 11:57:40
    /// 修改人：石忠孝
    /// 修改时间：2015/1/1 11:57:40
    /// 说明：
    ///     智能绑定核心类 
    /// </summary>
    public class SmartBindUtil : IDisposable
    {
        #region 模拟请求所用的参数设置
        public readonly static string COOKIE_H = "Cookie";

        public readonly static string CONNECTION_H = "Connection";
        public readonly static string CONNECTION = "keep-alive";

        public readonly static string CACHE_CONTROL_H = "Cache-Control";
        public readonly static string CACHE_CONTROL = "max-age=0";

        public readonly static string HOST_H = "Host";
        public readonly static string HOST = "mp.weixin.qq.com";

        public readonly static string ACCEPT_H = "Accept";
        public readonly static string ACCEPT = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";

        public readonly static string ORIGIN_H = "Origin";
        public readonly static string ORIGIN = "https://mp.weixin.qq.com";

        public readonly static string XMLHTTP_REQUEST_H = "X-Requested-With";
        public readonly static string XMLHTTP_REQUEST = "XMLHttpRequest";

        public readonly static string USER_AGENT_H = "User-Agent";
        public readonly static string USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.154 Safari/537.36";

        public readonly static string CONTENT_TYPE_H = "Content-Type";
        public readonly static string CONTENT_TYPE = "application/x-www-form-urlencoded; charset=UTF-8";

        public readonly static string REFERER_H = "Referer";
        public readonly static string REFERER = "https://mp.weixin.qq.com/";

        public readonly static string ACCEPT_ENCODEING_H = "Accept-Encoding";
        public readonly static string ACCEPT_ENCODEING = "gzip,deflate,sdch";

        public readonly static string ACCEPT_LANGUAGE_H = "Accept-Language";
        public readonly static string ACCEPT_LANGUAGE = "zh-CN,zh;q=0.8";



        public readonly static string UTF_8 = "UTF-8";
        #endregion

        private HttpClientHandler handler = new HttpClientHandler();
        private HttpClient _httpClient;
        private bool isLogin = false;
        private string cookiestr;
        private string loginUser;
        private string loginPwd;
        private string token;
        private PostBackModel loginBack;

        public SmartBindUtil(string loginUser, string loginPwd)
        {
            this.loginUser = loginUser;
            this.loginPwd = loginPwd;

            handler.UseCookies = true;
        }

        /// <summary>
        /// 登陆微信并得到开发基本配置信息 
        /// </summary>
        /// <returns></returns>
        public WechatDevInfo GetWechatDev()
        {
            int staus = Login();
            if (staus == (int)HttpStatusCode.OK)
            {
                isLogin = true;
                return GetWechatDevInfo();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 登陆微信
        /// </summary>
        /// <returns></returns>
        public int Login()
        {
            if (isLogin)
            {
                return (int)HttpStatusCode.OK;
            }

            HttpResponseMessage response = null;
            //正在登陆微信公众平台...
            try
            {
                _httpClient = new HttpClient(handler);
                SetHeader(); //设置httpClient头

                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(CONTENT_TYPE_H, CONTENT_TYPE);

                //java DigestUtils.md5Hex(this.loginPwd.getBytes())
                //c#  Md5Helper.Md5Hex(this.loginPwd); 

                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                paramList.Add(new KeyValuePair<string, string>("username", this.loginUser));
                paramList.Add(new KeyValuePair<string, string>("pwd", Md5Helper.Md5Hex(this.loginPwd)));
                //paramList.Add(new KeyValuePair<string, string>("pwd", "d3d40c3dd1acd940d4a98d16d75897fe"));
                paramList.Add(new KeyValuePair<string, string>("imgcode", ""));
                paramList.Add(new KeyValuePair<string, string>("f", "json"));

                var uri = new Uri(WeChatUrl.LOGIN_URL);
                response = _httpClient.PostAsync(uri,
                    new FormUrlEncodedContent(paramList)).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //登陆成功
                    string result = response.Content.ReadAsStringAsync().Result;

                    loginBack = JsonConvert.DeserializeObject<PostBackModel>(result);

                    if (loginBack.base_resp.ret == 0)
                    {  //微信验证成功
                        StringBuilder cookie = new StringBuilder();

                        CookieCollection getCookies = handler.CookieContainer.GetCookies(uri);
                        foreach (Cookie c in getCookies)
                        {
                            cookie.Append(c.Name).Append("=")
                                .Append(c.Value).Append(";");
                        }
                        this.cookiestr = cookie.ToString();
                        //正在获取token
                        #region 正在获取token
                        if (!string.IsNullOrWhiteSpace(loginBack.redirect_url))
                        {
                            string[] ss = loginBack.redirect_url.Split('?');
                            string[] ps = null;
                            if (ss.Length == 2)
                            {
                                if (!string.IsNullOrWhiteSpace(ss[1])
                                        && ss[1].IndexOf("&") != -1)
                                    ps = ss[1].Split('&');
                            }
                            else if (ss.Length == 1)
                            {
                                if (!string.IsNullOrWhiteSpace(ss[0])
                                        && ss[0].IndexOf("&") != -1)
                                    ps = ss[0].Split('&');
                            }
                            if (ps != null)
                            {
                                foreach (var p in ps)
                                {
                                    if (string.IsNullOrWhiteSpace(p))
                                        continue;
                                    string[] tk = p.Split('=');
                                    if (!string.IsNullOrWhiteSpace(tk[0])
                                        && "token".Equals(tk[0].Trim().ToLower()))
                                    {
                                        if (!string.IsNullOrEmpty(tk[1]))
                                            token = tk[1].Trim();
                                        break;
                                    }
                                }
                            }
                            //获取token成功.. 
                        }
                        #endregion

                        //进入首页
                        return Index();
                    }
                    else
                    {  //验证错误
                        string err_msg = loginBack.base_resp.err_msg;
                    }
                }

            }
            catch (Exception e)
            {
                return -1;
            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
            return 0;
        }

        /// <summary>
        /// 进入首页
        /// </summary>
        /// <returns></returns>
        private int Index()
        {
            if (loginBack == null)
                return -1;

            HttpResponseMessage response = null;
            try
            {
                _httpClient = new HttpClient(handler);
                SetHeader();

                response = _httpClient.GetAsync(WeChatUrl.HOST_URL + loginBack.redirect_url).Result;
                //if (response.StatusCode == HttpStatusCode.OK)
                //{   //已经连接，正在接收数据

                //    string result = response.Content.ReadAsStringAsync().Result;
                //}
                return (int)response.StatusCode;
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
        }

        /// <summary>
        /// 得到AppId,AppSecret 
        /// </summary>
        /// <returns></returns>
        public WechatDevInfo GetWechatDevInfo()
        {

            // TODO 得到AppId,AppSecret  
            WechatDevInfo devInfo = null;
            HttpResponseMessage response = null;
            try
            {
                _httpClient = new HttpClient(handler);
                SetHeader();

                response = _httpClient.GetAsync(WeChatUrl.DEV_URL + token).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {   //已经连接，正在接收数据

                    string result = response.Content.ReadAsStringAsync().Result;

                    var parser = new JumonyParser();
                    var htmlDoc = parser.Parse(result);
                    var htmlEles = htmlDoc.Find(".developer_info_wrp");
                    if (htmlEles != null && htmlEles.Count() > 0)
                    {
                        var vertical = htmlEles.Find(".frm_vertical_pt").ToList();
                        devInfo = new WechatDevInfo();
                        #region  解析html获取相关文本信息
                        for (int i = 0; i < vertical.Count; i++)
                        {
                            try
                            {
                                var infoText = vertical[i].InnerText().Trim();
                                if (string.IsNullOrWhiteSpace(infoText))
                                    continue;
                                switch (i)
                                {
                                    case 0: devInfo.AppId = infoText;
                                        break;
                                    case 1: devInfo.AppSecret = infoText;
                                        break;
                                    case 2: devInfo.URL = infoText;
                                        break;
                                    case 3: devInfo.Token = infoText;
                                        break;
                                    case 4: devInfo.EncodingAESKey = infoText;
                                        break;
                                    case 5:
                                        SetEncodingAESType(devInfo.EncodingAESType, infoText);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            catch (Exception){}
                        }
                        #endregion
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
            return devInfo;
        }

        /// <summary>
        /// 得到微信公众平台个人信息 
        /// </summary>
        /// <returns></returns>
        public WechatAccountInfo GetAccount()
        {

            WechatAccountInfo account = null;
            HttpResponseMessage response = null;
            try
            {
                _httpClient = new HttpClient(handler);
                SetHeader();

                response = _httpClient.GetAsync(WeChatUrl.ACCOUNT_INFO_URL + token).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {   //已经连接，正在接收数据

                    string result = response.Content.ReadAsStringAsync().Result;

                    var parser = new JumonyParser();
                    var htmlDoc = parser.Parse(result);
                    var htmlEles = htmlDoc.Find(".account_setting_area .account_setting_item .meta_content");
                    if (htmlEles != null && htmlEles.Count() > 0)
                    {
                        var setting = htmlEles.ToList();
                        account = new WechatAccountInfo();

                        #region  解析html获取相关文本信息
                        for (int i = 0; i < setting.Count; i++)
                        {
                            try
                            {

                                var infoText = setting[i].InnerText().Trim();
                                if (i > 1 && string.IsNullOrWhiteSpace(infoText))
                                    continue;
                                switch (i)
                                {
                                    case 0: account.HeadImage = setting[0].Find("img").FirstOrDefault()
                                        .Attribute("src").AttributeValue;
                                        break;
                                    case 1: account.QRCode = setting[1].Find("img").FirstOrDefault()
                                         .Attribute("src").AttributeValue;
                                        break;
                                    case 2: account.AccountName = infoText;
                                        break;
                                    case 3: account.WechatNumber = infoText;
                                        break;
                                    case 4: SetWechatType(account.WechatType, infoText);
                                        break;
                                    case 5: account.Introduces = infoText;
                                        break;
                                    case 6: SetAuthenticate(account.Authenticate, infoText);
                                        break;
                                    case 7: account.PlaceAddress = infoText;
                                        break;
                                    case 8: account.SubjectInfo = infoText;
                                        break;
                                    case 9: account.LoginEmail = infoText;
                                        break;
                                    case 10: account.AccountId = infoText;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            catch (Exception) { }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
            return account;
        }

        /// <summary>
        /// 切换开发模式/编辑模式 
        /// </summary>
        /// <param name="flag">开启1 关闭0 </param>
        /// <param name="type">开发模式2 编辑模式1 </param>
        /// <returns></returns>
        public int EnabledDev(int flag, int type)
        {
            int staus = Login();
            if (staus == (int)HttpStatusCode.OK)
            {
                isLogin = true;

                HttpResponseMessage response = null;
                try
                {
                    _httpClient = new HttpClient(handler);
                    SetHeader();
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(CONTENT_TYPE_H, CONTENT_TYPE);

                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<string, string>("flag", flag.ToString()));
                    paramList.Add(new KeyValuePair<string, string>("type", type.ToString()));
                    paramList.Add(new KeyValuePair<string, string>("token", token));

                    var uri = new Uri(WeChatUrl.DEV_UPDATE_RUL);
                    response = _httpClient.PostAsync(uri,
                        new FormUrlEncodedContent(paramList)).Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //连接成功
                        string result = response.Content.ReadAsStringAsync().Result;

                        PostBackModel xhStatus = JsonConvert.DeserializeObject<PostBackModel>(result);
                        if (xhStatus != null)
                            return xhStatus.base_resp.ret;
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    if (response != null)
                        response.Dispose();
                }
            }
            else
            {

            }
            return -1;
        }

        /// <summary>
        /// 设置开发模式，服务器回调 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback_token"></param>
        /// <returns></returns>
        public int SetDevServiceUrl(string url, string callback_token, string encoding_aeskey,
            string callback_encrypt_mode)
        {

            int staus = Login();
            if (staus == (int)HttpStatusCode.OK)
            {
                isLogin = true;

                HttpResponseMessage response = null;
                try
                {
                    _httpClient = new HttpClient(handler);
                    SetHeader();
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(CONTENT_TYPE_H, CONTENT_TYPE);

                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<string, string>("url", "" + url));
                    paramList.Add(new KeyValuePair<string, string>("callback_token", callback_token));
                    paramList.Add(new KeyValuePair<string, string>("encoding_aeskey", encoding_aeskey));
                    paramList.Add(new KeyValuePair<string, string>("callback_encrypt_mode", callback_encrypt_mode));
                    paramList.Add(new KeyValuePair<string, string>("operation_seq", "401119815"));

                    var uri = new Uri(WeChatUrl.DEV_SERVICE_URL + token);
                    //正在设置公众平台回调
                    response = _httpClient.PostAsync(uri,
                        new FormUrlEncodedContent(paramList)).Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //服务器连接成功
                        string result = response.Content.ReadAsStringAsync().Result;

                        PostBackModel xhStatus = JsonConvert.DeserializeObject<PostBackModel>(result);
                        if (xhStatus != null)
                            return xhStatus.base_resp.ret;

                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    if (response != null)
                        response.Dispose();
                }
            }
            else
            {

            }
            return -1;
        }

        /// <summary>
        /// 退出微信
        /// </summary>
        /// <returns></returns>
        public int Logout()
        {
            //退出服务...
            HttpResponseMessage response = null;
            try
            {
                _httpClient = new HttpClient(handler);
                SetHeader();

                response = _httpClient.GetAsync(WeChatUrl.LOGOUT_URL + token).Result;
                return (int)response.StatusCode;
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
        }

        /// <summary>
        /// 设置http Post头
        /// </summary>
        private void SetHeader()
        {
            _httpClient.DefaultRequestHeaders.Add(CONNECTION_H, CONNECTION);
            _httpClient.DefaultRequestHeaders.Add(HOST_H, HOST);
            _httpClient.DefaultRequestHeaders.Add(REFERER_H, REFERER);
            _httpClient.DefaultRequestHeaders.Add(USER_AGENT_H, USER_AGENT);

            _httpClient.DefaultRequestHeaders.Add(XMLHTTP_REQUEST_H, XMLHTTP_REQUEST);

            if (this.cookiestr != null && this.cookiestr.Length > 0)
            {
                _httpClient.DefaultRequestHeaders.Add(COOKIE_H, this.cookiestr);
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {

            if (handler != null)
                handler.Dispose();
            if (_httpClient != null)
                _httpClient.Dispose();

        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="imgUrl">图片所在的路径</param>
        /// <param name="savePath">保存本地的地址</param>
        /// <returns></returns>
        public int DownloadImg(string imgUrl, string fileName)
        {
            int staus = Login();
            if (staus == (int)HttpStatusCode.OK)
            {
                isLogin = true;

                #region 验证保存路径是否存在，并且文件不能存在

                FileInfo finfo = new FileInfo(fileName);
                if (finfo.Exists)
                {
                    return -2;
                }
                DirectoryInfo dinfo = finfo.Directory;
                if (!dinfo.Exists)
                    dinfo.Create();

                #endregion

                HttpResponseMessage response = null;
                try
                {
                    _httpClient = new HttpClient(handler);
                    SetHeader();

                    response = _httpClient.GetAsync(WeChatUrl.HOST_URL + imgUrl).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {   //已经连接，正在接收数据
                        byte[] result = response.Content.ReadAsByteArrayAsync().Result;

                        using (MemoryStream ms = new MemoryStream(result))
                        {
                            using (Image outputImg = Image.FromStream(ms))
                            {
                                outputImg.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            };
                        }
                    }
                    return (int)response.StatusCode;
                }
                catch (Exception)
                {
                    return -1;
                }
                finally
                {
                    if (response != null)
                        response.Dispose();
                }
            }
            return -1;
        }

        /// <summary>
        /// 设置消息加密方式
        /// </summary>
        /// <param name="encodingAESType"></param>
        /// <param name="infoText"></param>
        private static void SetEncodingAESType(EncodingAESType encodingAESType, string infoText)
        {
            switch (infoText)
            {
                case "明文模式": encodingAESType = EncodingAESType.明文模式;
                    break;
                case "兼容模式": encodingAESType = EncodingAESType.兼容模式;
                    break;
                case "安全模式": encodingAESType = EncodingAESType.安全模式;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 设置微信号类型
        /// </summary>
        /// <param name="encodingAESType"></param>
        /// <param name="infoText"></param>
        private static void SetWechatType(WechatType wechatType, string infoText)
        {
            switch (infoText)
            {
                case "订阅号": wechatType = WechatType.订阅号;
                    break;
                case "服务号": wechatType = WechatType.服务号;
                    break;
                case "企业号": wechatType = WechatType.企业号;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 设置微信号认证情况
        /// </summary>
        /// <param name="encodingAESType"></param>
        /// <param name="infoText"></param>
        private static void SetAuthenticate(Authenticate authenticate, string infoText)
        {
            switch (infoText)
            {
                case "未认证": authenticate = Authenticate.未认证;
                    break;
                case "已认证": authenticate = Authenticate.已认证;
                    break;
                default:
                    break;
            }
        }

    }

    /// <summary>
    /// //{"base_resp": {"ret": 0,"err_msg": "ok"},
    ///    "redirect_url": "/cgi-bin/home?t=home/index&lang=zh_CN&token=1714666153"}
    /// </summary>
    public class PostBackModel
    {

        public BaseResp base_resp { get; set; }

        public class BaseResp
        {
            public int ret { get; set; }
            public string err_msg { get; set; }
        }

        public string redirect_url { get; set; }
    }

}
