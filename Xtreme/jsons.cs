using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtreme
{
    public class offlineMoney
    {
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int score { get; set; }
    }
    public class HandContainer
    {
        /// <summary>
        /// 
        /// </summary>
        public int Slot { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string item { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rawnameid { get; set; }
    }
    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string xbox_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 测试信息
        /// </summary>
        public string info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int qq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int trial { get; set; }
        /// <summary>
        /// 测试
        /// </summary>
        public string server { get; set; }
    }

    public class Bbe
    {
        /// <summary>
        /// 
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int error { get; set; }
        /// <summary>
        /// 存在违规行为
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
    }
    public class YiYan
    {
        /// <summary>
        /// 一言标识
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// 一言正文。编码方式 unicode。使用 utf-8。
        /// </summary>
        public string hitokoto { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 一言的出处
        /// </summary>
        public string from { get; set; }
        /// <summary>
        /// 一言的作者
        /// </summary>
        public string from_who { get; set; }
        /// <summary>
        /// 添加者
        /// </summary>
        public string creator { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int creator_uid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int reviewer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string commit_from { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string created_at { get; set; }
        /// <summary>
        /// 句子长度
        /// </summary>
        public int length { get; set; }
    }
    public class Home
    {
        /// <summary>
        /// 
        /// </summary>
        public float x { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float y { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float z { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int dimid { get; set; }
    }


}
namespace LMenu
{
    public class Image
    {
        /// <summary>
        /// 图片类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 图片数据来源
        /// </summary>
        public string data { get; set; }
    }

    public class ButtonsItem
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public Image image { get; set; }
    }

    public class CustomsMenu
    {
        /// <summary>
        /// 表单类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ButtonsItem> buttons { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string content { get; set; }
    }
}
namespace LMenuConfig
{
    public class Image
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string data { get; set; }
    }

    public class ButtonsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public bool images { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Image image { get; set; }
        /// <summary>
        /// 基础菜单
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string command { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
    }

    public class config
    {
        /// <summary>
        /// 幻想空域菜单
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 菜单如下
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ButtonsItem> buttons { get; set; }
    }
    public class menuconfig
    {
        /// <summary>
        /// 
        /// </summary>
        public int itemid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string version { get; set; }
        public bool convertYaml { get; set; }
        public string yamlPath { get; set; }
    }
}
