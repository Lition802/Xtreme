using CSR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Xtreme
{
    class _TPP
    {
        public static void tpp(MCCSAPI api)
        {
            api.setCommandDescribe("tpp", "跨维度传送");
            var helper = new Helper(api);
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                var cmd = a.cmd.Split(' ');
                if (cmd[0] == "/tpp" && helper.ifop(helper.GetXUID(a.playername)))
                {
                    if (cmd.Length == 5)
                    {
                        try
                        {
                            SymCall.teleport(api, a.playerPtr, Convert.ToSingle(cmd[1]), Convert.ToSingle(cmd[2]), Convert.ToSingle(cmd[3]), int.Parse(cmd[4]));
                        }
                        catch
                        {
                            api.sendText(helper.GetUUID(a.playername), "输入的数据类型有误");
                        }

                    }
                    else
                    {
                        api.sendText(helper.GetUUID(a.playername), "使用方法：/tpp <int:x> <int:y> <int:z> <int:did>");
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
                        if (cmd.Length == 6)
                        {
                            if (helper.getPtr(cmd[1]) != IntPtr.Zero)
                            {
                                try
                                {
                                    SymCall.teleport(api, helper.getPtr(cmd[1]), Convert.ToSingle(cmd[2]), Convert.ToSingle(cmd[3]), Convert.ToSingle(cmd[4]), int.Parse(cmd[5]));
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
                            if (helper.getPtr(value) != IntPtr.Zero)
                            {
                                SymCall.teleport(api, helper.getPtr(value), Convert.ToSingle(cmd[cmd.Length - 4]), Convert.ToSingle(cmd[cmd.Length - 3]), Convert.ToSingle(cmd[cmd.Length - 2]), int.Parse(cmd[cmd.Length - 1]));
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
    }
}
