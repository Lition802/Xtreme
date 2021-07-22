using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSR;
using Newtonsoft.Json.Linq;

namespace Xtreme
{
    class _AUTOCLEAR
    {
        public static void clear(Setting cfg,MCCSAPI api)
        {
            string GetClearMobs(List<string> s)
            {
                string cmd = "kill @e[type=!player";
                foreach (var i in s)
                {
                    cmd += ",type=!" + i;
                };
                return cmd + "]";
            }
            var helper = new Helper(api);
            if (cfg.AutoClear.items.enable)
            {
                Console.WriteLine("[Xtreme] 定时清理掉落物品组件开启成功");
                new Thread(() =>
                {
                    int t = cfg.AutoClear.items.timeout;
                    while (true)
                    {
                        Thread.Sleep(1000);
                        t--;
                        switch (t)
                        {
                            case 0:
                                //api.logout("[Xtreme] 即将清理掉落物品");
                                Xtreme.iffeedback = true;
                                api.runcmd("kill @e[type=item]");
                                helper.SendAllText(cfg.AutoClear.items.message.done);
                                t = cfg.AutoClear.items.timeout;
                                break;
                            case 5:
                                helper.SendAllText(cfg.AutoClear.items.message.waiting.Replace("%time%", "5"));
                                break;
                            case 10:
                                helper.SendAllText(cfg.AutoClear.items.message.waiting.Replace("%time%", "10"));
                                break;
                        }
                    }
                }).Start();
            }
            if (cfg.AutoClear.mobs.enable)
            {
                Console.WriteLine("[Xtreme] 定时清理生物组件开启成功");
                new Thread(() =>
                {
                    Thread.Sleep(5000);
                    int t = cfg.AutoClear.items.timeout;
                    while (true)
                    {
                        Thread.Sleep(1000);
                        t--;
                        switch (t)
                        {
                            case 0:
                                //api.logout("[Xtreme] 即将清理生物");
                                Xtreme.iffeedback = true;
                                api.runcmd(GetClearMobs(cfg.AutoClear.mobs.keeps));
                                helper.SendAllText(cfg.AutoClear.mobs.message.done);
                                t = cfg.AutoClear.mobs.timeout;
                                break;
                            case 5:
                                helper.SendAllText(cfg.AutoClear.mobs.message.waiting.Replace("%time%", "5"));
                                break;
                            case 10:
                                helper.SendAllText(cfg.AutoClear.mobs.message.waiting.Replace("%time%", "10"));
                                break;
                        }
                    }
                }).Start();
            }
        }
    }
}
