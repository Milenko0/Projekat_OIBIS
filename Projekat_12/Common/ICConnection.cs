using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ICConnection
    {
        [OperationContract]
        bool ValidateUser(string username, string password);
        [OperationContract]
        Tuple<string, string> ServiceRequest(string service, string username);
    }
}
