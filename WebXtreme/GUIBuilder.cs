using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    class GUIBuilder
    {
        public static void gui()
        {
            var gui = File.ReadAllLines("pay.gui");
            for(int index = 0; index<gui.Length; index++)
            {
                var line = gui[index].Split(',');
                var dic = new Dictionary<string, string>();
                foreach(var item in line)
                {
                    dic.Add(item.Split('=')[0], item.Split('=')[1]);
                }
                if (index == 0)
                {

                }
            }
        }
    }
}
