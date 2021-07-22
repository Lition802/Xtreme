using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace DLLImport
{
    public static  class Program
    {
        [DllImport(@"a.dll", EntryPoint = "test", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void test(IntPtr intPtr);
        delegate IntPtr TEST(string s);
        static TEST e_test2 = (s) =>
        {
            Console.WriteLine(s);
            return Marshal.GetIUnknownForObject(s);
        };
        static TEST e_test = (s) =>
        {
            MessageBox.Show("饼干6666",s);
            Console.WriteLine(s);
            return Marshal.GetFunctionPointerForDelegate(e_test2);
        };
        static void Main(string[] args)
        {
            test(Marshal.GetFunctionPointerForDelegate(e_test));
        }
    }
}
