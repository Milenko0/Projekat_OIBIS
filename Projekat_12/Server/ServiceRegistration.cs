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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void SingOut(string hostName)
        {
            try
            {
                factory.SignOutService(hostName);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
