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
    /// 文件名：Enums
    /// 创建时间：2015/1/1 11:49:15
    /// 修改人：石忠孝
    /// 修改时间：2015/1/1 11:49:15
    /// 说明：


    /// </summary>
    /// <summary>
    /// 请求类型
    /// </summary>
    public enum Action
    {
        post,
        get,
        socket
    }
    /// <summary>
    /// response数据格式
    /// </summary>
    public enum Format
    {
        json,
        xml
    }

    /// <summary>
    /// 传输数据格式，默认为url方式
    /// </summary>
    public enum TransType
    {
        text,
        json
    }

    /// <summary>
    /// 消息加解密方式
    /// </summary>
    public enum EncodingAESType
    {
        明文模式 = 0,
        兼容模式 = 1,
        安全模式 = 2
    }

    /// <summary>
    /// 微信号类型
    /// </summary>
    public enum WechatType
    {
        订阅号 = 0,
        服务号 = 1,
        企业号 = 2
    }

    /// <summary>
    /// 微信号认证情况
    /// </summary>
    public enum Authenticate
    {
        未认证 = 0,
        已认证 = 1
    }
}
