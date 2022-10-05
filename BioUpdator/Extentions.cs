using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BioUpdator
{
    internal class Extentions
    {
     
        public static string InstanceType(JObject jobject)
        {
            if (jobject["hidden"] != null)
                return "Friends+";
            if (jobject["friends"] != null)
                return "Friends";
            if (jobject["private"] != null)
                return "Private";

            return "Public";
        }
        public static string ValidateString(string str)
        {
            if (str.Contains(";"))
                str = str.Replace(";", ":");

            return str.Length >= 30 ? str.Remove(str.Length - 30, str.Length - 30) : str;
        }
        
        private static int CountSubStr(string str, string pattern)
        {
            int count = 0, minIndex = str.IndexOf(pattern, 0);
            while (minIndex != -1)
            {
                minIndex = str.IndexOf(pattern, minIndex + pattern.Length);
                count++;
            }
            return count;
        }
        private static int s_index { get; set; } = 0;
        public static string FormBetterDescription(string bio, int friendsOnline, int friendsTotal, int friendsOffline, int usersInWorld,
            int capacity, string region, string instanceType, string worldName, string avatarName, int avatarVersion)
        {

            for (int i = 0; i < s_keyWords.Length; i++)
            {

                if (bio.Contains(s_keyWords[i]))
                {
                    switch (s_keyWords[i].ToString())
                    {
                        case " Fo˸":
                            bio = ReplaceKeyWord(" Fo˸", bio, friendsOnline);
                            break;
                        case " Of˸":
                            bio = ReplaceKeyWord(" Of˸", bio, friendsOffline);
                            break;
                        case " Ft˸":
                            bio = ReplaceKeyWord(" Ft˸", bio, friendsTotal);
                            break;
                        case " U˸":
                            bio = ReplaceKeyWord(" U˸", bio, usersInWorld);
                            break;
                        case " C˸":
                            bio = ReplaceKeyWord(" C˸", bio, capacity);
                            break;
                        case " R˸":
                            bio = ReplaceKeyWord(" R˸", bio, region);
                            break;
                        case " T˸":
                            bio = ReplaceKeyWord(" T˸", bio, instanceType);
                            break;
                        case " W˸":
                            bio = ReplaceKeyWord(" W˸", bio, worldName);
                            break;
                        case " A˸":
                            bio = ReplaceKeyWord(" A˸", bio, avatarName);
                            break;
                        case " Av˸":
                            bio = ReplaceKeyWord(" Av˸", bio, avatarVersion);
                            break;
                    }
                }
                // if (Regex.Matches(bio, s_keyWords[i]).Count <= 1) continue;
                if (CountSubStr(bio, s_keyWords[i]) <= 1) continue;
                Console.WriteLine("ERROR: You Have The Same KeyWord Multiple Times: " + s_keyWords[i]);
                return String.Empty;
            }
            return bio.Length >= 512 ? bio.Remove(bio.Length - 512, bio.Length - 512) : bio;
        }

        private static string ReplaceKeyWord(string keyword, string bio, object obj)
        {
            s_index = bio[bio.IndexOf(keyword) + 2] == '˸' ? bio.IndexOf(keyword) + 3 : bio.IndexOf(keyword) + 4;
            for (int i = s_index; i < bio.Length; i++)
            {
                if (bio[i] != ';') continue;
                bio = bio.Remove(s_index, i - s_index);
                bio = bio.Replace(keyword, keyword +" " + obj.ToString());
                break;
            }
            return bio;
        }
        private static readonly string[] s_keyWords = new string[] { " Fo˸", " Of˸", " Ft˸", " U˸", " C˸", " R˸", " T˸", " W˸", " A˸", " Av˸" };
    }
}
