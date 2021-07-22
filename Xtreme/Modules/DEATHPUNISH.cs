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
            var helper = new Helper(api);
            api.addBeforeActListener(EventKey.onMobDie, x =>
            {
                var a = BaseEvent.getFrom(x) as MobDieEvent;
                if (a.playername != null)
                {
                    if (api.getscoreboard(helper.GetUUID(a.playername), cfg.DeathPunish.scoreboard) - cfg.DeathPunish.count > 0)
                    {
                        int bk = api.getscoreboard(helper.GetUUID(a.playername), cfg.DeathPunish.scoreboard) - cfg.DeathPunish.count;
                        api.setscoreboard(helper.GetUUID(a.playername), cfg.DeathPunish.scoreboard, bk);
                        api.sendText(helper.GetUUID(a.playername), cfg.DeathPunish.message.moneyEnough.Replace("%money%", cfg.DeathPunish.count.ToString()));
                    }
                    else
                    {
                        api.sendText(helper.GetUUID(a.playername), cfg.DeathPunish.message.moneyInsufficient);
                    }
                }
                return true;
            });
        }
    }
}
