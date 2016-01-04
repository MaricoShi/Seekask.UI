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
    /// 文件名：WechatAccountInfo
    /// 创建时间：2015/1/1 11:53:57
    /// 修改人：石忠孝
    /// 修改时间：2015/1/1 11:53:57
    /// 说明：
    /// 
    /// </summary>
    public class WechatAccountInfo
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImage { get; set; }
        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode { get; set; }
        /// <summary>
        /// 名称 
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string WechatNumber { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public WechatType WechatType { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Introduces { get; set; }
        /// <summary>
        /// 认证情况
        /// </summary>
        public Authenticate Authenticate { get; set; }
        /// <summary>
        /// 所在地址
        /// </summary>
        public string PlaceAddress { get; set; }
        /// <summary>
        /// 主体信息
        /// </summary>
        public string SubjectInfo{ get; set; }
        /// <summary>
        /// 登录邮箱
        /// </summary>
        public string LoginEmail { get; set; }
        /// <summary>
        /// 原始ID
        /// </summary>
        public string AccountId { get; set; }
        
    }
}
