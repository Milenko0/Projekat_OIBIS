using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ISConnection
    {
        [OperationContract]
        bool RegisterService(string IPAddr, string hostName, string port, string hashPassword, string username);
        [OperationContract]
        void SignOutService(string hostName);
    }
}
