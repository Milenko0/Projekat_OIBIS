using System;
using System.Collections.Generic;
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
            List<string> retCipher = new List<string>();
            foreach(string s in ret)
            {
                retCipher.Add(Encrypting.EncryptMessage(s, Program.secretKey));
            }
            return retCipher;
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
                return "Uspesno upisana poruka";
            }catch(Exception e)
            {
                return e.Message;
            }
        }
        
        
    }
}
