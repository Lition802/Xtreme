using CSR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using LMenuConfig;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace Xtreme
{
    public class Xtreme
    {
        public static List<string> onlines = new List<string>();
        public static bool iffeedback = false;
        public static string version = "1.6.5";
        /// <param name="intPtr">玩家指针</param>
		/// <param name="x">X坐标</param>
		/// <param name="y">Y坐标</param>
		/// <param name="z">Z坐标</param>
		/// <param name="d">维度ID</param>
        delegate void TELEPORT(MCCSAPI mapi, IntPtr intPtr, float x, float y, float z, int d);
        static TELEPORT cs_teleport = (api, ptr, x, y, z, d) =>
        {
            SymCall.teleport(api, ptr, x, y, z, d);
        };
    public static void Maximum(MCCSAPI api)
        {
            Directory.CreateDirectory("./plugins/Xtreme");
            if (File.Exists("./plugins/Xtreme/offlineMoney.yaml") == false)
            {
                var s = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
                string yaml = s.Serialize(new List<offlineMoney>()
                {
                    new offlineMoney()
                    {
                        name = "test",
                        score = 0
                    }
                });
                File.WriteAllText("./plugins/Xtreme/offlineMoney.yaml", yaml);
            }
            if (File.Exists("./plugins/Xtreme/config.yaml") == false)
            {
                var s = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
                var yaml = s.Serialize(new Setting()
                {
                    version = version,
                    @unsafe = new Unsafe()
                    {
                        enable = false,
                        ptr1 = 0,
                        ptr2 = 0,
                        ptr3 = 0
                    },
                    AutoClear = new AutoClear()
                    {
                        items = new Items()
                        {
                            enable = true,
                            message = new Clear_Message()
                            {
                                done = "掉落物品清理完成！",
                                waiting = "还有%time%秒就要清理掉落物品了！！"
                            },
                            timeout = 60
                        },
                        mobs = new Mobs()
                        {
                            enable = false,
                            message = new Clear_Message()
                            {
                                done = "生物清理完成！",
                                waiting = "还有%time%秒就要清理生物了！！"
                            },
                            keeps = new List<string>() { "cow", "sheep" },
                            timeout = 60
                        }
                    },
                    DeathPunish = new DeathPunish()
                    {
                        enable = false,
                        scoreboard = "money",
                        count = 100,
                        message = new Message()
                        {
                            moneyEnough = "您已死亡，扣除%money%金币",
                            moneyInsufficient = "您的余额不足，本次死亡惩罚已取消"
                        }
                    },
                    Timer = new Timer()
                    {
                        enable = false,
                        cmds = new Dictionary<string, int>()
                        {
                            {"say 114514",30 }
                        }
                    },
                    offlineMoney = new OMoney()
                    {
                        scoreboard = "money"
                    },
                    Chat = new Chat()
                    {
                        enable = true,
                        MaxLength = 20,
                        dirty = new Dictionary<string, int>()
                        {
                            {"草",0 },
                            {"nmsl",1 },
                            {"操",2 }
                        },
                        message = "不要说脏话哦"
                    },
                    Itemdb = new Itemdb()
                    {
                        enable = true
                    },
                    Ban = new Ban()
                    {
                        blackbe = new Blackbe()
                        {
                            enable = true,
                            messsage = "对不起，您在云端黑名单中，不能在此服务器游玩"
                        },
                        localBan = new LocalBan()
                        {
                            enable = true,
                            messsage = "对不起，您在服务器黑名单中，不能在此服务器游玩",
                            hackers = new List<string>()
                            {
                                "steve"
                            }
                        }
                    },
                    Pay = new Pay()
                    {
                        enable = true,
                        rate = 5,
                        scoreboard = "money",
                        message = new Message()
                        {
                            moneyEnough = "成功向%playername%转账%money%金币!",
                            moneyInsufficient = "您的余额不足!"
                        }
                    },
                    hitokoto = new Hitokoto()
                    {
                        enable = true,
                        joinMessage = new List<string>()
                        {
                            "欢迎加入服务器,%playername%\n",
                            "现在是%time%\n",
                            "%hitokoto%\n"
                        }
                    },
                    menu = new Menu()
                    {
                        enable = true,
                        itemid = 391
                    },
                    balance = new Balance()
                    {
                        enable = true,
                        scoreboard = "money",
                        command = "mymoney",
                        message = "您的余额为：%money%"
                    },
                    sign = new Sign()
                    {
                        enable = true,
                        message = new sign_Message()
                        {
                            already = "你今天已经签到过了哦！",
                            done = "签到成功！",
                            wait = "你今天还没有签到哦，用/sign来签到吧"
                        },
                        prize = new List<items>()
                        {
                            new items()
                            {
                                itemid = 1,
                                itemaux = 0,
                                count = 1
                            }
                        }
                    },
                    hunter = new Hunter()
                    {
                        enable = false,
                        message = "击杀奖励%money%金币！",
                        scoreboard = "money"
                    },
                    tpa = new TPA()
                    {
                        enable = true
                    },
                    home = new PFET_Home()
                    {
                        enable = true,
                        max = 5
                    },
                    back = new Back()
                    {
                        enable = true,
                        cost = new Cost()
                        {
                            enable = false,
                            money = 100,
                            scoreboard = "money",
                            message = new Message()
                            {
                                moneyEnough = "[BACK] 成功返回死亡点!花费%money%金币",
                                moneyInsufficient = "[BACK] 无法返回死亡点！您至少需要%money%金币"
                            }
                        }
                    },
                    tpp = new Tpp()
                    {
                        enable = true
                    },
                    econadmin = new Econadmin()
                    {
                        enable = true,
                        scoreboard = "money"
                    },
                    hideSeed = new HideSeed()
                    {
                        enable = true,
                        seed = 114514
                    }
                }) ;
                File.WriteAllText("./plugins/Xtreme/config.yaml", yaml);
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
            
            var deserializer = new DeserializerBuilder().Build();
            var yamlObject = deserializer.Deserialize(new StringReader(File.ReadAllText("./plugins/Xtreme/config.yaml")));
            var serializer = new SerializerBuilder()
                .JsonCompatible()
                .Build();
            var json = serializer.Serialize(yamlObject);
            var cfg = JsonConvert.DeserializeObject<Setting>(json);
            var omoney = JsonConvert.DeserializeObject<List<offlineMoney>>(serializer.Serialize(deserializer.Deserialize(new StringReader(File.ReadAllText("./plugins/Xtreme/offlineMoney.yaml")))));
            if (cfg.@unsafe.enable)
            {
                void addvalue(Dictionary<string,int> dic,int i)
                {
                    if (dic.ContainsKey(api.VERSION))
                    {
                        dic[api.VERSION] = i;
                    }
                    else
                    {
                        dic.Add(api.VERSION, i);
                    }
                    addvalue(SymCall.ptr1, cfg.@unsafe.ptr1);
                    addvalue(SymCall.ptr2, cfg.@unsafe.ptr2);
                    addvalue(SymCall.ptr3, cfg.@unsafe.ptr3);
                    Console.WriteLine("[WARN][Xtreme] 您正在使用unsafe模式，数据错误可能会造成服务器崩溃");
                }
            }
            offlineMoney GetOmoney(string name)
            {
                foreach (var i in omoney)
                {
                    if (i.name == name)
                    {
                        return i;
                    }
                }
                return null;
            }
            IntPtr getPtr(string p)
            {
                if(GetUUID(p) != null)
                {
                    var data = api.selectPlayer(GetUUID(p));
                    //Console.WriteLine(data);
                    if (!string.IsNullOrEmpty(data))
                    {
                        var pinfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                        if (pinfo != null)
                        {
                            object pptr;
                            if (pinfo.TryGetValue("playerptr", out pptr))
                            {
                                var ptr = (IntPtr)Convert.ToInt64(pptr);
                                return ptr;
                            }
                        }
                    }
                }               
                return IntPtr.Zero;
            }
            if (cfg.version != version)
            {
                Console.WriteLine("[Xtreme] 您的配置文件版本过旧，请备份后重新生成");
                Console.WriteLine("[Xtreme] 为安全考虑，已放弃加载");
                return;
            }
            _AUTOCLEAR.clear(cfg, api);
            if (cfg.Timer.enable)
            {
                Console.WriteLine("[Xtreme] 成功加载"+cfg.Timer.cmds.Keys.Count+"条定时任务");
                _TIMER.timer(cfg, api);
            }  
            if (cfg.DeathPunish.enable)
            {
                Console.WriteLine("[Xtreme] 死亡惩罚组件开启成功");
                api.addBeforeActListener(EventKey.onMobDie, x =>
                {
                    var a = BaseEvent.getFrom(x) as MobDieEvent;
                    if (a.playername != null)
                    {
                        if (api.getscoreboard(GetUUID(a.playername), cfg.DeathPunish.scoreboard) - cfg.DeathPunish.count > 0)
                        {
                            int bk = api.getscoreboard(GetUUID(a.playername), cfg.DeathPunish.scoreboard) - cfg.DeathPunish.count;
                            api.setscoreboard(GetUUID(a.playername), cfg.DeathPunish.scoreboard, bk);
                            api.sendText(GetUUID(a.playername), cfg.DeathPunish.message.moneyEnough.Replace("%money%", cfg.DeathPunish.count.ToString()));
                        }
                        else
                        {
                            api.sendText(GetUUID(a.playername), cfg.DeathPunish.message.moneyInsufficient);
                        }
                    }
                    return true;
                });
            }
            api.addBeforeActListener(EventKey.onLoadName, x =>
            {
                var a = BaseEvent.getFrom(x) as LoadNameEvent;
                if (onlines.Contains(a.playername))
                {
                    api.disconnectClient(a.uuid,"您的另一个账号在此服务器游玩");
                    return true;
                }
                onlines.Add(a.playername);
                bool ifat = false;
                try
                {
                    foreach (var i in omoney)
                    {
                        if (i.name == a.playername)
                        {
                            ifat = true;
                            break;
                        }
                    }
                    if (!ifat)
                    {
                        omoney.Add(new offlineMoney()
                        {
                            score = api.getscoreboard(GetUUID(a.playername), cfg.offlineMoney.scoreboard),
                            name = a.playername
                        });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return true;
            });
            api.addBeforeActListener(EventKey.onPlayerLeft, x =>
            {
                var a = BaseEvent.getFrom(x) as PlayerLeftEvent;
                try
                {
                    onlines.Remove(a.playername);
                }
                catch { }
                GetOmoney(a.playername).score = api.getscoreboard(GetUUID(a.playername), cfg.offlineMoney.scoreboard);
                var s = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
                File.WriteAllText("./plugins/Xtreme/offlineMoney.yaml", s.Serialize(omoney));
                return true;
            });
            api.addBeforeActListener(EventKey.onServerCmdOutput, x =>
            {
                var a = BaseEvent.getFrom(x) as ServerCmdOutputEvent;
                if (iffeedback)
                {
                    iffeedback = false;
                    File.AppendAllText("./plugins/Xtreme/cmd.log", "[" + DateTime.Now.ToString() + "] " + a.output);
                    return iffeedback;
                }
                return true;
            });
            if (cfg.Chat.enable)
            {
                Console.WriteLine("[Xtreme] 聊天管理组件加载成功");
                Console.WriteLine("[Xtreme] 已载入本地敏感词库" + cfg.Chat.dirty.Keys.Count + "个");
                _CHAT.chat(cfg, api);
            }
            if (cfg.Itemdb.enable)
            {
                Console.WriteLine("[Xtreme] 物品调试组件开启成功");
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
            if (cfg.Ban.blackbe.enable)
            {
                Console.WriteLine("[Xtreme] 云黑组件开启成功");
                api.addBeforeActListener(EventKey.onLoadName, x =>
                {
                    var a = BaseEvent.getFrom(x) as LoadNameEvent;
                    Task.Run(() =>
                    {
                        var wb = new WebClient();
                        var dt = JsonConvert.DeserializeObject<Bbe>(wb.DownloadString("http://api.blackbe.xyz/api/check?v2=true&id=" + a.playername));
                        if (dt.error == 2002)
                        {
                            api.disconnectClient(a.uuid, cfg.Ban.blackbe.messsage);
                            Console.WriteLine($"[Xtreme] 玩家{a.playername}在Blcakbe中有登记记录，已断开连接");
                        }
                        Console.WriteLine($"[Xtreme] 玩家{a.playername}的云端黑名单检查已完成");
                    });
                    return true;
                });
            }
            if (cfg.Ban.localBan.enable)
            {
                Console.WriteLine("[Xtreme] 本地黑名单组件开启成功");
                api.addBeforeActListener(EventKey.onLoadName, x =>
                {
                    var a = BaseEvent.getFrom(x) as LoadNameEvent;
                    if (cfg.Ban.localBan.hackers.Contains(a.playername))
                    {
                        api.disconnectClient(GetUUID(a.playername), cfg.Ban.localBan.messsage);
                        Console.WriteLine($"[Xtreme] 玩家{a.playername}在本地黑名单中有登记记录，已断开连接");
                    }
                    Console.WriteLine($"[Xtreme] 玩家{a.playername}的本地黑名单检查已完成");
                    return true;
                });
            }
            if (cfg.Pay.enable)
            {
                Console.WriteLine("[Xtreme] 转账组件开启成功");
                api.setCommandDescribe("pay", "转账");
                var formid = new Dictionary<string, uint>();
                api.addBeforeActListener(EventKey.onInputCommand, x =>
                {
                    var a = BaseEvent.getFrom(x) as InputCommandEvent;
                    if(a.cmd == "/pay")
                    {
                        var gui = new GUIS.GUIBuilder(api, "转账面板");
                        gui.AddDropdown("选择一个转账目标", 0, onlines.ToArray());
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
                        if(formid[a.playername] == a.formid && a.selected != "null")
                        {
                            var j = JArray.Parse(a.selected);
                            string topl = onlines.ToArray()[int.Parse(j[0].ToString())];
                            int m;
                            try
                            {
                                m = int.Parse(j[1].ToString());
                            }
                            catch
                            {
                                api.sendText(a.uuid,"输入的数据类型有错误");
                                formid.Remove(a.playername);
                                return true;
                            }
                            if(api.getscoreboard(a.uuid,cfg.Pay.scoreboard) >= m)
                            {
                                api.setscoreboard(a.uuid, cfg.Pay.scoreboard, api.getscoreboard(a.uuid, cfg.Pay.scoreboard) - m);
                                api.setscoreboard(GetUUID(topl), cfg.Pay.scoreboard, api.getscoreboard(GetUUID(topl), cfg.Pay.scoreboard) + m);
                                api.sendText(a.uuid, cfg.Pay.message.moneyEnough.Replace("%playername%",topl).Replace("%money%",m.ToString()));
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
            if (cfg.hitokoto.enable)
            {
                api.addBeforeActListener(EventKey.onLoadName, x =>
                {
                    var a = BaseEvent.getFrom(x) as LoadNameEvent;
                    Task.Run(() =>
                    {
                        Thread.Sleep(15000);
                        var wb = new WebClient();
                        var msg = Encoding.UTF8.GetString(wb.DownloadData("https://v1.hitokoto.cn/"));
                        var j = JsonConvert.DeserializeObject<YiYan>(msg);
                        string tellarw = "";// = "§3欢迎进入服务器，现在是" + DateTime.Now.ToString("yyyy-M-d H:m:s") + "\n§6" + j.hitokoto;
                        cfg.hitokoto.joinMessage.ForEach(s =>
                        {
                            s = s.Replace("%playername%", a.playername);
                            s = s.Replace("%time%", DateTime.Now.ToString());
                            s = s.Replace("%hitokoto%", j.hitokoto);
                            tellarw += s;
                        });
                        api.runcmd("tellraw \"" + a.playername + "\" {\"rawtext\":[{\"text\":\"" + tellarw + "\"}]}");
                    });
                    return true;
                });
            }
            if (cfg.menu.enable)
            {
                Console.WriteLine("[Xtreme] 菜单组件开启成功");
                if(File.Exists("./plugins/Xtreme/menu/main.yaml") == false)
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
                    }) ;
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
                string Getxuid(string p)
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
                    catch(Exception r)
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
                        LMenuConfig.config m = JsonConvert.DeserializeObject<LMenuConfig.config>(YamlToJson(File.ReadAllText("./plugins/Xtreme/menu/"+page[a.playername]+".yaml")));
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
                                if(File.Exists("./plugins/Xtreme/menu/"+cmd+".yaml") == false)
                                {
                                    api.sendModalForm(a.uuid, "MENU error", "无法找到文件："+cmd+".yaml！！\n请检查！！", "OK", "KO");
                                    break;
                                }
                                sendMenu(a.playername, cmd);
                                break;
                            case "op":
                                if (IfOP(Getxuid(a.playername)))
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
                                if (IfOP(Getxuid(a.playername)))
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
            if (cfg.balance.enable)
            {
                Console.WriteLine("[Xtreme] 余额查询组件开启成功");
                api.setCommandDescribe(cfg.balance.command, "查询余额");
                api.addBeforeActListener(EventKey.onInputCommand, x =>
                {
                    var a = BaseEvent.getFrom(x) as InputCommandEvent;
                    if(a.cmd == "/"+cfg.balance.command)
                    {
                        api.sendText(GetUUID(a.playername), cfg.balance.message.Replace("%money%", api.getscoreboard(GetUUID(a.playername), cfg.balance.scoreboard).ToString()));
                        return false;
                    }
                    return true;
                });
            }
            if (cfg.sign.enable)
            {
                if(cfg.sign.prize.Count < 7)
                {
                    Console.WriteLine("[Xtreme][Warn] 签到组件异常：prize项数目必须为7！！！");
                    for (var i= cfg.sign.prize.Count; i< 7; i++)
                    {
                        cfg.sign.prize.Add(new items()
                        {
                            itemid = 1,
                            itemaux = 0,
                            count = 1
                        });
                    }
                    var s = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
                    File.WriteAllText("./plugins/Xtreme/config.yaml", s.Serialize(cfg));
                    Console.WriteLine("[Xtreme][Warn] 签到组件已自动补全缺项为石头");
                }
                if(File.Exists("./plugins/Xtreme/sign.json") == false)
                {
                    File.WriteAllText("./plugins/Xtreme/sign.json", "{}");
                }
                var sign_dt = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText("./plugins/Xtreme/sign.json"));
                api.setCommandDescribe("sign", "签到");
                api.addBeforeActListener(EventKey.onInputCommand, x =>
                {
                    var a = BaseEvent.getFrom(x) as InputCommandEvent;
                    if(a.cmd == "/sign")
                    {
                        if (!sign_dt.ContainsKey(DateTime.Now.ToString("yy-MM-dd")))
                        {
                            sign_dt.Add(DateTime.Now.ToString("yy-MM-dd"), new List<string>());
                        }
                        if (sign_dt[DateTime.Now.ToString("yy-MM-dd")].Contains(a.playername))
                        {
                            api.sendText(GetUUID(a.playername), cfg.sign.message.already);
                            return false;
                        }
                        else
                        {
                            sign_dt[DateTime.Now.ToString("yy-MM-dd")].Add(a.playername);
                            api.sendText(GetUUID(a.playername), cfg.sign.message.done);
                            var it = cfg.sign.prize.ToArray()[int.Parse(DateTime.Now.DayOfWeek.ToString("d"))];
                            api.addPlayerItem(GetUUID(a.playername), it.itemid, (short)it.itemaux, (byte)it.count);
                            File.WriteAllText("./plugins/Xtreme/sign.json", JsonConvert.SerializeObject(sign_dt));
                            return false;
                        }                       
                    }
                    return true;
                });
            }
            if (cfg.hunter.enable)
            {
                Console.WriteLine("[Xtreme] 赏金猎人组件开启成功");
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
                                api.setscoreboard(GetUUID(a.srcname), cfg.hunter.scoreboard, api.getscoreboard(GetUUID(a.srcname), cfg.hunter.scoreboard) + ht[a.mobtype]);
                                api.sendText(GetUUID(a.srcname), cfg.hunter.message.Replace("%money%", ht[a.mobtype].ToString()));
                            }
                        }
                    }
                    return true;
                });
            }
            if (cfg.tpa.enable)
            {
                Console.WriteLine("[Xtreme] TPA组件开启成功");
                _TPA.tpa(api);
            }
            if (cfg.home.enable)
            {
                Console.WriteLine("[Xtreme] home组件开启成功");
                _HOME.home(cfg,api);
            }
            if (cfg.back.enable)
            {
                Console.WriteLine("[Xtreme] back组件开启成功");
                _BACK.back(cfg,api);
            }
            if (cfg.tpp.enable)
            {
                Console.WriteLine("[Xtreme] TPP组件开启成功");
                api.setCommandDescribe("tpp", "跨维度传送");
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
                    var cmd = a.cmd.Split(' ');
                    if(cmd[0] == "/tpp" && ifop(GetXUID(a.playername)))
                    {
                        if(cmd.Length == 5)
                        {
                            try
                            {
                                SymCall.teleport(api, a.playerPtr, Convert.ToSingle(cmd[1]), Convert.ToSingle(cmd[2]), Convert.ToSingle(cmd[3]), int.Parse(cmd[4]));
                            }
                            catch
                            {
                                api.sendText(GetUUID(a.playername), "输入的数据类型有误");
                            }                            
                            
                        }
                        else
                        {
                            api.sendText(GetUUID(a.playername), "使用方法：/tpp <int:x> <int:y> <int:z> <int:did>");
                        }
                        return false;
                    }
                    return true;
                   
                });
                api.addBeforeActListener(EventKey.onServerCmd, x =>
                {
                    var a = BaseEvent.getFrom(x) as ServerCmdEvent;
                    if (a.cmd.StartsWith("tpp "))
                    {
                        var cmd = a.cmd.Split(' ');
                        Regex reg = new Regex("\"(.+)\"");
                        Match match = reg.Match(a.cmd);
                        string value = match.Groups[1].Value;
                        if (string.IsNullOrEmpty(value))
                        {
                            if(cmd.Length == 6)
                            {
                                if (getPtr(cmd[1]) != IntPtr.Zero)
                                {
                                    try
                                    {
                                        SymCall.teleport(api, getPtr(cmd[1]), Convert.ToSingle(cmd[2]), Convert.ToSingle(cmd[3]), Convert.ToSingle(cmd[4]), int.Parse(cmd[5]));
                                    }
                                    catch (Exception e)
                                    {
                                        api.logout(e.Message);
                                    }
                                }
                                else
                                {
                                    api.logout("No targets matched selector");
                                }
                            }
                            else
                            {
                                api.logout("使用方法：/tpp <int:x> <int:y> <int:z> <int:did>");
                            }
                            
                        }
                        else
                        {
                            try
                            {
                                if (getPtr(value) != IntPtr.Zero)
                                {
                                    SymCall.teleport(api, getPtr(value), Convert.ToSingle(cmd[cmd.Length - 4]), Convert.ToSingle(cmd[cmd.Length - 3]), Convert.ToSingle(cmd[cmd.Length - 2]), int.Parse(cmd[cmd.Length - 1]));
                                }
                                else
                                {
                                    api.logout("No targets matched selector");
                                }
                            }
                            catch (Exception e)
                            {
                                api.logout(e.Message);
                            }
                        }                      
                        return false;
                    }
                    return true;
                });
            }
            if (cfg.econadmin.enable)
            {
                Console.WriteLine("[Xtreme] 经济管理组件开启成功");
                _ECONADMIN.econadmin(cfg, api);
            }
            if (cfg.hideSeed.enable)
            {
                Console.WriteLine("[Xtreme] 假种子组件开启成功");
                SymCall.HideSeed(api, cfg.hideSeed.seed);
            }
            //Console.WriteLine(Marshal.PtrToStringAuto(api.dlsym(0x01A995F0)));
            api.setSharePtr("Xtreme_tp", Marshal.GetFunctionPointerForDelegate<TELEPORT>(cs_teleport));
            Console.WriteLine("[Xtreme] (Share)[teleport] -> "+api.getSharePtr("Xtreme_tp"));
            //var tp = (TELEPORT)Marshal.GetDelegateForFunctionPointer(api.getSharePtr("Xtreme_tp"), typeof(TELEPORT));
            Console.WriteLine(@"   _  ____                         
  | |/ / /_________  ____ ___  ___ 
  |   / __/ ___/ _ \/ __ `__ \/ _ \
 /   / /_/ /  /  __/ / / / / /  __/
/_/|_\__/_/   \___/_/ /_/ /_/\___/                                      
");
            Console.WriteLine("[Xtreme] init!");
            Console.WriteLine("[Xtreme] version = " + version);
        }
    }
}
namespace CSR
{
    partial class Plugin
    {
        public static void onStart(MCCSAPI api)
        {
            try
            {
                Xtreme.Xtreme.Maximum(api);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

