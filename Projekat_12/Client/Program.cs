using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IDataManagment> channel = new ChannelFactory<IDataManagment>("ServiceName");

            IDataManagment proxy = channel.CreateChannel();

            bool authentificated = Authentificate();
            if (authentificated)
            {
                string key = RecievePrivateKey();
                Console.WriteLine($"Primljen tajni kljuc od servisa.");
            }
            Console.ReadKey();

           
            
        }

        private static bool Authentificate()
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding();
                string address = "net.tcp://localhost:9999/AuthentificationService";

                binding.Security.Mode = SecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

                ChannelFactory<IAuthentificationService> authServiceFactory = new ChannelFactory<IAuthentificationService>(binding, address);
                IAuthentificationService authentificationService = authServiceFactory.CreateChannel();
                return authentificationService.AuthentificateClient();
                
            }catch(Exception e)
            {
                Console.WriteLine("Did not Authentificate");
                return false;
            }
        }

        private static string RecievePrivateKey()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:8888/TicketGrantingService";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ChannelFactory<ITicketGrantingService> tgsFactory = new ChannelFactory<ITicketGrantingService>(binding, address);

            ITicketGrantingService TGS = tgsFactory.CreateChannel();

            string key = TGS.GeneratePrivateString();

            return key;
        }
    }
}
