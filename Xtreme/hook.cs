using CSR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    public static class SymCall
    {
        private struct Vec3
        {
            public float x, y, z;
        }
        static int i = 0;
        public static void log()
        {
            Console.WriteLine(++i);
        }
        delegate IntPtr TP_Func1(IntPtr mem, IntPtr pl, Vec3 pos, int a1, int a2, float a3, float a4, int a5);
        delegate void TP_Func2(IntPtr pl, IntPtr mem);
        public static Dictionary<string, int> ptr1 = new Dictionary<string, int>()
        {
            {"1.17.0.03",0x0096B880 },
            {"1.17.10.04",0x0099DB60}
        };
        public static Dictionary<string, int> ptr2 = new Dictionary<string, int>()
        {
            {"1.17.0.03",0x0096B660 },
            {"1.17.10.04",0x0099D940}
        };
        public static Dictionary<string, int> ptr3 = new Dictionary<string, int>()
        {
            {"1.17.10.04",0x008EB610}
        };
        static int fakeseed;
        delegate ulong HIDESEEDPACKET(IntPtr a1, ulong a2);
        static IntPtr hideorg = IntPtr.Zero;
        /// <summary>
        /// 新方法，hook方式修改数据包，隐藏种子
        /// </summary>
        /// <param name="a1">StartGamePacket指针</param>
        /// <param name="a2">BinaryStream引用</param>
        /// <returns></returns>
        private static readonly HIDESEEDPACKET hide = (a1, a2) => {
            Marshal.WriteInt32(a1 + 40, fakeseed);
            var org = Marshal.GetDelegateForFunctionPointer<HIDESEEDPACKET>(hideorg);
            return org(a1, a2);
        };
        public static unsafe void teleport(MCCSAPI api, IntPtr pl, float x, float y, float z, int dimid)
        {
            try
            {
                if (!ptr1.ContainsKey(api.VERSION))
                {
                    Console.WriteLine("[Xtreme] teleport 版本未适配");
                    return;
                }
                Vec3 vec = new Vec3
                {
                    x = x,
                    y = y,
                    z = z
                };
                //log();
                var mem = stackalloc char[128];
                //log();
                var org = api.dlsym(ptr1[api.VERSION]);//computeTarget@TeleportCommand@@SA?AVTeleportTarget@@AEAVActor@@VVec3@@PEAV4@V?$AutomaticID@VDimension@@H@@VRelativeFloat@@4H@Z
                //log();
                var func = (TP_Func1)Marshal.GetDelegateForFunctionPointer(org, typeof(TP_Func1));
                //log();
                func((IntPtr)mem, pl, vec, 0, dimid, 0, 0, 15);
                var org2 = api.dlsym(SymCall.ptr2[api.VERSION]);//?applyTarget@TeleportCommand@@SAXAEAVActor@@VTeleportTarget@@@Z
                //log();
                var func2 = (TP_Func2)Marshal.GetDelegateForFunctionPointer(org2, typeof(TP_Func2));
                func2(pl, (IntPtr)mem);
                //log();
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void HideSeed(MCCSAPI api, int seed)
        {
            fakeseed = seed;
            int rva = 0;
            try {
                rva = ptr3[api.VERSION];
            }
            catch
            {
                Console.WriteLine("[Xtreme] HideSeed 版本未适配");
            }
            if (rva != 0)
            {
                bool ret = api.cshook(rva,  // IDA write@StartGamePacket@@UEBAXAEAVBinaryStream@@@Z
                               Marshal.GetFunctionPointerForDelegate(hide),
                               out hideorg);
                if (!ret)
                    Console.WriteLine("[Xtreme][HideSeed] Hook失败!");
                //Console.WriteLine(Marshal.PtrToStringAnsi(api.dlsym(0x01A1CB38)));
            }
        }
    }
}
