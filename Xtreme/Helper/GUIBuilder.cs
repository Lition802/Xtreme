using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CSR;
using Newtonsoft.Json.Linq;

namespace Xtreme
{
    public class GUIBuilder
    {
        private  MCCSAPI api;
        private  static  string playername;
        public string[] GetDropdown(string i)
        {
            switch (i)
            {
                case "%o":
                    return Xtreme.onlines.ToArray();
                case "%h":
                    return _HOME.getHomeList(playername).ToArray();
                default:
                    return  i.Substring(1, i.Length - 1).Split('-');
            }
        }
        public GUIBuilder(MCCSAPI mcapi, string name)
        {
            playername = name;
            api = mcapi;
        }
        public uint GUI(string path)
        {
            var helper = new Helper(api);
            var j = JArray.Parse(convert_gui(path));
            //Console.WriteLine(j);
            var gui = new GUIS.GUIBuilder(api, j[0]["title"].ToString());
            for(int d = 1; d < j.Count-1; d++)
            {
                try
                {
                    switch (j[d]["type"].ToString())
                    {
                        case "Label":
                            gui.AddLabel(j[d]["text"].ToString());
                            break;
                        case "Input":
                            gui.AddInput(j[d]["text"].ToString());
                            break;
                        case "Toggle":
                            gui.AddToggle(j[d]["text"].ToString());
                            break;
                        case "Dropdown":
                            gui.AddDropdown(j[d]["text"].ToString(), int.Parse(j[d]["_default"].ToString()), GetDropdown(j[d]["options"].ToString()));
                            break;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
            return gui.SendToPlayer(helper.GetUUID(playername));
        }
        public string convert_gui(string gui)
        {
            var g = File.ReadAllLines("./plugins/Xtreme/gui/"+gui+".gui");
            var jsons = new List<Dictionary<string, string>>();           
            for (int index = 0; index < g.Length; index++)
            {
                if (g[index].StartsWith("#"))
                    continue;
                var line = g[index].Split(',');
                var dic = new Dictionary<string, string>();
                foreach (var item in line)
                {

                    //Console.WriteLine("add " + item.Split('=')[0] + " => " + item.Split('=')[1]);
                    try
                    {
                        dic.Add(item.Split('=')[0], item.Split('=')[1]);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    
                }
                jsons.Add(dic);
            }
            return JsonConvert.SerializeObject(jsons);
        }
    }
}
