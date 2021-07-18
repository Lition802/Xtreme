using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSR;
using Newtonsoft.Json.Linq;

namespace Xtreme
{
    class _CHAT
    {
        public static void chat(Setting cfg,MCCSAPI api)
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
            api.addBeforeActListener(EventKey.onInputText, x =>
            {
                var a = BaseEvent.getFrom(x) as InputTextEvent;
                if (a.msg.Length > cfg.Chat.MaxLength)
                {
                    return false;
                }
                string msg = a.msg;
                bool ifre = false;
                foreach (var i in cfg.Chat.dirty)
                {
                    if (msg.IndexOf(i.Key) != -1)
                    {
                        switch (i.Value)
                        {
                            case 0:
                                msg = msg.Replace(i.Key, "*");
                                ifre = true;
                                break;
                            case 1:
                                return false;
                            case 2:
                                api.sendText(GetUUID(a.playername), cfg.Chat.message);
                                return false;
                        }
                    }
                }
                if (ifre)
                {
                    api.talkAs(GetUUID(a.playername), msg);
                    return false;
                }
                return true;
            });
        }
    }
}
