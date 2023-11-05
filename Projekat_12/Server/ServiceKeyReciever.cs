using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ServiceKeyReciever : IKeySender
    {
        public void SendKey(string key)
        {
            Program.secretKey = key;
        }
    }
}
