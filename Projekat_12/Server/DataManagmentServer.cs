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
        private string Modify(string key)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(key);
            byte[] hash = null;
            SHA256Managed sha256 = new SHA256Managed();
            hash = sha256.ComputeHash(data);
            return Convert.ToBase64String(hash);
        }
        
    }
}
