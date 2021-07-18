using CSR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    class _DEATHPUNISH
    {
        public static void deathpunish(Setting cfg, MCCSAPI api)
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
            api.addBeforeActListener(EventKey.onMobDie, x =>
            {
                var a = BaseEvent.getFrom(x) as MobDieEvent;
                if (a.playername != null)
                {
                    if (api.getscoreboard(GetUUID(a.playername), cfg.DeathPunish.scoreboard) - cfg.DeathPunish.count > 0)
                    {
                        int bk = api.getscoreboard(GetUUID(a.playername), cfg.DeathPunish.scoreboard) - cfg.DeathPunish.count;
                        api.setscoreboard(GetUUID(a.playername), cfg.DeathPunish.scoreboard, bk);
                        api.sendText(GetUUID(a.playername), cfg.DeathPunish.message.moneyEnough.Replace("%money%", cfg.DeathPunish.count.ToString()));
                    }
                    else
                    {
                        api.sendText(GetUUID(a.playername), cfg.DeathPunish.message.moneyInsufficient);
                    }
                }
                return true;
            });
        }
    }
}
