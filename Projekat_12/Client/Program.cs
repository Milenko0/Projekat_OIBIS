using Common;
using System;
using System.Collections.Generic;
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
                    Console.WriteLine("Trazeni servis je aktivan!\nKlijent dobio tajni kljuc: " + secretKey);

                    using (WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(serviceEndpointAndKey.Item1))))
                    {
                        char izbor;
                        string message = "";
                        bool close = false;
                        while (!close)
                        {
                            message = "";
                            Console.WriteLine();
                            Console.WriteLine("------Izaberi------");
                            Console.WriteLine("1 -> Slanje poruke (Write)");
                            Console.WriteLine("2 -> Prijem poruke (Read)");
                            Console.WriteLine("3 -> Zatvaranje komunikacije");
                            Console.WriteLine("Unesite:");
                            izbor = Console.ReadKey().KeyChar;
                            Console.WriteLine();
                            switch (izbor)
                            {
                                case '1':
                                    Console.WriteLine("Unesite poruku za slanje:");
                                    message = Console.ReadLine();
                                    Console.WriteLine();
                                    if (!message.Trim().Equals(""))
                                    {
                                        //if (proxy.Write(message, secretKey))
                                        {
                                            Console.WriteLine($"Poruka '{message}' je uspesno poslata. ");
                                        }
                                    }
                                    else
                                        Console.WriteLine("Poruka za slanje nije uneta");
                                    break;
                                case '2':
                                    Console.WriteLine();
                                    //string primljenaPoruka = proxy.Read(secretKey);
                                    string primljenaPoruka = "";
                                    if (primljenaPoruka.Trim().Equals(""))
                                    {
                                        Console.WriteLine("Nema podataka na serveru");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Server Poslao: " + primljenaPoruka);
                                    }
                                    break;
                                case '3':
                                    close = true;
                                 
                                    break;
                                default:
                                    Console.WriteLine("Pogresan unos");
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Kredencijali nisu vazeci.");
                }
            }
        }
    }
}
      