using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Text.Unicode;
using System.IO.Compression;

namespace BioUpdator
{
    public class WebReuests
    {
        public static WebReuests Instance { get; set; }
        public enum RequestType
        {
            Put,
            Get
        }
        public WebReuests()
        {
            Instance = this;
            s_cookies.Add(new Cookie() { Name = "apiKey", Value = "JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26", Domain = "vrchat.com" });
            s_cookies.Add(new Cookie() { Name = "auth", Value = Config.s_json.AuthCookie, Domain = "vrchat.com" });
        }

        private CookieContainer s_cookies = new CookieContainer();
        public HttpWebRequest VRCRequest { get; private set; }
        public HttpWebResponse WebResponse { get; private set; }
        private string s_payload { get; set; } = string.Empty;
        public string SendVRCWebReq(RequestType req, string url, object? payload = null)
        {
            s_payload = string.Empty;
            if (payload != null)
                s_payload = JsonConvert.SerializeObject(payload);
            VRCRequest = (HttpWebRequest)WebRequest.Create(url);
            VRCRequest.CookieContainer = s_cookies;
            VRCRequest.Method = req == 0 ? "Put" : "Get";
            VRCRequest.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Mobile Safari/537.36";
            VRCRequest.ContentType = "application/json";
            VRCRequest.Accept = "application/json, text/plain, */*";
            VRCRequest.SendChunked = true;
            VRCRequest.ContentLength = s_payload == null ? 0 : s_payload.Length;
            VRCRequest.AutomaticDecompression = DecompressionMethods.GZip;
            VRCRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            if (s_payload != string.Empty)
            {
                using (var writer = new StreamWriter(VRCRequest.GetRequestStream(), Encoding.UTF8))
                    writer.Write(s_payload);
            }
            WebResponse = (HttpWebResponse)VRCRequest.GetResponse();
            using (var reader = new StreamReader(WebResponse.GetResponseStream(), ASCIIEncoding.UTF8))
                return reader.ReadToEnd();

        }

    }
}
