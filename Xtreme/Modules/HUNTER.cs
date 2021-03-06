using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xtreme
{
    class _HUNTER
    {
        public static void hunter(Setting cfg,MCCSAPI api)
        {
            var helper = new Helper(api);
            Dictionary<string, int> ht = new Dictionary<string, int>();
            try
            {
                ht = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText("./plugins/Xtreme/hunter.json"));
            }
            catch
            {
                Console.WriteLine("[Error][Xtreme] 无法找到hunter.json");
            }
            api.addBeforeActListener(EventKey.onMobDie, x =>
            {

                var a = BaseEvent.getFrom(x) as MobDieEvent;
                if (a.playername == null)
                {
                    if (a.srctype == "entity.player.name")
                    {
                        //Console.WriteLine(new CsActor(api, a.mobPtr).TypeId);
                        if (ht.ContainsKey(a.mobtype))
                        {
                            api.setscoreboard(helper.GetUUID(a.srcname), cfg.hunter.scoreboard, api.getscoreboard(helper.GetUUID(a.srcname), cfg.hunter.scoreboard) + ht[a.mobtype]);
                            api.sendText(helper.GetUUID(a.srcname), cfg.hunter.message.Replace("%money%", ht[a.mobtype].ToString()));
                        }
                    }
                }
                return true;
            });
        }
    }
}
