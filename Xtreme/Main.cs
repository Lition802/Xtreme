using CSR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Scripting.Hosting;
using MoonSharp.Interpreter;

namespace Xtreme
{
    public class Xtreme
    {
        public static List<string> onlines = new List<string>();
        public static bool iffeedback = false;
        public static string version = "1.6.7";
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
            var logger = new logger();
            var update =  new Update();
            var tmpv = update.getUpadte();
            update.getAnnouncement();
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
                        ptr2 = 0
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
                    }
                }) ;
                File.WriteAllText("./plugins/Xtreme/config.yaml", yaml);
            }                
            var deserializer = new DeserializerBuilder().Build();
            var yamlObject = deserializer.Deserialize(new StringReader(File.ReadAllText("./plugins/Xtreme/config.yaml")));
            var serializer = new SerializerBuilder()
                .JsonCompatible()
                .Build();
            var json = serializer.Serialize(yamlObject);
            var cfg = JsonConvert.DeserializeObject<Setting>(json);            
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
                }
                addvalue(SymCall.ptr1, cfg.@unsafe.ptr1);
                addvalue(SymCall.ptr2, cfg.@unsafe.ptr2);
                logger.LogWarn("您正在使用unsafe模式，数据错误可能会造成服务器崩溃");
            }        
            if (cfg.version != version)
            {
                logger.LogError("您的配置文件版本过旧，请备份后重新生成");
                logger.LogError("为安全考虑，已放弃加载");
                return;
            }
            _AUTOCLEAR.clear(cfg, api);
            _OFFLINEMONEY.offlinemoney(cfg, api);
            if (cfg.Timer.enable)
            {
                logger.Log("成功加载"+cfg.Timer.cmds.Keys.Count+"条定时任务");
                _TIMER.timer(cfg, api);
            }  
            if (cfg.DeathPunish.enable)
            {
                logger.Log("死亡惩罚组件开启成功");
                _DEATHPUNISH.deathpunish(cfg, api);
            }
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
                logger.Log("聊天管理组件加载成功");
                logger.Log("已载入本地敏感词库" + cfg.Chat.dirty.Keys.Count + "个");
                _CHAT.chat(cfg, api);
            }
            if (cfg.Itemdb.enable)
            {
                logger.Log("物品调试组件开启成功");
                _ITEMDB.itemdb(cfg, api);
            }
            if (cfg.Ban.blackbe.enable)
            {
                logger.Log("云黑组件开启成功");
                _BAN.cloudban(cfg, api);
            }
            if (cfg.Ban.localBan.enable)
            {
                logger.Log("本地黑名单组件开启成功");
                _BAN.localban(cfg, api);
            }
            if (cfg.Pay.enable)
            {
                logger.Log("转账组件开启成功");
                _PAY.pay(cfg, api);
            }
            if (cfg.hitokoto.enable)
            {
                _HITOKOTO.hitokoto(cfg, api);
            }
            if (cfg.menu.enable)
            {
                logger.Log("菜单组件开启成功");
                _MENU.menu(cfg, api);
            }
            if (cfg.balance.enable)
            {
                logger.Log("余额查询组件开启成功");
                _BALANCE.balance(cfg, api);
            }
            if (cfg.sign.enable)
            {               
                _SIGN.sign(cfg, api);
            }
            if (cfg.hunter.enable)
            {
                logger.Log("赏金猎人组件开启成功");
                _HUNTER.hunter(cfg, api);
            }
            if (cfg.tpa.enable)
            {
                logger.Log("TPA组件开启成功");
                _TPA.tpa(api);
            }
            if (cfg.home.enable)
            {
                logger.Log("home组件开启成功");
                _HOME.home(cfg,api);
            }
            if (cfg.back.enable)
            {
                logger.Log("back组件开启成功");
                _BACK.back(cfg,api);
            }
            if (cfg.tpp.enable)
            {
                logger.Log("TPP组件开启成功");
                _TPP.tpp(api);
            }
            if (cfg.econadmin.enable)
            {
                logger.Log("经济管理组件开启成功");
                _ECONADMIN.econadmin(cfg, api);
            }
            api.setSharePtr("Xtreme_tp", Marshal.GetFunctionPointerForDelegate<TELEPORT>(cs_teleport));
            Console.WriteLine("[Xtreme] (Share)[teleport] -> "+api.getSharePtr("Xtreme_tp"));
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

