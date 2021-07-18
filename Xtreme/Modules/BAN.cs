using CSR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    class _BAN
    {
        public static void cloudban(Setting cfg, MCCSAPI api)
        {
            
            api.addBeforeActListener(EventKey.onLoadName, x =>
            {
                var a = BaseEvent.getFrom(x) as LoadNameEvent;
                Task.Run(() =>
                {
                    var wb = new WebClient();
                    var dt = JsonConvert.DeserializeObject<Bbe>(wb.DownloadString("http://api.blackbe.xyz/api/check?v2=true&id=" + a.playername));
                    if (dt.error == 2002)
                    {
                        api.disconnectClient(a.uuid, cfg.Ban.blackbe.messsage);
                        Console.WriteLine($"[Xtreme] 玩家{a.playername}在Blcakbe中有登记记录，已断开连接");
                    }
                    Console.WriteLine($"[Xtreme] 玩家{a.playername}的云端黑名单检查已完成");
                });
                return true;
            });
        }
        public static void localban(Setting cfg, MCCSAPI api)
        {
            string GetUUID(string p)
            {
                var j = JArray.Parse(api.getOnLinePlayers());
                foreach (var i in j)
                {
                    if (i["playername"].ToString() == p)
                        return i["uuid"].ToString();
                }
                Console.WriteLine("[Xtreme] 无法查询玩家" + p + "的UUID!");
                return null;
            }
            api.addBeforeActListener(EventKey.onLoadName, x =>
            {
                var a = BaseEvent.getFrom(x) as LoadNameEvent;
                if (cfg.Ban.localBan.hackers.Contains(a.playername))
                {
                    api.disconnectClient(GetUUID(a.playername), cfg.Ban.localBan.messsage);
                    Console.WriteLine($"[Xtreme] 玩家{a.playername}在本地黑名单中有登记记录，已断开连接");
                }
                Console.WriteLine($"[Xtreme] 玩家{a.playername}的本地黑名单检查已完成");
                return true;
            });
        }
    }
}
