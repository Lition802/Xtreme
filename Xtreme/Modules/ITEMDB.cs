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
            var helper = new Helper(api);
            api.setCommandDescribe("itemdb", "查看手中物品");
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                if (a.cmd == "/itemdb")
                {
                    var pl = new CsPlayer(api, a.playerPtr);
                    var h = JsonConvert.DeserializeObject<List<HandContainer>>(pl.HandContainer).ToArray();
                    string dbg = "物品id:" + h[0].id + "\n物品名称" + h[0].item;
                    api.sendText(helper.GetUUID(a.playername), dbg);
                    return false;
                }
                return true;
            });
        }
    }
}
