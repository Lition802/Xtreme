using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSR;
using Newtonsoft.Json.Linq;

namespace Xtreme
{
    class _ECONADMIN
    {
        public static void econadmin(Setting cfg,MCCSAPI api)
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
            string GetXUID(string p)
            {
                foreach (JToken playerData in JArray.Parse(api.getOnLinePlayers()))
                {
                    if ($"{playerData["playername"]}" == p)
                    {
                        return $"{playerData["xuid"]}";
                    }
                }
                Console.WriteLine("[Xtreme] 无法查询玩家" + p + "的XUID!");
                return null;
            }
            api.setCommandDescribe("econadmin", "经济管理");
            var formid = new Dictionary<string, uint>();
            var modle = new Dictionary<string, string>();
            var chpl = new Dictionary<string, string>();
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
                            modle[k] = v.ToString();
                        }
                        else
                        {
                            modle.Add(k, v.ToString());
                        }
                        break;
                    case 2:
                        if (chpl.ContainsKey(k))
                        {
                            chpl[k] = v.ToString();
                        }
                        else
                        {
                            chpl.Add(k, v.ToString());
                        }
                        break;
                }
            }
            string GetJson()
            {
                string re = "[";
                foreach (var i in Xtreme.onlines)
                {
                    re += "\"" + i + "\n$" + api.getscoreboard(GetUUID(i), cfg.econadmin.scoreboard) + "\",";
                }
                return re.Substring(0, re.Length - 1) + "]";
            }
            bool ifop(string xuid)
            {
                foreach (JToken playerData in JArray.Parse(File.ReadAllText("permissions.json")))
                {
                    if ($"{playerData["xuid"]}" == xuid && $"{playerData["permission"]}" == "operator")
                    {
                        return true;
                    }
                }
                return false;
            }
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                if (a.cmd == "/econadmin")
                {
                    if (ifop(GetXUID(a.playername)))
                    {
                        addvalue(1, a.playername, "main");
                        addvalue(0, a.playername, api.sendSimpleForm(GetUUID(a.playername), "EconAdmin", "", GetJson()));
                        return false;
                    }
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
                        if (modle[a.playername] == "main")
                        {
                            int se = int.Parse(a.selected);
                            var gui = new GUIS.GUIBuilder(api, "EconAdmin");
                            gui.AddDropdown("选择管理模式", 0, new string[] { "添加", "扣除" });
                            gui.AddInput("请输入金额");
                            addvalue(2, a.playername, Xtreme.onlines.ToArray()[se]);
                            addvalue(1, a.playername, "edit");
                            addvalue(0, a.playername, gui.SendToPlayer(a.uuid));
                        }
                        else if (modle[a.playername] == "edit")
                        {
                            var se = JArray.Parse(a.selected);
                            int m = 0;
                            try
                            {
                                m = int.Parse(se[1].ToString());
                            }
                            catch
                            {
                                api.sendText(a.uuid, "输入的数据格式有误");
                                return true;
                            }
                            if (int.Parse(se[0].ToString()) == 0)
                            {
                                api.setscoreboard(GetUUID(chpl[a.playername]), cfg.econadmin.scoreboard, api.getscoreboard(GetUUID(chpl[a.playername]), cfg.econadmin.scoreboard) + m);
                                api.sendText(a.uuid, $"成功为{chpl[a.playername]}添加{m}到{cfg.econadmin.scoreboard}");
                            }
                            else
                            {
                                if (api.getscoreboard(GetUUID(chpl[a.playername]), cfg.econadmin.scoreboard) >= m)
                                {
                                    api.setscoreboard(GetUUID(chpl[a.playername]), cfg.econadmin.scoreboard, api.getscoreboard(GetUUID(chpl[a.playername]), cfg.econadmin.scoreboard) - m);
                                    api.sendText(a.uuid, $"成功为{chpl[a.playername]}扣除{m}到{cfg.econadmin.scoreboard}");
                                }
                                else
                                {
                                    api.sendText(a.uuid, chpl[a.playername] + "的余额不足");

                                }
                            }
                        }
                    }
                }
                return true;
            });
        }
    }
}
