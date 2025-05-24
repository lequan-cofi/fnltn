using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    internal class UserSession
    {

        public static string Username { get; private set; }
        public static string Role { get; private set; } // Chính là 'phanquyen'
        public static bool IsLoggedIn { get; private set; }

        public static void Login(string username, string role)
        {
            Username = username;
            Role = role;
            IsLoggedIn = true;
        }

        public static void Logout()
        {
            Username = null;
            Role = null;
            IsLoggedIn = false;
        }
    }
}
