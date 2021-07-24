using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebXtreme
{
    public class Ban
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> list { get; set; }
    }
    public class Update
    {
        /// <summary>
        /// 
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string URL { get; set; }
    }
    public class PtrItem
{
    /// <summary>
    /// 
    /// </summary>
    public string ptr1 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ptr2 { get; set; }
}
public class Setting
    {
        /// <summary>
        /// 
        /// </summary>
        public int port { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> announcement { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Ban ban { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Update update { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string,PtrItem> ptr { get; set; }
    }
    public class HttpReturn
    {
        public int code { get; set; }
        public object data { get; set; }
    }
}