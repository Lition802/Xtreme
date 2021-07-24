using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WebXtreme
{
    class Program
    {
        public static string GetUrlIndex(string url)
        {
            int end = url.IndexOf("?");
            if (end == -1)
            {
                return url;
            }
            return url.Substring(0, end);
        }
        public static string json_dump(int c,object t)
        {
            return JsonConvert.SerializeObject(new HttpReturn()
            {
                code = c,
                data = t
            });
        }
        static void Main(string[] args)
        {
            var cfg =JsonConvert.DeserializeObject<Setting>(File.ReadAllText("setting.json"));
            HttpListener listener = new HttpListener();
            string address = "http://+:" + cfg.port+"/";
            listener.Prefixes.Add(address);
            listener.Start();
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                Task.Run(() =>
                {
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    response.ContentType = "application/json;charset=UTF-8";
                    response.ContentEncoding = Encoding.UTF8;
                    string responseString = "{\"code\":400}";
                    Console.WriteLine(GetUrlIndex(request.RawUrl));
                    Console.WriteLine($"[{DateTime.Now}] accept request from [{request.RemoteEndPoint.Address}] , type is {request.HttpMethod}");
                    File.WriteAllText($"./logs/{DateTime.Now.Date.ToString("yyyy-M-d")}.log",$"[{DateTime.Now}] accept request from [{request.RemoteEndPoint.Address}] , type is {request.HttpMethod}");
                    switch (request.HttpMethod)
                    {
                        case "GET":
                            switch (GetUrlIndex(request.RawUrl))
                            {
                                case "/announcement":
                                    responseString = json_dump(0,cfg.announcement);
                                    break;
                                case "/update":
                                    responseString = json_dump(0,cfg.update.version);
                                    break;
                                case "/init":
                                    bool load = true;
                                    if (cfg.ban.enable)
                                    {
                                        if (cfg.ban.list.Contains(request.RemoteEndPoint.Address.ToString()))
                                        {
                                            load = false;
                                        }
                                    }
                                    responseString = json_dump(0, load);
                                    break;
                                case "/update/url":
                                    responseString = json_dump(0, cfg.update.URL);
                                    break;
                                case "/ptr":
                                    responseString = json_dump(0, cfg.ptr);
                                    break;
                            }
                            break;

                        case "POST":

                            break;
                    }
                    
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    response.StatusCode = 200;
                    output.Close();
                });
            }
        }
    }
}
