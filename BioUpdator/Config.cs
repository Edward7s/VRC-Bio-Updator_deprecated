using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioUpdator
{
    internal class Config
    {
        internal protected static Json s_json { get; set; }
        public class Json
        {
            public string? UserId { get; set; }
            public string? AuthCookie { get; set; }

        }
        private string _fileName { get; } = "\\VRCBioUpdator.json";
        public Config()
        {
            try
            {
                Console.WriteLine("Here its an example on how to make your bio update");
                Console.WriteLine(@"< < < < < < < < < < Friends:
- Online Friends Fo: 7;
- Offline Friends Of: 41;
- Total Friends Ft: 52;
< < < < < < < < < < World:
- Current World W: The Black Cat;
- Instance Type T: Public;
- Users In World U: 32; 
- Capacity C: 18;
- Region R: unknown;
< < < < < < < < < < Avatar:
- Avatar A: Optimized Karin;
- Avatar Version Av: 44;");
                  RegistryKey startup = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                 if (startup.GetValue("VRCBioUpdator") == null)
                   startup.SetValue("VRCBioUpdator", Directory.GetCurrentDirectory() + "\\BioUpdator.exe");



                if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + _fileName))
                {
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + _fileName, JsonConvert.SerializeObject(new Config.Json()
                    {
                        AuthCookie = String.Empty,
                        UserId = String.Empty,
                    }));
                    return;
                }
                s_json = JsonConvert.DeserializeObject<Config.Json>(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + _fileName));
                Console.ForegroundColor = ConsoleColor.Red;
                if (s_json.AuthCookie == String.Empty)
                    Console.WriteLine("Please Add Your AuthCookie To The: " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + _fileName + "  Then Restart The App");

                if (s_json.UserId == String.Empty)
                    Console.WriteLine("Please Add Your UserId To The: " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + _fileName + "  Then Restart The App");
                Console.ForegroundColor = ConsoleColor.Green;

                new WebReuests();
                new Loop();
            }
            catch (Exception ex) { Console.WriteLine(ex); }


        }

    }
}
