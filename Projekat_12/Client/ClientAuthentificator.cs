using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientAuthentificator  : ChannelFactory<ICConnection>, IDisposable
    {
        ICConnection factory;
        public ClientAuthentificator(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = CreateChannel();
        }

        public Tuple<string, string> ServiceRequest(string service, string username)
        {
            return factory.ServiceRequest(service, username);
        }
        public bool Authenticate(string username, string password)
        {
            bool ret = false;
            try
            {
                ret = factory.AuthentificateClient(username, password);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Authenticate] Korisnik ne postoji");
            }
            return ret;
        }
    }
}
