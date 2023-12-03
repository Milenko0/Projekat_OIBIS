using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.ServiceModel;
using System.ServiceModel.Channels;
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

        public Tuple<bool, string> Write(string key)
        {
            try
            {
                string modifiedKey = Modify(key);
                string answer = factory.Write(modifiedKey);
                return Tuple.Create(true, answer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Tuple.Create(false, e.Message);
            }
        }

        private string Modify(string key)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(key);
            byte[] hash = null;
            SHA256Managed sha256 = new SHA256Managed();
            hash = sha256.ComputeHash(data);
            return Convert.ToBase64String(hash);
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
