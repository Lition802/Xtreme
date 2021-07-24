using CSR;
using MoonSharp.Interpreter;
using MoonSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Xtreme.LuaEngine
{
    class init
    {
        static MCCSAPI mapi;

        static string JSString(object o)
        {
            return o?.ToString();
        }
        static JavaScriptSerializer ser = new JavaScriptSerializer();
        delegate bool RENAMEBYUUID(object uuid, object n);
        delegate string GETPLAYERABILITIES(object uuid);
        delegate bool SETPLAYERABILITIES(object uuid, object a);
        delegate bool ADDPLAYERITEM(object uuid, object id, object aux, object count);
        delegate bool SETPLAYERBOSSBAR(object uuid, object title, object per);
        delegate bool REMOVEPLAYERBOSSBAR(object uuid);
        delegate bool TRANSFERSERVER(object uuid, object addr, object port);
        delegate bool TELEPORT(object uuid, object x, object y, object z, object did);
        delegate uint SENDSIMPLEFORM(object uuid, object title, object content, object buttons);
        delegate uint SENDMODALFORM(object uuid, object title, object content, object button1, object button2);
        delegate uint SENDCUSTOMFORM(object uuid, object json);
        delegate bool RELEASEFORM(object formid);
        delegate bool SETPLAYERSIDEBAR(object uuid, object title, object list);
        delegate int GETSCOREBOARD(object uuid, object stitle);
        delegate bool RUNCMD(object cmd);
        /// <summary>
        /// 重命名一个指定的玩家名
        /// </summary>
        static RENAMEBYUUID cs_reNameByUuid = (uuid, name) =>
        {
            return mapi.reNameByUuid(JSString(uuid), JSString(name));
        };
        /// <summary>
        /// 增加玩家一个物品(简易方式)
        /// </summary>
        static ADDPLAYERITEM cs_addPlayerItem = (uuid, id, aux, count) =>
        {
            return mapi.addPlayerItem(JSString(uuid), int.Parse(JSString(id)), short.Parse(JSString(aux)), byte.Parse(JSString(count)));
        };
        /// <summary>
        /// 查询在线玩家基本信息
        /// </summary>
        static GETPLAYERABILITIES cs_selectPlayer = (uuid) =>
        {
            return mapi.selectPlayer(JSString(uuid));
        };
        /// <summary>
        /// 模拟玩家发送一个文本
        /// </summary>
        static SETPLAYERABILITIES cs_talkAs = (uuid, a) =>
        {
            return mapi.talkAs(JSString(uuid), JSString(a));
        };
        /// <summary>
        /// 模拟玩家执行一个指令
        /// </summary>
        static SETPLAYERABILITIES cs_runcmdAs = (uuid, a) =>
        {
            return mapi.runcmdAs(JSString(uuid), JSString(a));
        };
        /// <summary>
        /// 向指定的玩家发送一个简单表单
        /// </summary>
        static SENDSIMPLEFORM cs_sendSimpleForm = (uuid, title, content, buttons) =>
        {
            return mapi.sendSimpleForm(JSString(uuid), JSString(title), JSString(content), JSString(buttons));
        };
        /// <summary>
        /// 向指定的玩家发送一个模式对话框
        /// </summary>
        static SENDMODALFORM cs_sendModalForm = (uuid, title, content, button1, button2) =>
        {
            return mapi.sendModalForm(JSString(uuid), JSString(title), JSString(content), JSString(button1), JSString(button2));
        };
        /// <summary>
        /// 向指定的玩家发送一个自定义表单
        /// </summary>
        static SENDCUSTOMFORM cs_sendCustomForm = (uuid, json) =>
        {
            return mapi.sendCustomForm(JSString(uuid), JSString(json));
        };
        /// <summary>
        /// 放弃一个表单
        /// </summary>
        static RELEASEFORM cs_releaseForm = (formid) =>
        {
            return mapi.releaseForm(uint.Parse(JSString(formid)));
        };
        /// <summary>
        /// 断开一个玩家的连接
        /// </summary>
        static SETPLAYERABILITIES cs_disconnectClient = (uuid, a) =>
        {
            return mapi.disconnectClient(JSString(uuid), JSString(a));
        };
        /// <summary>
        /// 发送一个原始显示文本给玩家
        /// </summary>
        static SETPLAYERABILITIES cs_sendText = (uuid, a) =>
        {
            return mapi.sendText(JSString(uuid), JSString(a));
        };
        /// <summary>
        /// 获取指定玩家指定计分板上的数值<br/>
        /// 注：特定情况下会自动创建计分板
        /// </summary>
        static GETSCOREBOARD cs_getscoreboard = (uuid, a) =>
        {
            return mapi.getscoreboard(JSString(uuid), JSString(a));
        };

        /// <summary>
        /// 设置指定玩家指定计分板上的数值
        /// </summary>
        static TRANSFERSERVER cs_setscoreboard = (uuid, stitle, count) =>
        {
            return mapi.setscoreboard(JSString(uuid), JSString(stitle), int.Parse(JSString(count)));
        };
        /// <summary>
        /// 获取玩家IP
        /// </summary>
        static GETPLAYERABILITIES cs_getPlayerIP = (uuid) =>
        {
            var data = mapi.selectPlayer(JSString(uuid));
            if (!string.IsNullOrEmpty(data))
            {
                var pinfo = ser.Deserialize<Dictionary<string, object>>(data);
                if (pinfo != null)
                {
                    object pptr;
                    if (pinfo.TryGetValue("playerptr", out pptr))
                    {
                        var ptr = (IntPtr)Convert.ToInt64(pptr);
                        if (ptr != IntPtr.Zero)
                        {
                            CsPlayer p = new CsPlayer(mapi, ptr);
                            var ipport = p.IpPort;
                            var ip = ipport.Substring(0, ipport.IndexOf('|'));
                            return ip;
                        }
                    }
                }
            }
            return string.Empty;
        };
        /// <summary>
        /// 执行后台指令
        /// </summary>
        static RUNCMD cs_runcmd = (cmd) =>
        {
            return mapi.runcmd(JSString(cmd));
        };
        public Script initEngine(MCCSAPI mcapi, Script eng)
        {
            mapi = mcapi;
            eng.Globals["sendText"] = cs_sendText;
            return eng;
        }
    }
}
