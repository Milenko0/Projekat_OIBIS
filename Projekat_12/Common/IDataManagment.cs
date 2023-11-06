using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    [ServiceContract]
    public interface IDataManagment
    {
        [OperationContract]
        void Read();
        [OperationContract]
        string Write(string modifiedKey);
    }
}
