using CSR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    class _BACK
    {
        public static void back(Setting cfg, MCCSAPI api)
        {
            var helper = new Helper(api);
            var dies = new Dictionary<string, Home>();
            void addvalue(string p, Home vec)
            {
                if (dies.ContainsKey(p))
                {
                    dies[p] = vec;
                }
                else
                {
                    dies.Add(p, vec);
                }
            }
            api.setCommandDescribe("back", "返回死亡点");
            api.addBeforeActListener(EventKey.onMobDie, x =>
            {
                var a = BaseEvent.getFrom(x) as MobDieEvent;
                if (a.playername != null)
                {
                    addvalue(a.playername, new Home()
                    {
                        x = a.XYZ.x,
                        y = a.XYZ.y,
                        z = a.XYZ.z,
                        dimid = a.dimensionid

                    });
                }
                return true;
            });
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                if (a.cmd == "/back")
                {
                    if (dies.ContainsKey(a.playername))
                    {
                        if (cfg.back.cost.enable)
                        {
                            if (api.getscoreboard(helper.GetUUID(a.playername), cfg.back.cost.scoreboard) >= cfg.back.cost.money)
                            {
                                api.setscoreboard(helper.GetUUID(a.playername), cfg.back.cost.scoreboard, api.getscoreboard(helper.GetUUID(a.playername), cfg.back.cost.scoreboard) - cfg.back.cost.money);
                                SymCall.teleport(api, a.playerPtr, dies[a.playername].x, dies[a.playername].y, dies[a.playername].z, dies[a.playername].dimid);
                                api.sendText(helper.GetUUID(a.playername), cfg.back.cost.message.moneyEnough.Replace("%money%", cfg.back.cost.money.ToString()));
                            }
                            else
                            {
                                api.sendText(helper.GetUUID(a.playername), cfg.back.cost.message.moneyInsufficient.Replace("%money%", cfg.back.cost.money.ToString()));
                            }
                        }
                        else
                        {
                            SymCall.teleport(api, a.playerPtr, dies[a.playername].x, dies[a.playername].y, dies[a.playername].z, dies[a.playername].dimid);
                            api.sendText(helper.GetUUID(a.playername), "[BACK] 成功返回上一死亡点!");
                        }
                    }
                    else
                    {
                        api.sendText(helper.GetUUID(a.playername), "[BACK] 找不到死亡点!");
                    }
                    return false;
                }
                return true;
            });
        }
    }
}
