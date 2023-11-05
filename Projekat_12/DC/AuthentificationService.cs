using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC
{
    public class AuthentificationService : ICConnection
    {
        

        public bool AuthentificateClient(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, string> ServiceRequest(string service, string username)
        {
            throw new NotImplementedException();
        }
    }
}
