using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Server
{
    public class DataManagmentServer : IDataManagment
    {
       public Tuple<string, string> Connection(string keyHashed, string salt)
        {
            if (Encrypting.Hash256(Program.secretKey, salt).Equals(keyHashed))
            {
                //Console.WriteLine(Program.secretKey);
                return new Tuple<string, string>(Encrypting.Hash256(Program.secretKey, "serversalt"), "serversalt");
            }
            else
                //Console.WriteLine("dfsaf");
                return null;
        }
        public List<string> Read()
        {
            var ret = SQliteDataAccess.ReadMessages();
            if (ret != null)
            {
                List<string> retCipher = new List<string>();
                foreach (string s in ret)
                {
                    retCipher.Add(Encrypting.EncryptMessage(s, Program.secretKey));
                }
                /*
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Servis";
                    log.WriteEntry($"Successfully fetched data from the database.", EventLogEntryType.SuccessAudit);
                }
                */
                return retCipher;

            }
            else
            {
                /*
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Servis";
                    log.WriteEntry($"Did not fatch any data from the database.", EventLogEntryType.Error);
                }
                */
                return null;
            }
        }
        public string Write(string ciptherText) 
        {
            /*
            string key = Modify(Program.secretKey);
            if(modifiedKey == key)
            {
                return "Komunikacija je uspesna";
            }
            else
            {
                return "Kominikacija nije uspesna";
            }*/
            try
            {
                
                string text = Encrypting.DecryptMessage(ciptherText, Program.secretKey);
                SQliteDataAccess.WriteMessage(text);
                /*
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Servis";
                    log.WriteEntry($"Successfully inserted data into the database.", EventLogEntryType.SuccessAudit);
                }
                */
                return "Uspesno upisana poruka";
            }catch(Exception e)
            {
                /*
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Servis";
                    log.WriteEntry($"Did not write any data into the database.", EventLogEntryType.Error);
                }
                */
                return e.Message;
            }
        }
        
        
    }
}
