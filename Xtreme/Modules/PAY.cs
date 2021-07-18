using CSR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    class _PAY
    {
        public static void pay(Setting cfg,MCCSAPI api)
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
            api.setCommandDescribe("pay", "转账");
            var formid = new Dictionary<string, uint>();
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                if (a.cmd == "/pay")
                {
                    var gui = new GUIS.GUIBuilder(api, "转账面板");
                    gui.AddDropdown("选择一个转账目标", 0, Xtreme.onlines.ToArray());
                    gui.AddInput("请输入转账金额");
                    try
                    {
                        formid.Add(a.playername, gui.SendToPlayer(GetUUID(a.playername)));
                    }
                    catch
                    {
                        formid[a.playername] = gui.SendToPlayer(GetUUID(a.playername));
                    }
                    return false;
                }
                return true;
            });
            api.addBeforeActListener(EventKey.onFormSelect, x =>
            {
                var a = BaseEvent.getFrom(x) as FormSelectEvent;
                if (formid.ContainsKey(a.playername))
                {
                    if (formid[a.playername] == a.formid && a.selected != "null")
                    {
                        var j = JArray.Parse(a.selected);
                        string topl = Xtreme.onlines.ToArray()[int.Parse(j[0].ToString())];
                        int m;
                        try
                        {
                            m = int.Parse(j[1].ToString());
                        }
                        catch
                        {
                            api.sendText(a.uuid, "输入的数据类型有错误");
                            formid.Remove(a.playername);
                            return true;
                        }
                        if (api.getscoreboard(a.uuid, cfg.Pay.scoreboard) >= m)
                        {
                            api.setscoreboard(a.uuid, cfg.Pay.scoreboard, api.getscoreboard(a.uuid, cfg.Pay.scoreboard) - m);
                            api.setscoreboard(GetUUID(topl), cfg.Pay.scoreboard, api.getscoreboard(GetUUID(topl), cfg.Pay.scoreboard) + m);
                            api.sendText(a.uuid, cfg.Pay.message.moneyEnough.Replace("%playername%", topl).Replace("%money%", m.ToString()));
                        }
                        else
                        {
                            api.sendText(a.uuid, cfg.Pay.message.moneyInsufficient);
                        }
                        formid.Remove(a.playername);
                        return true;
                    }
                }
                return true;
            });
        }
    }
}
