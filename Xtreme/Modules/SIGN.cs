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
    class _SIGN
    {
        public static void sign(Setting cfg,MCCSAPI api)
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
            if (cfg.sign.prize.Count < 7)
            {
                Console.WriteLine("[Xtreme][Warn] 签到组件异常：prize项数目必须为7！！！");
                for (var i = cfg.sign.prize.Count; i < 7; i++)
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
            if (File.Exists("./plugins/Xtreme/sign.json") == false)
            {
                File.WriteAllText("./plugins/Xtreme/sign.json", "{}");
            }
            var sign_dt = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText("./plugins/Xtreme/sign.json"));
            api.setCommandDescribe("sign", "签到");
            api.addBeforeActListener(EventKey.onInputCommand, x =>
            {
                var a = BaseEvent.getFrom(x) as InputCommandEvent;
                if (a.cmd == "/sign")
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
    }
}
