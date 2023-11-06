using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace DC
{
    public class ServiceModel
    {
        public string IPAddress { get; set; }
        public string HostName { get; set; }
        public string Port { get; set; }
        public string ServicePassword { get; set; }
        public EndpointIdentity ServiceIdentity { get; set; }

        public ServiceModel(string ipAddress, string hostName, string port, string servicePassword, EndpointIdentity serviceIdentity)
        {
            IPAddress = ipAddress;
            HostName = hostName;
            Port = port;
            ServicePassword = servicePassword;
            ServiceIdentity = serviceIdentity;
        }
    }
}
