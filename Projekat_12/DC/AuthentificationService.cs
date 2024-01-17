using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            RegisteredUsers.Add("DESKTOP-VU574HQ\\pera", password);
            RegisteredUsers.Add("BUBALO-LAPTOP\\Bubalo", password);
            RegisteredUsers.Add("BUBALO-LAPTOP\\pera", password);
            RegisteredUsers.Add("NOBLICABOOK\\nikol", password);
            RegisteredUsers.Add("DESKTOP-VGLB1AL\\Djuradj", password);
            RegisteredUsers.Add("NOBLICABOOK\\wcfclient", password);

            UsersAccountForServer = new Dictionary<string, string>();
            UsersAccountForServer.Add("DESKTOP-VU574HQ\\Milenko", password);
            UsersAccountForServer.Add("DESKTOP-VU574HQ\\zika", password);
            UsersAccountForServer.Add("BUBALO-LAPTOP\\Bubalo", password);
            UsersAccountForServer.Add("BUBALO-LAPTOP\\zika", password);
            UsersAccountForServer.Add("NOBLICABOOK\\nikol", password);
            UsersAccountForServer.Add("NOBLICABOOK\\wcfservice", password);
            UsersAccountForServer.Add("DESKTOP-VGLB1AL\\Djuradj", password);

        }

        public bool AuthentificateClient(string username, string password)
        {
            if (!RegisteredUsers.ContainsKey(username))
            {
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Domain Controller";
                    log.WriteEntry($"Client {username} does not exist", EventLogEntryType.Information);
                }
                throw new Exception("Korisnik ne postoji.");
            }

            if (RegisteredUsers[username] == password)
            {
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Domain Controller";
                    log.WriteEntry($"Client {username} successfuly authentificated", EventLogEntryType.SuccessAudit);
                }
                return true;
            }
            else
            {
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Domain Controller";
                    log.WriteEntry($"Client {username} authentification failed with wrong password.", EventLogEntryType.FailureAudit);
                }
                return false;
            }
        }


         public bool AuthenticateServer(string IPAddr, string hostName, string port, string hashPassword, string username)
         {
             if (!UsersAccountForServer.ContainsKey(username))
             {
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Domain Controller";
                    log.WriteEntry($"Service {username} doesn't exist.", EventLogEntryType.Information);
                }
                throw new Exception("Servis ne postoji.");
             }

             if (UsersAccountForServer[username] == hashPassword)  ///	Ispravan password ==> true
             {
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Domain Controller";
                    log.WriteEntry($"Service {username} successfuly authentificated", EventLogEntryType.SuccessAudit);
                }
                return true;
             }
             else    /// Neispravan password ==> false
             {
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Domain Controller";
                    log.WriteEntry($"Service {username} authentification failed with wrong password.", EventLogEntryType.FailureAudit);
                }
                return false;
             }
         }

        

        public string GetUserPassword(string username)
        {
            return RegisteredUsers[username];
        }
    }
}
