using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Domain Controller";
                    log.WriteEntry($"Service '{serviceName}' recieved secret key.", EventLogEntryType.Information);
                }
                return new Tuple<string, string>(hostEndpoint, key);

            }
            else
            {
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Domain Controller";
                    log.WriteEntry($"Service '{serviceName}' endpoint doesn't exist.", EventLogEntryType.FailureAudit);
                }

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
            Console.WriteLine("Servis {0} je deaktiviran.", hostName);
            using (EventLog log = new EventLog("Application"))
            {
                log.Source = "Domain Controller";
                log.WriteEntry($"Service '{hostName}' signed out.", EventLogEntryType.Information);
            }

        }

        public void RegisterService(string IPAddr, string hostName, string port, string hashPassword)
        {
            if (activeServices.ContainsKey(hostName))
            {
                Console.WriteLine($"Servis {hostName} je vec aktivan.");

                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Domain Controller";
                    log.WriteEntry($"Service '{hostName}' can't be registered because it already exists.", EventLogEntryType.FailureAudit);
                }
                throw new Exception("Service already registered.");
                
            }

            EndpointAddress endpointAdress = new EndpointAddress(new Uri("net.tcp://localhost:" + port + "/" + hostName), EndpointIdentity.CreateUpnIdentity("admin@w7ent"));
            activeServices.Add(hostName, new ServiceModel(IPAddr, hostName, port, hashPassword, endpointAdress.Identity));
            dnsTable.Add(IPAddr, hostName);
            Console.WriteLine("Servis {0} je aktiviran.", hostName);
            using (EventLog log = new EventLog("Application"))
            {
                log.Source = "Domain Controller";
                log.WriteEntry($"Service '{hostName}' successfully registered.", EventLogEntryType.SuccessAudit);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

      
