using CSR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    class _BALANCE
    {
        public static void balance(Setting cfg,MCCSAPI api)
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
            api.setCommandDescribe(cfg.balance.command, "查询余额");
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                if (a.cmd == "/" + cfg.balance.command)
                {
                    api.sendText(GetUUID(a.playername), cfg.balance.message.Replace("%money%", api.getscoreboard(GetUUID(a.playername), cfg.balance.scoreboard).ToString()));
                    return false;
                }
                return true;
            });
        }
    }
}
