using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SDT_Project_Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class ServiceServer : IServiceServer
    {
        MySqlConnection connection = new MySqlConnection("server = localhost; user = root; database = sdt-project; password = root;");
        List<ServerUser> Users = new List<ServerUser>();
        uint NextId = 1;
        public uint Connect(string userName, string userPassword)
        {
            // TODO: Добавить запрос на поиск пользователя в БД.
            StringBuilder strings = new StringBuilder();

            connection.Open();

            string sql = $"SELECT name, password, id, usertype FROM users WHERE name = '{userName}' and password = '{userPassword}'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command?.ExecuteReader();
            uint result = 0;
            if (reader.Read())
            {
                UInt32 id = reader.GetUInt32(2);
                UInt32 type = reader.GetUInt32(3);
                strings.AppendLine($"Результат запроса к БД:\nName: {reader.GetString(0) ?? "Null"}\nPassword: {reader.GetString(1) ?? "Null"}\n" +
                                   $"Id: {id}\n UserType: {GetUserTypeInString(type)}");
                connection.Close();

                ServerUser user = new ServerUser()
                {
                    Id = id,
                    Name = userName,
                    Password = userPassword,
                    UserType = (UserTypes)type,
                    operationContext = OperationContext.Current
                };

                Users.Add(user);
                strings.AppendLine($"Пользователь {user.Name} был подключен к серверу с идентификатором {user.Id}.");
                result = user.Id;
            }
            else
            {
                strings.AppendLine($"Пользователь {userName} не был подключен к серверу, поскольку запрос вернул null.");
            }

            reader.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo);
            return result;
        }

        public void Disconnect(uint id)
        {
            var user = Users.FirstOrDefault(i => i.Id == id);
            if (user != null)
            {
                Users.Remove(user);
                ConsoleLog($"Пользователь {user.Name} с идентификатором {user.Id} был отключен от сервера.", MessageImportance.SysInfo);
            }
        }

        public uint Registering()
        {
            throw new NotImplementedException();
        }

        public string GetUserData(uint id, DataTypes dataType = DataTypes.UserData)
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
                        Console.WriteLine($"\tSystem notification:" +
                            $"\n________________________________________________\n\n" +
                            $"{DateTime.Now.ToShortTimeString()}: {msg}" +
                            $"\n________________________________________________\n");
                        break;
                    }
            }
        }

        private string GetUserTypeInString(uint typeID)
        {
            switch (typeID)
            {
                case 0:
                    return "User";
                case 1:
                    return "Accountant";
                case 2:
                    return "Admin";
                case 3:
                    return "SysAdmin";
                default:
                    return "Undefined";
            }
        }
    }
}
