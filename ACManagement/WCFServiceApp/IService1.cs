using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using ObjectData;

namespace WCFServiceApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        DataSet GetUserInfo();

        [OperationContract]
        string InsertAccountInfo(AccountInfo acInfo);

        [OperationContract]
        string UpdateAccountInfo(AccountInfo acInfo);

        [OperationContract]
        DataSet SelectAccountInfo();

        [OperationContract]
        string DeleteAccountInfo(Int32 idInfo);
    }
}
