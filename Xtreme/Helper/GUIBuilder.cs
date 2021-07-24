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
            var gui = new GUIS.GUIBuilder(api, j[0]["text"].ToString());
            foreach (var v in j)
            {
                //Console.WriteLine("v =>" + v["type"].ToString());
                try
                {
                    switch (v["type"].ToString())
                    {
                        case "Label":
                            gui.AddLabel(v["text"].ToString());
                            break;
                        case "Input":
                            gui.AddInput(v["text"].ToString());
                            break;
                        case "Toggle":
                            gui.AddToggle(v["text"].ToString());
                            break;
                        case "Dropdown":
                            gui.AddDropdown(v["text"].ToString(), int.Parse(v["_default"].ToString()), GetDropdown(v["options"].ToString()));
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
