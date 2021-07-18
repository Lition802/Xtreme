using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSR;
using Newtonsoft.Json;

namespace Xtreme
{
    class _HITOKOTO
    {
        public static void hitokoto(Setting cfg, MCCSAPI api)
        {
            api.addBeforeActListener(EventKey.onLoadName, x =>
            {
                var a = BaseEvent.getFrom(x) as LoadNameEvent;
                Task.Run(() =>
                {
                    Thread.Sleep(15000);
                    var wb = new WebClient();
                    var msg = Encoding.UTF8.GetString(wb.DownloadData("https://v1.hitokoto.cn/"));
                    var j = JsonConvert.DeserializeObject<YiYan>(msg);
                    string tellarw = "";// = "§3欢迎进入服务器，现在是" + DateTime.Now.ToString("yyyy-M-d H:m:s") + "\n§6" + j.hitokoto;
                    cfg.hitokoto.joinMessage.ForEach(s =>
                    {
                        s = s.Replace("%playername%", a.playername);
                        s = s.Replace("%time%", DateTime.Now.ToString());
                        s = s.Replace("%hitokoto%", j.hitokoto);
                        tellarw += s;
                    });
                    api.runcmd("tellraw \"" + a.playername + "\" {\"rawtext\":[{\"text\":\"" + tellarw + "\"}]}");
                });
                return true;
            });
        }
    }
}
