using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ServiceRegistration : ChannelFactory<ISConnection>, IDisposable
    {
        ISConnection factory;

        public ServiceRegistration(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = CreateChannel();
        }

        public void Register(string IPAddr, string hostName, string port, string hashPassword, string username)
        {
            try
            {
                factory.RegisterService(IPAddr, hostName, port, hashPassword, username);
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Servis";
                    log.WriteEntry($"Service {hostName} registered successfully.", EventLogEntryType.SuccessAudit);
                }
            }
            catch (Exception e)
            {
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Servis";
                    log.WriteEntry($"Service {hostName} failed to register.", EventLogEntryType.FailureAudit);
                }
                Console.WriteLine(e.Message);
            }
        }

        public void SingOut(string hostName)
        {
            try
            {
                factory.SignOutService(hostName);
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Servis";
                    log.WriteEntry($"Service {hostName} signed out successfully.", EventLogEntryType.Information);
                }

            }
            catch (Exception e)
            {
                using (EventLog log = new EventLog("Application"))
                {
                    log.Source = "Servis";
                    log.WriteEntry($"Service {hostName} failed to sign out.", EventLogEntryType.FailureAudit);
                }
                Console.WriteLine(e.Message);
            }
        }
    }
}
