using CSR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    class _ITEMDB
    {
        public static void itemdb(Setting cfg,MCCSAPI api)
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
            api.setCommandDescribe("itemdb", "查看手中物品");
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                if (a.cmd == "/itemdb")
                {
                    var pl = new CsPlayer(api, a.playerPtr);
                    var h = JsonConvert.DeserializeObject<List<HandContainer>>(pl.HandContainer).ToArray();
                    string dbg = "物品id:" + h[0].id + "\n物品名称" + h[0].item;
                    api.sendText(GetUUID(a.playername), dbg);
                    return false;
                }
                return true;
            });
        }
    }
}
