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
        public void Read()
        {

        }
        public string Write(string modifiedKey) 
        {
            string key = Modify(Program.secretKey);
            if(modifiedKey == key)
            {
                return "Komunikacija je uspesna";
            }
            else
            {
                return "Kominikacija nije uspesna";
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
