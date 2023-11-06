using System;
using System.Collections.Generic;
using System.Linq;
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
            string key = ReturnToOriginalKey(modifiedKey);
            if(key == Program.secretKey)
            {
                return "Komunikacija je uspesna";
            }
            else
            {
                return "Kominikacija nije uspesna";
            }
        }

        private string ReturnToOriginalKey(string modifiedKey)
        {
            string key = "";
            foreach (char l in modifiedKey)
            {
                char z = (char)(l - 'z');
                key += z;
            }
            return key;
        }
    }
}
