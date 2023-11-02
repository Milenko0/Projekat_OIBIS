using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC
{
    public class AuthentificationService : IAuthentificationService
    {
        public bool AuthentificateClient()
        {
            Console.WriteLine("Klijent se uspesno povezao na servis autentifikacije");

            return true;
        }
    }
}
