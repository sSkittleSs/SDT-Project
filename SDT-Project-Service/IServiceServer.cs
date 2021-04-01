using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SDT_Project.AdditionalStructures;

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
        uint Registering(string userName, string userPassword, string email);

        [OperationContract]
        string GetUserData(uint id, DataTypes dataType = DataTypes.UserData);

        [OperationContract]
        string GetEmail(uint id);

        [OperationContract]
        TravelCard GetCard(uint id);

        [OperationContract]
        Transaction GetLastTransaction(uint id);

        [OperationContract]
        bool PayForTheTrip(uint cardId, int newBalance);

        [OperationContract]
        bool ChangeUsername(uint userId, string newUsername);

        [OperationContract]
        bool ChangeEmail(uint userId, string newEmail);

        [OperationContract]
        bool BuyCard(uint userId, double sum, CardTypes type, CardCategories category, int balance);

        [OperationContract]
        bool ExtendCard(uint cardId, int newBalance, DateTime newDate);

        [OperationContract(IsOneWay = true)]
        void ConsoleLog(string msg, MessageImportance importance = 0, ConsoleColor foregroundColor = ConsoleColor.Black);

        [OperationContract]
        int GetUserType(uint id);

        [OperationContract]
        string SendConfirmationLetter(string email);

        [OperationContract(IsOneWay = true)]
        void SendLetter(string email, string body, string subject = "Notification", bool isBodyHtml = false);
    }

    public interface IServiceDataCallback
    {
        [OperationContract(IsOneWay = true)]
        void DataCallback(string data);

    }
}
