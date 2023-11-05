using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DC
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Authentification Service setup
            NetTcpBinding bindingAS = new NetTcpBinding();
            string addressAS = "net.tcp://localhost:9999/AuthentificationService";
            bindingAS.Security.Mode = SecurityMode.Transport;
            bindingAS.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingAS.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost hostAS = new ServiceHost(typeof(AuthentificationService));
            hostAS.AddServiceEndpoint(typeof(ICConnection), bindingAS, addressAS);

            //Ticket Granting Service setup
            NetTcpBinding bindingTGS = new NetTcpBinding();
            string addressTGS = "net.tcp://localhost:8888/TicketGrantingService";
            bindingTGS.Security.Mode = SecurityMode.Transport;
            bindingTGS.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingTGS.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost hostTGS = new ServiceHost(typeof(TicketGrantingService));
            hostTGS.AddServiceEndpoint(typeof(ITicketGrantingService), bindingTGS, addressTGS);


            //Prihvata klijenta prvo
            hostAS.Open();

           

            
            //Salje klijentu tajni kljuc
            hostTGS.Open();
            
            //Ovdje treba da posalje servisu tajni kljuc isto

            Console.ReadKey();
        }
    }
}
