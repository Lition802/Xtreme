using CSR;
using LMenuConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Xtreme
{
    class _MENU
    {
        public static void menu(Setting cfg, MCCSAPI api)
        {
            if (File.Exists("./plugins/Xtreme/menu/main.yaml") == false)
            {
                Directory.CreateDirectory("./plugins/Xtreme/menu/");
                var s = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
                var y = s.Serialize(new LMenuConfig.config()
                {
                    content = "GUI菜单",
                    title = "GUI",
                    type = "form",
                    buttons = new List<ButtonsItem>()
                        {
                            new ButtonsItem()
                            {
                                image = new Image()
                                {
                                    data = "textures/items/apple",
                                    type = "path"
                                },
                                images = true,
                                command = "me hello",
                                text = "说hello",
                                type = "default"
                            }
                        }
                });
                File.WriteAllText("./plugins/Xtreme/menu/main.yaml", y);
            }
            Dictionary<string, uint> formid = new Dictionary<string, uint>();
            Dictionary<string, bool> ifuse = new Dictionary<string, bool>();
            Dictionary<string, string> page = new Dictionary<string, string>();
            string convertJson(string jsontext)
            {
                var j = JsonConvert.DeserializeObject<LMenuConfig.config>(jsontext);
                var butt = new List<LMenu.ButtonsItem>();
                var menu = new LMenu.CustomsMenu()
                {
                    content = j.content,
                    title = j.title,
                    type = j.type ?? "form"
                };
                foreach (LMenuConfig.ButtonsItem item in j.buttons)
                {
                    var img = new LMenu.Image()
                    {
                        type = item.image.type,
                        data = item.image.data
                    };
                    if (!item.images)
                    {
                        img = null;
                    }
                    var tmp = new LMenu.ButtonsItem()
                    {
                        text = item.text,
                        image = img
                    };
                    butt.Add(tmp);
                }
                menu.buttons = butt;
                return JsonConvert.SerializeObject(menu);
            }
            string getMneuChose(LMenuConfig.config menu, int index, int mod)
            {
                int a = 0;
                foreach (LMenuConfig.ButtonsItem i in menu.buttons)
                {
                    if (a == index)
                    {
                        switch (mod)
                        {
                            case 0:
                                return i.type;
                            case 1:
                                return i.command;
                        }
                    }
                    a += 1;
                }
                return null;
            }
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
            bool IfOP(string xuid)
            {
                var jo = JArray.Parse(File.ReadAllText("permissions.json"));
                foreach (var i in jo)
                {
                    if (i["xuid"].ToString() == xuid)
                    {
                        if (i["permission"].ToString() == "operator")
                        {
                            return true;
                        }
                    }

                }
                return false;
            }
            string GetXUID(string p)
            {
                var jo = JArray.Parse(api.getOnLinePlayers());
                foreach (var i in jo)
                {
                    if (i["playername"].ToString() == p)
                        return i["xuid"].ToString();
                }
                Console.WriteLine($"[Xtreme] 无法找到玩家{p}的XUID!");
                return null;

            }
            string YamlToJson(string yaml)
            {
                try
                {
                    var r = new StringReader(yaml);
                    var d = new DeserializerBuilder().Build();
                    var y = d.Deserialize(r);
                    var s = new SerializerBuilder()
                        .JsonCompatible()
                        .Build();
                    var js = s.Serialize(y);
                    return js;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return null;
            }
            void sendMenu(string name, string p)
            {
                string m = "./plugins/Xtreme/menu/" + p + ".yaml";
                var TMP = api.sendCustomForm(GetUUID(name), convertJson(YamlToJson(File.ReadAllText(m))));
                formid[name] = TMP;
            }
            api.addBeforeActListener(EventKey.onLoadName, x =>
            {
                var a = BaseEvent.getFrom(x) as LoadNameEvent;
                ifuse[a.playername] = true;
                formid[a.playername] = 0;
                page[a.playername] = "main";
                return true;
            });
            api.addBeforeActListener(EventKey.onPlayerLeft, x =>
            {
                var a = BaseEvent.getFrom(x) as PlayerLeftEvent;
                ifuse.Remove(a.playername);
                formid.Remove(a.playername);
                page.Remove(a.playername);
                return true;
            });
            api.addBeforeActListener(EventKey.onUseItem, x =>
            {
                var a = BaseEvent.getFrom(x) as UseItemEvent;
                try
                {
                    if (ifuse[a.playername] && a.itemid == cfg.menu.itemid)
                    {
                        ifuse[a.playername] = false;
                        page[a.playername] = "main";
                        sendMenu(a.playername, page[a.playername]);
                        Task.Run(() =>
                        {
                            Thread.Sleep(1000);
                            ifuse[a.playername] = true;
                        });
                        return false;
                    }
                }
                catch (Exception r)
                {
                    Console.WriteLine(r.ToString());
                }

                return true;
            });
            api.addBeforeActListener(EventKey.onFormSelect, x =>
            {
                var a = BaseEvent.getFrom(x) as FormSelectEvent;
                if (a.formid == formid[a.playername] && a.selected != "null")
                {
                    LMenuConfig.config m = JsonConvert.DeserializeObject<LMenuConfig.config>(YamlToJson(File.ReadAllText("./plugins/Xtreme/menu/" + page[a.playername] + ".yaml")));
                    string cmd = getMneuChose(m, Convert.ToInt32(a.selected), 1);
                    switch (getMneuChose(m, Convert.ToInt32(a.selected), 0))
                    {
                        case "default":
                            api.runcmdAs(a.uuid, cmd.Replace("@s", "\"" + a.playername + "\""));
                            break;
                        case "cmd":
                            api.runcmd(cmd.Replace("@s", "\"" + a.playername + "\""));
                            break;
                        case "form":
                            page[a.playername] = cmd;
                            if (File.Exists("./plugins/Xtreme/menu/" + cmd + ".yaml") == false)
                            {
                                api.sendModalForm(a.uuid, "MENU error", "无法找到文件：" + cmd + ".yaml！！\n请检查！！", "OK", "KO");
                                break;
                            }
                            sendMenu(a.playername, cmd);
                            break;
                        case "op":
                            if (IfOP(GetXUID(a.playername)))
                            {
                                api.runcmd(cmd.Replace("@s", "\"" + a.playername + "\""));
                            }
                            else
                            {
                                api.sendText(a.uuid, "[MENU] 你没有权限！");
                            }
                            break;
                        case "opform":
                            page[a.playername] = cmd;
                            if (File.Exists("./plugins/Xtreme/menu/" + cmd + ".yaml") == false)
                            {
                                api.sendModalForm(a.uuid, "MENU error", "无法找到文件：" + cmd + ".yaml！！\n请检查！！", "OK", "KO");
                                break;
                            }
                            if (IfOP(GetXUID(a.playername)))
                            {
                                sendMenu(a.playername, cmd);
                            }
                            else
                            {
                                api.sendText(a.uuid, "[MENU] 你没有权限！");
                            }
                            break;
                        default:
                            api.sendModalForm(a.uuid, "MENU error", "未定义的tpye！！\n请检查json！！", "OK", "KO");
                            break;
                    }
                }
                return true;
            });
        }
    }
}
