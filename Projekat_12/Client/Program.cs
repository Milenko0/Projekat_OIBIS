using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            string username = id.Name; 
            Console.WriteLine($"User: {username}");

            string address = "net.tcp://localhost:6004/AuthentificationService";
          
            string service = "DataManagmentServer";
            Tuple<string, string> serviceEndpointAndKey = null;
            string secretKey = null;
            using (ClientAuthentificator authenticator = new ClientAuthentificator(binding, new EndpointAddress(new Uri(address))))
            { 
                if(authenticator.Authenticate(username, "password"))
                {
                    serviceEndpointAndKey = authenticator.ServiceRequest(service, username);
                    if (serviceEndpointAndKey == null)
                    {
                        Console.WriteLine("Trazeni servis nije aktivan. Pritisni enter za kraj.");
                        Console.ReadLine();
                        return;
                    }
                    secretKey = serviceEndpointAndKey.Item2;
                    Console.WriteLine("Trazeni servis je aktivan!\nKlijent dobio tajni kljuc: " + secretKey);

                    using (WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(serviceEndpointAndKey.Item1))))
                    {
                        secretKey = "1111111";
                        if (proxy.Connect(secretKey))
                        {
                            char izbor;
                            string message = "";
                            bool close = false;
                            while (!close)
                            {
                                message = "";
                                Console.WriteLine();
                                Console.WriteLine("------OPCIJE------");
                                Console.WriteLine("1) Write");
                                Console.WriteLine("2) Read");
                                Console.WriteLine("3) Zatvaranje komunikacije");
                                Console.WriteLine("Unesite:");
                                izbor = Console.ReadKey().KeyChar;
                                Console.WriteLine();
                                switch (izbor)
                                {
                                    case '1':
                                        Console.WriteLine();
                                        //secretKey = "1111111";
                                        Console.WriteLine("Unesite poruku za server:");
                                        string text = Console.ReadLine();
                                        Tuple<bool, string> answerW = proxy.Write(secretKey, text);
                                        if (answerW.Item1)
                                        {

                                            Console.WriteLine("Odgovor servera: " + answerW.Item2);
                                        }
                                        break;
                                    case '2':
                                        Console.WriteLine();
                                        Tuple<bool, List<string>> answerR = proxy.Read(secretKey);
                                        if (answerR.Item1)
                                        {
                                            foreach (string s in answerR.Item2)
                                            {
                                                Console.WriteLine(s);
                                            }
                                        }
                                        break;
                                    case '3':
                                        using (EventLog log = new EventLog("Application"))
                                        {
                                            log.Source = "Client";
                                            log.WriteEntry($"Client Shut Down", EventLogEntryType.Information);
                                        }
                                        close = true;

                                        break;
                                    default:
                                        Console.WriteLine("Pogresan unos");
                                        break;
                                }
                            }
                        }
                        else { Console.WriteLine("Autentifikacija sa servisom je neuspesna");
                            
                        }
                    }
                        
                }
                else
                {
                    Console.WriteLine("Kredencijali nisu vazeci.");
                }
            }
            Console.ReadKey();
        }
    }
}
      