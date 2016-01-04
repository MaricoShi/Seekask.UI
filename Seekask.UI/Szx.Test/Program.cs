using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szx.WeiXin.Api;
using Szx.WeiXin.Api.Domain;
using Szx.WeiXin.Api.Models;

using System.Threading;
using System.IO;

namespace Szx.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SmartBindUtil smartBind = new SmartBindUtil("784478682@qq.com", "shizhongxiao93"))
            {
                //登录成功后，在开发者中心获取开发者信息
                WechatDevInfo devInfo = smartBind.GetWechatDev();

                WechatAccountInfo accountInfo = smartBind.GetAccount();
                //无法获取用户信息，结束操作并跳出
                if (accountInfo == null) return;

                //下载头像
                if (!string.IsNullOrWhiteSpace(accountInfo.HeadImage))
                {
                    string fileName = AppDomain.CurrentDomain.BaseDirectory + "img\\headImg.jpg";
                    int downStatus = smartBind.DownloadImg(accountInfo.HeadImage, fileName);
                }
                //下载二维码
                if (!string.IsNullOrWhiteSpace(accountInfo.HeadImage))
                {
                    string fileName = AppDomain.CurrentDomain.BaseDirectory + "img\\qrCode.jpg";
                    int downStatus = smartBind.DownloadImg(accountInfo.QRCode, fileName);
                }

                //设置启用开发模式
                int status = smartBind.EnabledDev(1, 2);
                //启用开发模式失败，结束操作并跳出
                if (status != 0) return;

                // 验证服务器接口回调，此处修改服务器配置中的URL和Token
                int i = 2;
                while (i > 0)
                {
                    status = smartBind.SetDevServiceUrl(
                        "http://omsvip.sinaapp.com/coreServlet",
                        "SeekAsk2015WeiXin", "T9538lolqhzjEQokpni6xSfYQ0LpJChwLvJiohLu4oV",
                        ((int)EncodingAESType.安全模式).ToString());
                    if (status == 0) break;

                    Thread.Sleep(2000);
                    i--;
                }
                if (status == 0) { 
                    //修改成功！
                }

            };
        }
    }
}
