using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        public static string secretKey;
        static void Main(string[] args)
        {

            WindowsIdentity id = WindowsIdentity.GetCurrent();
            NetTcpBinding binding = new NetTcpBinding();
            secretKey = null;

            string serviceIP = "net.tcp://localhost";
            string clientPort = "6000";
            string clientService = "DataManagmentServer";
            string connectionPort = "6007";
            string keyReciverService = "ServiceKeyReciver";

            string serviceEndpoint = serviceIP + ":" + clientPort + "/" + clientService;
            string keyRecieverEndpoint = serviceIP + ":" + connectionPort + "/" + keyReciverService;

            ServiceHost clientHost = new ServiceHost(typeof(DataManagmentServer));
            clientHost.AddServiceEndpoint(typeof(IDataManagment), binding, serviceEndpoint);



            clientHost.Open();
            Console.WriteLine("Servis je pokrenut.");

            using (ServiceRegistration proxy = new ServiceRegistration(binding, new EndpointAddress(new Uri("net.tcp://localhost:6001/ServiceRegistration"))))
            {
                proxy.Register(serviceIP + clientPort, clientService, clientPort, "password", id.Name);
            }

            ServiceHost connectionHost = new ServiceHost(typeof(ServiceKeyReciever));
            connectionHost.AddServiceEndpoint(typeof(IKeySender), binding, keyRecieverEndpoint);
            connectionHost.Open();
            Console.WriteLine("Servis za primanje kljuca je pokrenut.");

            while (secretKey == null) System.Threading.Thread.Sleep(50);
            Console.WriteLine("Privatni kljuc" + secretKey);

            var enMessage = Encrypting.EncryptMessage("HELLO WORLD", secretKey);
            var deMEssage = Encrypting.DecryptMessage(enMessage, secretKey);

            Console.WriteLine(deMEssage + " " + enMessage);
            

            Console.WriteLine("Izadji sa ENTER");
            Console.ReadKey();

            using (ServiceRegistration proxy = new ServiceRegistration(binding, new EndpointAddress(new Uri("net.tcp://localhost:6001/ServiceRegistration"))))
            {
                proxy.SingOut(clientService);
            }

            clientHost.Close();
            connectionHost.Close();
        }
    }
}
