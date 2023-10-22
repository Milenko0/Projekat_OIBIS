using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IDataManagment> channel = new ChannelFactory<IDataManagment>("ServiceName");

            IDataManagment proxy = channel.CreateChannel();
            Console.ReadKey();
        }
    }
}
