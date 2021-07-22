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
    class Helper
    {
        private MCCSAPI api;
        public Helper(MCCSAPI api)
        {
            this.api = api;
        }
        public string GetUUID(string p)
        {
            var j = JArray.Parse(api.getOnLinePlayers());
            foreach (var i in j)
            {
                if (i["playername"].ToString() == p)
                    return i["uuid"].ToString();
            }
            return GetUUID(Xtreme.onlines.ToArray()[0]);
        }
        public bool ifop(string xuid)
        {
            foreach (var p in JArray.Parse(File.ReadAllText("permissions.json")))
            {
                if (p["xuid"].ToString() == xuid && p["permission"].ToString() == "operator")
                {
                    return true;
                }
            }
            return false;
        }
        public string GetXUID(string p)
        {
            var jo = JArray.Parse(api.getOnLinePlayers());
            foreach (var i in jo)
            {
                if (i["playername"].ToString() == p)
                    return i["xuid"].ToString();
            }
            return GetXUID(Xtreme.onlines.ToArray()[0]);
        }
        public IntPtr getPtr(string p)
        {
            if (GetUUID(p) != null)
            {
                var data = api.selectPlayer(GetUUID(p));
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
        public void SendAllText(string msg)
        {
            if (Xtreme.onlines.Count > 0)
            {
                var jspn = JArray.Parse(api.getOnLinePlayers());
                try
                {
                    foreach (var i in jspn)
                    {
                        api.sendText(i["uuid"].ToString(), msg);
                    }
                }
                catch { }
            }
        }
    }
}
