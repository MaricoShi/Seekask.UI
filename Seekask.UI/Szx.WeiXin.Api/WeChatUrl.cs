using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szx.WeiXin.Api
{
    /// <summary>
    /// 
    /// 作者：石忠孝   
    /// 文件名：WeChatUrl
    /// 创建时间：2015/1/1 12:04:59
    /// 修改人：石忠孝
    /// 修改时间：2015/1/1 12:04:59
    /// 说明：
    ///     微信相关地址定义
    /// </summary>
    public class WeChatUrl
    {
        /// <summary>
        ///  微信URL
        /// </summary>
        public readonly static string HOST_URL =
            "https://mp.weixin.qq.com";
        /// <summary>
        ///  微信登陆地址
        /// </summary>
        public readonly static string LOGIN_URL =
            "https://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN";
        /// <summary>
        ///  微信登出地址
        /// </summary>
        public readonly static string LOGOUT_URL =
            "http://mp.weixin.qq.com/cgi-bin/logout?t=wxm-logout&lang=zh_CN&token=";
        /// <summary>
        ///  主页地址
        /// </summary>
        public readonly static string INDEX_URL =
            "http://mp.weixin.qq.com/cgi-bin/home?t=home/index&lang=zh_CN&token=";
        /// <summary>
        /// 微信账号信息
        /// </summary>
        public readonly static string ACCOUNT_INFO_URL =
            "http://mp.weixin.qq.com/cgi-bin/settingpage?t=setting/index&action=index&lang=zh_CN&token=";
        /// <summary>
        /// 开发者页面地址  
        /// </summary>
        public readonly static string DEV_URL =
            "https://mp.weixin.qq.com/advanced/advanced?action=dev&t=advanced/dev&lang=zh_CN&token=";

        /// <summary>
        /// 切换开发模式
        /// </summary>
        public readonly static string DEV_UPDATE_RUL =
            "https://mp.weixin.qq.com/misc/skeyform?form=advancedswitchform&lang=zh_CN&flag=1&type=2";
        /// <summary>
        /// 服务器配置提交地址
        /// </summary>
        public readonly static string DEV_SERVICE_URL =
            "https://mp.weixin.qq.com/advanced/callbackprofile?t=ajax-response&lang=zh_CN&token=";
        /// <summary>
        /// 获取ACCESS_TOKEN接口  
        /// </summary>
        public readonly static string ACCESS_TOKEN_URL =
            "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=%s&secret=%s";
        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        public readonly static string FANS_URL =
            "https://api.weixin.qq.com/cgi-bin/user/get?access_token=%s&next_openid=%s";
        /// <summary>
        /// 获取粉丝信息
        /// </summary>
        public readonly static string FANS_INFO_URL =
            "https://api.weixin.qq.com/cgi-bin/user/info?access_token=%s&openid=%s&lang=zh_CN";
        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        public readonly static string MENU_DELETE_URL =
            "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=%s";
        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        public readonly static string MENU_CREATE_URL =
            "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=%s";
    }
}
