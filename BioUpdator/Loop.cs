using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BioUpdator.Config;

namespace BioUpdator
{
    internal class Loop
    {
        public Loop()
        {
            if (Config.s_json.AuthCookie == String.Empty || Config.s_json.UserId == String.Empty) return;
            Console.WriteLine("Starting Loop.");
            Task.Run(LoopAction());
            // LoopAction();
        }
        private string _bio { get; set; } = string.Empty;
        private string _updatedBio { get; set; } = string.Empty;

        private JObject _jObject { get; set; }
        private int _friendsOnline { get; set; }
        private int _friendsTotal { get; set; }
        private int _friendsOffline { get; set; }
        private string _worldId { get; set; } = string.Empty;
        private string _instanceId { get; set; } = string.Empty;
        private int _usersInWorlds { get; set; }
        private int _capacity { get; set; }
        private string _region { get; set; } = string.Empty;
        private string _instanceType { get; set; } = string.Empty;
        private string _worldName { get; set; } = string.Empty;
        private string _avatarId { get; set; } = string.Empty;
        private string _avatarName { get; set; } = string.Empty;
        private int _avatarVersion { get; set; }

        private Action LoopAction()
        {
            while (true)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(10000);
                try
                {
                    if (Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "VRChat") == null) continue;
                    _jObject = JObject.FromObject(JObject.Parse(WebReuests.Instance.SendVRCWebReq(WebReuests.RequestType.Get, Urls.VRCApiLink + Urls.LocalUser)));
                    _bio = (string)_jObject["bio"];
                    _friendsOnline = _jObject["onlineFriends"].ToArray().Length;
                    _friendsOffline = _jObject["offlineFriends"].ToArray().Length;
                    _friendsTotal = _jObject["friends"].ToArray().Length;
                    Thread.Sleep(100);
                    _jObject = JObject.FromObject(JObject.Parse(WebReuests.Instance.SendVRCWebReq(WebReuests.RequestType.Get, Urls.VRCApiLink + Urls.UserEndPoint + Config.s_json.UserId)));
                    _avatarId = (string)_jObject["currentAvatar"];
                    _worldId = (string)_jObject["worldId"];
                    _instanceId = (string)_jObject["instanceId"];
                    Thread.Sleep(100);
                    if (_worldId == "offline" || _worldId == "traveling")
                    {
                        _usersInWorlds = 0;
                        _capacity = 0;
                        _region = _worldId;
                        _instanceType = _worldId;
                        _worldName = _worldId;

                    }
                    else
                    {
                        _jObject = JObject.FromObject(JObject.Parse(WebReuests.Instance.SendVRCWebReq(WebReuests.RequestType.Get, Urls.VRCApiLink + Urls.Instance + _worldId + ":" + _instanceId)));
                        _usersInWorlds = (int)_jObject["n_users"];
                        _capacity = (int)_jObject["capacity"];
                        _region = (string)_jObject["region"];
                        _instanceType = Extentions.InstanceType(_jObject);
                        Thread.Sleep(100);
                        _jObject = JObject.FromObject(JObject.Parse(WebReuests.Instance.SendVRCWebReq(WebReuests.RequestType.Get, Urls.VRCApiLink + Urls.Worlds + _worldId)));
                        _worldName = Extentions.ValidateString((string)_jObject["name"]);
                    }
                    Thread.Sleep(100);
                    _jObject = JObject.FromObject(JObject.Parse(WebReuests.Instance.SendVRCWebReq(WebReuests.RequestType.Get, Urls.VRCApiLink + Urls.Avatar + _avatarId)));
                    _avatarName = (string)_jObject["name"];
                    _avatarVersion = (int)_jObject["version"];
                    Thread.Sleep(100);
                    _updatedBio = Extentions.FormBetterDescription(_bio, _friendsOnline, _friendsTotal, _friendsOffline, _usersInWorlds, _capacity, _region, _instanceType, _worldName, _avatarName, _avatarVersion);
                    if (_updatedBio == string.Empty || _updatedBio == _bio)
                        continue;
                    WebReuests.Instance.SendVRCWebReq(WebReuests.RequestType.Put, Urls.VRCApiLink + Urls.UserEndPoint + Config.s_json.UserId, (object)new
                    {
                        bio = _updatedBio,
                    });
                }
                catch { }


            }
        }
    }
}
