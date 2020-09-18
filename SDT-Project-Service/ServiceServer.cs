using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SDT_Project_Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceServer : IServiceServer
    {
        List<ServerUser> Users = new List<ServerUser>();
        int NextId = 1;
        public int Connect(string userName, string userPassword)
        {
            // TODO: Добавить запрос на поиск пользователя в БД.

            ServerUser user = new ServerUser()
            {
                Id = NextId++,
                Name = userName,
                Password = userPassword,
                UserType = UserTypes.User,
                operationContext = OperationContext.Current
            };

            Users.Add(user);
            ConsoleLog($"Пользователь {user.Name} был подключен к серверу с идентификатором {user.Id}.", MessageImportance.SysInfo);

            return user.Id;
        }

        public void Disconnect(int id)
        {
            var user = Users.FirstOrDefault(i => i.Id == id);
            if (user != null)
            {
                Users.Remove(user);
                ConsoleLog($"Пользователь {user.Name} с идентификатором {user.Id} был отключен от сервера.", MessageImportance.SysInfo);
            }
        }

        public int Registering()
        {
            throw new NotImplementedException();
        }

        public string GetUserData(int id, DataTypes dataType = DataTypes.UserData)
        {
            throw new NotImplementedException();
        }

        public void ConsoleLog(string msg, MessageImportance importance = 0)
        {
            switch (importance)
            {
                case MessageImportance.Common:
                    {
                        Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {msg}");

                        break;
                    }
                case MessageImportance.Important:
                    {
                        Console.WriteLine($"!!!   {DateTime.Now.ToShortTimeString()}: {msg}   !!!");
                        break;
                    }
                case MessageImportance.SysInfo:
                    {
                        Console.WriteLine($"System information:\n{DateTime.Now.ToShortTimeString()}: {msg}");
                        break;
                    }
            }
        }
    }
}
