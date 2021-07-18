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
                var jo = JArray.Parse(api.getOnLinePlayers());
                foreach (var i in jo)
                {
                    if (i["playername"].ToString() == p)
                        return i["xuid"].ToString();
                }
                Console.WriteLine($"[Xtreme] 无法找到玩家{p}的XUID!");
                return null;

            }
            IntPtr getPtr(string p)
            {
                if (GetUUID(p) != null)
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
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                var cmd = a.cmd.Split(' ');
                if (cmd[0] == "/tpp" && ifop(GetXUID(a.playername)))
                {
                    if (cmd.Length == 5)
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
                        if (cmd.Length == 6)
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
    }
}
