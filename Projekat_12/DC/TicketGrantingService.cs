using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DC
{
    public class TicketGrantingService : ITicketGrantingService
    {
        public string GeneratePrivateString()
        {
            string key = "Tajni kljuc";
            Console.WriteLine("Klijentu Poslat tajni kljuc");
            return key;
        }
    }
}
