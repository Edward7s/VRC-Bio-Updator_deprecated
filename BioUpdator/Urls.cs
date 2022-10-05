using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioUpdator
{
    internal class Urls
    {
        public static string VRCApiLink { get; } = "https://vrchat.com/api/1/";
        public static string UserEndPoint { get; } = "users/"; /* Need UserId At The End */
        public static string LocalUser { get; } = "auth/user"; 
        public static string FriendsOnline { get; } = "users/friends?offline=false&n=50&offset=0";
        public static string Worlds { get; } = "worlds/"; /* WorldId */
        public static string Instance { get; } = "instances/"; /* InstanceId */
        public static string Avatar { get; } = "avatars/"; /* AvatarId */

    }
}
