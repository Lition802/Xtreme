using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    class logger
    {
        public static void Log(object t)
        {
            Console.WriteLine($"[INFO][Xtreme] {t}");
        }
        public static void LogSuccess(object t)
        {
            Console.WriteLine($"[Success][Xtreme] {t}");
        }
        public static void LogWarn(object t)
        {
            Console.WriteLine($"[Warn][Xtreme] {t}");
        }
        public static void LogError(object t)
        {
            Console.WriteLine($"[Error][Xtreme] {t}");
        }
    }
}
