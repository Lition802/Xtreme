using CSR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xtreme
{
    class _TIMER
    {
        public static void timer(Setting cfg,MCCSAPI api)
        {
            foreach (var i in cfg.Timer.cmds)
            {
                try
                {
                    new Thread(() =>
                    {
                        while (true)
                        {
                            Thread.Sleep(i.Value * 1000);
                            Xtreme.iffeedback = true;
                            api.runcmd(i.Key);
                        }
                    }).Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("------------------------------");
                    Console.WriteLine($"------------[ERROR:Timer:{i.Key}]--------------");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("------------------------------");
                }
            }
        }
    }
}
