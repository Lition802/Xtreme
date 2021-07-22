using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSR;
using Newtonsoft.Json.Linq;

namespace Xtreme
{
    
    class _TPA
    {
        public static void tpa(MCCSAPI api)
        {
            var formid = new Dictionary<string, uint>();
            var modle = new Dictionary<string, uint>();
            var tpm = new Dictionary<string, string>();
            var tpp = new Dictionary<string, string>();
            var helper = new Helper(api);
            void addvalue(int m, string k, object v)
            {
                switch (m)
                {
                    case 0:
                        if (formid.ContainsKey(k))
                        {
                            formid[k] = Convert.ToUInt32(v);
                        }
                        else
                        {
                            formid.Add(k, Convert.ToUInt32(v));
                        }
                        break;
                    case 1:
                        if (modle.ContainsKey(k))
                        {
                            modle[k] = Convert.ToUInt32(v);
                        }
                        else
                        {
                            modle.Add(k, Convert.ToUInt32(v));
                        }
                        break;
                    case 2:
                        if (tpp.ContainsKey(k))
                        {
                            tpp[k] = v.ToString();
                        }
                        else
                        {
                            tpp.Add(k, v.ToString());
                        }
                        break;
                    case 3:
                        if (tpm.ContainsKey(k))
                        {
                            tpm[k] = v.ToString();
                        }
                        else
                        {
                            tpm.Add(k, v.ToString());
                        }
                        break;
                }
            }
            api.setCommandDescribe("tpa", "传送面板");
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                if (a.cmd == "/tpa")
                {
                    var gui = new GUIS.GUIBuilder(api, "TPA");
                    gui.AddDropdown("选择要传送的玩家", 0, Xtreme.onlines.ToArray());
                    gui.AddDropdown("传送模式", 0, new string[] { "传送自己到玩家", "传送玩家到自己" });
                    addvalue(1, a.playername, 0);
                    addvalue(0, a.playername, gui.SendToPlayer(helper.GetUUID(a.playername)));
                    return false;
                }
                return true;
            });
            api.addBeforeActListener(EventKey.onFormSelect, x =>
            {
                var a = BaseEvent.getFrom(x) as FormSelectEvent;
                if (formid.ContainsKey(a.playername))
                {
                    if (a.formid == formid[a.playername] && a.selected != "null")
                    {
                        if (modle[a.playername] == 0)
                        {
                            var j = JArray.Parse(a.selected);
                            string topl = Xtreme.onlines.ToArray()[int.Parse(j[0].ToString())];
                            if (topl == a.playername)
                            {
                                api.sendText(helper.GetUUID(a.playername), "不允许传送自己！");
                                return true;
                            }
                            switch (int.Parse(j[1].ToString()))
                            {
                                case 0:
                                    api.sendText(helper.GetUUID(a.playername), "传送请求已发送");
                                    addvalue(1, topl, 1);
                                    addvalue(2, topl, a.playername);
                                    addvalue(3, topl, "to");
                                    api.sendText(helper.GetUUID(topl), "[TPA] " + a.playername + "想传送到你的位置");
                                    addvalue(0, topl, api.sendModalForm(helper.GetUUID(topl), "TPA", a.playername + "想传送到你的位置", "允许", "不允许"));
                                    break;
                                case 1:
                                    api.sendText(helper.GetUUID(a.playername), "传送请求已发送");
                                    addvalue(1, topl, 1);
                                    addvalue(2, topl, a.playername);
                                    addvalue(3, topl, "for");
                                    api.sendText(helper.GetUUID(topl), "[TPA] " + a.playername + "想要你传送到他的位置");
                                    addvalue(0, topl, api.sendModalForm(helper.GetUUID(topl), "TPA", a.playername + "想要你传送到他的位置", "允许", "不允许"));
                                    break;
                            }
                        }
                        else if (modle[a.playername] == 1)
                        {
                            var se = bool.Parse(a.selected);
                            if (se)
                            {
                                switch (tpm[a.playername])
                                {
                                    case "to":
                                        Xtreme.iffeedback = true;
                                        api.runcmd("tp \"" + tpp[a.playername] + "\" \"" + a.playername + "\"");
                                        break;
                                    case "for":
                                        Xtreme.iffeedback = true;
                                        api.runcmd("tp \"" + a.playername + "\" \"" + tpp[a.playername] + "\"");
                                        break;
                                }
                            }
                            else
                            {
                                api.sendText(helper.GetUUID(tpp[a.playername]), a.playername + "拒绝了你的传送请求");
                            }
                        }

                    }
                }
                return true;
            });
        }
    }
}
