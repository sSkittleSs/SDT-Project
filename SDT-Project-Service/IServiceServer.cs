using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SDT_Project_Service
{
    public enum MessageImportance { Common = 0, Important = 1, SysInfo = 2 }
    public enum DataTypes { UserData = 0 } // TODO: Добавить типы данных

    [ServiceContract(CallbackContract = typeof(IServiceDataCallback))]
    public interface IServiceServer
    {
        [OperationContract]
        uint Connect(string userName, string userPassword);

        [OperationContract(IsOneWay = true)]
        void Disconnect(uint id);

        [OperationContract]
        uint Registering(string userName, string userPassword);

        [OperationContract]
        string GetUserData(uint id, DataTypes dataType = DataTypes.UserData);

        [OperationContract(IsOneWay = true)]
        void ConsoleLog(string msg, MessageImportance importance = 0, ConsoleColor foregroundColor = ConsoleColor.Black);

    }

    public interface IServiceDataCallback
    {
        [OperationContract(IsOneWay = true)]
        void DataCallback(string data);

    }
}
