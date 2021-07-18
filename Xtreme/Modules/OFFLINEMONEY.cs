using CSR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Xtreme
{
    class _OFFLINEMONEY
    {
        public static void offlinemoney(Setting cfg,MCCSAPI api)
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
            var deserializer = new DeserializerBuilder().Build();
            var serializer = new SerializerBuilder()
                .JsonCompatible()
                .Build();
            var omoney = JsonConvert.DeserializeObject<List<offlineMoney>>(serializer.Serialize(deserializer.Deserialize(new StringReader(File.ReadAllText("./plugins/Xtreme/offlineMoney.yaml")))));
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
            api.addBeforeActListener(EventKey.onLoadName, x =>
            {
                var a = BaseEvent.getFrom(x) as LoadNameEvent;
                if (Xtreme.onlines.Contains(a.playername))
                {
                    api.disconnectClient(a.uuid, "您的另一个账号在此服务器游玩");
                    return true;
                }
                Xtreme.onlines.Add(a.playername);
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
                    Xtreme.onlines.Remove(a.playername);
                }
                catch { }
                GetOmoney(a.playername).score = api.getscoreboard(GetUUID(a.playername), cfg.offlineMoney.scoreboard);
                var s = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
                File.WriteAllText("./plugins/Xtreme/offlineMoney.yaml", s.Serialize(omoney));
                return true;
            });
        }
    }
}
