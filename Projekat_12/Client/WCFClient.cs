using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WCFClient : ChannelFactory<IDataManagment>, IDisposable
    {
        IDataManagment factory;
        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            try
            {
                factory = CreateChannel();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

    }
}
