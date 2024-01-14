using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DC
{
    public class Program
    {
        static void Main(string[] args)
        {
            string address = "net.tcp://localhost:6004/AuthentificationService";
            NetTcpBinding binding = new NetTcpBinding();

            ServiceHost host = new ServiceHost(typeof(DomainController));
            host.AddServiceEndpoint(typeof(ICConnection), binding, address);

            host.Open();
            Console.WriteLine("DC je pokrenut.");


            string addressForServer = "net.tcp://localhost:6001/ServiceRegistration";
            NetTcpBinding bindingForServer = new NetTcpBinding();
            
            ServiceHost hostForServer = new ServiceHost(typeof(DomainController));
            hostForServer.AddServiceEndpoint(typeof(ISConnection), bindingForServer, addressForServer);
            
            hostForServer.Open();
            

            Console.WriteLine("ServiceConnection host je pokrenut.");
            Console.ReadLine();

            host.Close();
        }
    }
}
