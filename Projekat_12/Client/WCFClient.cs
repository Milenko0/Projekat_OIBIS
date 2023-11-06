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
            string returnString = "";
            foreach(char l in key)
            {
                char z = (char)(l + 'z');
                //Console.WriteLine(z);
                returnString += z;
            }
            //Console.WriteLine(returnString);
            return returnString;
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
