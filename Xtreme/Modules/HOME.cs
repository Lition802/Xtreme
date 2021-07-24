using CSR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    class _HOME
    {
        public static Dictionary<string, Dictionary<string, Home>> homes;
        public static List<string> getHomeList(string p)
            {
            var rt = new List<string>();
            if (homes.ContainsKey(p))
            {
                foreach (var k in homes[p])
                {
                    rt.Add(k.Key);
                }
            }
            else
            {
                homes.Add(p, new Dictionary<string, Home>());
            }
            return rt;
        }

        public static void home(Setting cfg, MCCSAPI api)
        {
            var helper = new Helper(api);
            api.setCommandDescribe("home", "家园面板");
            if (!File.Exists("./plugins/Xtreme/home.json"))
            {
                File.WriteAllText("./plugins/Xtreme/home.json", "{}");
            }
            homes = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Home>>>(File.ReadAllText("./plugins/Xtreme/home.json"));
            var formid = new Dictionary<string, uint>();
            var modle = new Dictionary<string, string>();
            void addvalue(int m, string k, object v)
            {
                //Console.WriteLine($"添加{m},{k},{v}");
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
                }
            }
            
            void save()
            {
                File.WriteAllText("./plugins/Xtreme/home.json", JObject.Parse(JsonConvert.SerializeObject(homes)).ToString());
            }
            string GetHome(string p, int ind)
            {
                var i = 0;
                foreach (var h in homes[p])
                {
                    if (i == ind)
                    {
                        return h.Key;
                    }
                    i++;
                }
                return null;
            }
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                if (a.cmd == "/home")
                {
                    addvalue(1, a.playername, "main");
                    addvalue(0, a.playername, api.sendSimpleForm(helper.GetUUID(a.playername), "HOME", "请选择选项", "[\"添加HOME\",\"删除HOME\",\"传送到HOME\"]"));
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
                        switch (modle[a.playername])
                        {
                            case "main":
                                var se = int.Parse(a.selected);
                                switch (se)
                                {
                                    case 0:
                                        getHomeList(a.playername);
                                        if (homes[a.playername].Count == cfg.home.max)
                                        {
                                            api.sendText(helper.GetUUID(a.playername), "[HOME] 数量达到上限");
                                            return true;
                                        }
                                        addvalue(1, a.playername, "add");
                                        var gui = new GUIS.GUIBuilder(api, "添加HOME");
                                        gui.AddInput("给家取个名字");
                                        addvalue(0, a.playername, gui.SendToPlayer(a.uuid));
                                        break;
                                    case 1:
                                        addvalue(1, a.playername, "del");
                                        addvalue(0, a.playername, api.sendSimpleForm(a.uuid, "删除HOME", "选择要删除的HOME", JsonConvert.SerializeObject(getHomeList(a.playername))));
                                        break;
                                    case 2:
                                        addvalue(1, a.playername, "tp");
                                        addvalue(0, a.playername, api.sendSimpleForm(a.uuid, "传送到HOME", "选择要传送的HOME", JsonConvert.SerializeObject(getHomeList(a.playername))));
                                        break;
                                }
                                break;
                            case "add":
                                var ss = JArray.Parse(a.selected);
                                if (string.IsNullOrEmpty(ss[0].ToString()))
                                {
                                    api.sendText(a.uuid, "[HOME] 格式有误！");
                                    return true;
                                }
                                else
                                {
                                    if (homes[a.playername].ContainsKey(ss[0].ToString()))
                                    {
                                        api.sendText(a.uuid, "[HOME] 名称重复！");
                                        return true;
                                    }
                                    homes[a.playername].Add(ss[0].ToString(), new Home()
                                    {
                                        x = a.XYZ.x,
                                        y = a.XYZ.y,
                                        z = a.XYZ.z,
                                        dimid = a.dimensionid
                                    });
                                    save();
                                    api.sendText(a.uuid, $"[HOME] {ss[0]}添加成功！");
                                }
                                break;
                            case "del":
                                var ses = int.Parse(a.selected);
                                var hom = GetHome(a.playername, ses);
                                homes[a.playername].Remove(hom);
                                api.sendText(helper.GetUUID(a.playername), "[HOME] 已删除：" + hom);
                                save();
                                break;
                            case "tp":
                                var sss = int.Parse(a.selected);
                                var ho = GetHome(a.playername, sss);
                                SymCall.teleport(api, a.playerPtr, homes[a.playername][ho].x, homes[a.playername][ho].y, homes[a.playername][ho].z, homes[a.playername][ho].dimid);
                                api.sendText(helper.GetUUID(a.playername), "[HOME] 你已传送到：" + ho);
                                break;
                        }
                    }
                }
                return true;
            });
        }
    }
}
