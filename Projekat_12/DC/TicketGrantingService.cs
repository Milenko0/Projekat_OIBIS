using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DC
{
    public class TicketGrantingService : IDisposable
    {
        IKeySender factory;
        private static Dictionary<string, string> dnsTable;

        private static Dictionary<string, ServiceModel> activeServices;

        private static Dictionary<string, string> createdSecretKeys;

        public TicketGrantingService(NetTcpBinding binding, EndpointAddress address)
        {
            //factory = CreateChannel();
            if (activeServices == null) activeServices = new Dictionary<string, ServiceModel>();
            if (createdSecretKeys == null) createdSecretKeys = new Dictionary<string, string>();
            if (dnsTable == null) dnsTable = new Dictionary<string, string>();
        }

        public void SendKey(string key)
        {
            factory.SendKey(key);
        }

        public TicketGrantingService()
        {
            factory = new ChannelFactory<IKeySender>(new NetTcpBinding(), new EndpointAddress(new Uri("net.tcp://localhost:6007/ServiceKeyReciver"))).CreateChannel();

            if (activeServices == null) activeServices = new Dictionary<string, ServiceModel>();
            if (createdSecretKeys == null) createdSecretKeys = new Dictionary<string, string>();
            if (dnsTable == null) dnsTable = new Dictionary<string, string>();
        
         }

        public Tuple<string, string> GetServiceEndpointAndSecretKey(string serviceName)
        {
            if (activeServices.ContainsKey(serviceName))
            {
                string hostEndpoint = "net.tcp://localhost:" + activeServices[serviceName].Port + "/" + activeServices[serviceName].HostName;
                string key = GenerateKey();
                //TicketGrantingService service = new TicketGrantingService(new NetTcpBinding(), new EndpointAddress(new Uri("net.tcp://localhost:6004/ServiceKeyReciver")));
               // factory = new ChannelFactory<IKeySender>(new NetTcpBinding(), new EndpointAddress(new Uri("net.tcp://localhost:6004/ServiceKeyReciver"))).CreateChannel();
                 this.SendKey(key);
                return new Tuple<string, string>(hostEndpoint, key);

            }
            else
            {

                Console.WriteLine("adsa");
            return null;
             }
        }

        private string GenerateKey()
        {
            SymmetricAlgorithm sa = TripleDESCryptoServiceProvider.Create();

            return ASCIIEncoding.ASCII.GetString(sa.Key);
        }

        public void SignOutService(string hostName)
        {
            activeServices.Remove(hostName);
            Console.WriteLine("Servis {0} is deactivated.", hostName);
           
        }

        public void RegisterService(string IPAddr, string hostName, string port, string hashPassword)
        {
            if (activeServices.ContainsKey(hostName))
            {
                Console.WriteLine($"Service {hostName} already exists.");
               
                throw new Exception("Service already registered.");
            }

            EndpointAddress endpointAdress = new EndpointAddress(new Uri("net.tcp://localhost:" + port + "/" + hostName), EndpointIdentity.CreateUpnIdentity("admin@w7ent"));
            activeServices.Add(hostName, new ServiceModel(IPAddr, hostName, port, hashPassword, endpointAdress.Identity));
            dnsTable.Add(IPAddr, hostName);
            Console.WriteLine("Servis {0} is activated.", hostName);
           
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

      
