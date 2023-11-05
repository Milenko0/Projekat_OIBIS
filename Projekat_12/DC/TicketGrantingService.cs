using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DC
{
    public class TicketGrantingService : ChannelFactory<IKeySender>, IDisposable
    {
        IKeySender factory;

        public TicketGrantingService(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = CreateChannel();
        }

        public void SendKey(string key)
        {
            factory.SendKey(key);
        }
      
    }
}
