using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC
{
    public class AuthentificationService
    {
        private Dictionary<string, string> RegisteredUsers;
        private Dictionary<string, string> UsersAccountForServer;

     

        public AuthentificationService()
        {
            string password = "password";

            RegisteredUsers = new Dictionary<string, string>();
            RegisteredUsers.Add("DESKTOP-VU574HQ\\Milenko", password);
            

            UsersAccountForServer = new Dictionary<string, string>();
            UsersAccountForServer.Add("DESKTOP-VU574HQ\\Milenko", password);
           
        }

        public bool AuthentificateClient(string username, string password)
        {
            if (!RegisteredUsers.ContainsKey(username))
            {
                Console.WriteLine("fsa");
                throw new Exception("Korisnik ne postoji.");
            }

            if (RegisteredUsers[username] == password)
            {

                return true;
            }
            else
            {

                return false;
            }
        }


         public bool AuthenticateServer(string IPAddr, string hostName, string port, string hashPassword, string username)
         {
             if (!UsersAccountForServer.ContainsKey(username))
             {

                 throw new Exception("No shuch user.");
             }

             if (UsersAccountForServer[username] == hashPassword)  ///	Ispravan password ==> true
             {

                 return true;
             }
             else    /// Neispravan password ==> false
             {

                 return false;
             }
         }

        

        public string GetUserPassword(string username)
        {
            return RegisteredUsers[username];
        }
    }
}
