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
            var helper = new Helper(api);
            api.setCommandDescribe(cfg.balance.command, "查询余额");
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                if (a.cmd == "/" + cfg.balance.command)
                {
                    api.sendText(helper.GetUUID(a.playername), cfg.balance.message.Replace("%money%", api.getscoreboard(helper.GetUUID(a.playername), cfg.balance.scoreboard).ToString()));
                    return false;
                }
                return true;
            });
        }
    }
}
