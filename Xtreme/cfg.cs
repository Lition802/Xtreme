using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    public class Message
    {
        /// <summary>
        /// 
        /// </summary>
        public string moneyEnough { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string moneyInsufficient { get; set; }
    }

    public class DeathPunish
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string scoreboard { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Message message { get; set; }
    }

    public class Clear_Message
    {
        /// <summary>
        /// 
        /// </summary>
        public string waiting { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string done { get; set; }
    }

    public class Items
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int timeout { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Clear_Message message { get; set; }
    }


    public class Mobs
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int timeout { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Clear_Message message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> keeps { get; set; }
    }

    public class AutoClear
    {
        /// <summary>
        /// 
        /// </summary>
        public Items items { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Mobs mobs { get; set; }
    }
    public class OMoney
    {
        /// <summary>
        /// 
        /// </summary>
        public string scoreboard { get; set; }
    }

    public class Chat
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int MaxLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string,int> dirty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
    }
    public class Timer
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string,int> cmds { get; set; }
    }
    public class Itemdb
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
    }
    public class Blackbe
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string messsage { get; set; }
    }

    public class LocalBan
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string messsage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> hackers { get; set; }
    }

    public class Ban
    {
        /// <summary>
        /// 
        /// </summary>
        public Blackbe blackbe { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public LocalBan localBan { get; set; }
    }
    public class Pay
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int rate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Message message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string scoreboard { get; set; }
    }
    public class Hitokoto
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> joinMessage { get; set; }
    }
    public class Menu
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int itemid { get; set; }
    }
    public class Balance
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string command { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string scoreboard { get; set; }
        /// <summary>
        /// 您的余额为%money%金币
        /// </summary>
        public string message { get; set; }
    }

    public class items
    {
        /// <summary>
        /// 
        /// </summary>
        public int itemid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int itemaux { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
    }

    public class sign_Message
    {
        /// <summary>
        /// 
        /// </summary>
        public string done { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string already { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wait { get; set; }
    }

    public class Sign
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<items> prize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public sign_Message message { get; set; }
    }
    public class Hunter
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string scoreboard { get; set; }
    }
    public class TPA
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
    }
    public class PFET_Home
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int max { get; set; }
    }
    public class Cost
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int money { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string scoreboard { get; set; }
        public Message message { get; set; }
    }

    public class Back
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Cost cost { get; set; }
    }
    public class Tpp
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
    }
    public class Econadmin
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string scoreboard { get; set; }
    }
    public class Unsafe
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ptr1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ptr2 { get; set; }

    }
    public class Setting
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 不安全代码
        /// </summary>
        public Unsafe @unsafe { get; set; }
        /// <summary>
        /// 死亡惩罚
        /// </summary>
        public DeathPunish DeathPunish { get; set; }
        /// <summary>
        /// 自动清理
        /// </summary>
        public AutoClear AutoClear { get; set; }
        /// <summary>
        /// 定时器
        /// </summary>
        public Timer Timer { get; set; }
        /// <summary>
        /// 离线计分板
        /// </summary>
        public OMoney offlineMoney { get; set; }
        /// <summary>
        /// 聊天管理
        /// </summary>
        public Chat Chat { get; set; }
        /// <summary>
        /// idbg
        /// </summary> 
        public Itemdb Itemdb { get; set; }
        /// <summary>
        /// idbg
        /// </summary> 
        public Ban Ban { get; set; }
        /// <summary>
        /// 转帐
        /// </summary>
        public Pay Pay { get; set; }
        /// <summary>
        /// 一言
        /// </summary>
        public Hitokoto hitokoto { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public Menu menu { get; set; }
        /// <summary>
        /// 余额查询
        /// </summary>
        public Balance balance { get; set; }
        /// <summary>
        /// 签到
        /// </summary>
        public Sign sign { get; set; }
        /// <summary>
        /// 赏金猎人
        /// </summary>
        public Hunter hunter { get; set; }
        /// <summary>
        /// tpa
        /// </summary>
        public TPA tpa { get; set; }
        /// <summary>
        /// home
        /// </summary>
        public PFET_Home home { get; set; }
        /// <summary>
        /// back
        /// </summary>
        public Back back { get; set; }
        /// <summary>
        /// tpp
        /// </summary>
        public Tpp tpp { get; set; }
        /// <summary>
        /// 经济管理
        /// </summary>
        public Econadmin econadmin { get; set; }
    }


}
