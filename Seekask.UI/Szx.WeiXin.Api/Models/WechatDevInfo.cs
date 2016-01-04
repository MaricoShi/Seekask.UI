using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szx.WeiXin.Api.Models
{
    /// <summary>
    /// 
    /// 作者：石忠孝   
    /// 文件名：WechatDevInfo
    /// 创建时间：2015/1/1 11:56:27
    /// 修改人：石忠孝
    /// 修改时间：2015/1/1 11:56:27
    /// 说明：
    ///     开发 基本配置
    /// </summary>
    public class WechatDevInfo
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 应用秘钥
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 消息加解密密钥
        /// </summary>
        public string EncodingAESKey { get; set; }
        /// <summary>
        /// 消息加解密方式
        /// </summary>
        public EncodingAESType EncodingAESType { get; set; }
    }
}
