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
        List<ServerUser> users = new List<ServerUser>();
        public uint Connect(string userName, string userPassword)
        {
            StringBuilder strings = new StringBuilder();
            ConsoleColor consoleColor = ConsoleColor.White;
            connection.Open();

            string sql = $"SELECT name, password, id, usertype FROM users WHERE name = '{userName}' and password = '{userPassword}'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command?.ExecuteReader();
            uint result = 0;
            if (reader.HasRows && reader.Read())
            {
                UInt32 id = reader.GetUInt32(2);
                UInt32 type = reader.GetUInt32(3);
                strings.AppendLine($"Результат запроса к БД:\nName: {reader.GetString(0) ?? "Null"}\nPassword: {reader.GetString(1) ?? "Null"}\n" +
                                   $"Id: {id}\nUserType: {GetUserTypeInString(type)}");

                ServerUser user = new ServerUser()
                {
                    Id = id,
                    Name = userName,
                    Password = userPassword,
                    UserType = (UserTypes)type,
                    OperationContext = OperationContext.Current
                };

                consoleColor = ConsoleColor.Green;
                users.Add(user);
                strings.AppendLine($"Пользователь {user.Name} был подключен к серверу с идентификатором {user.Id} и правами {GetUserTypeInString(type)}'а.");
                result = user.Id;
            }
            else
            {
                consoleColor = ConsoleColor.DarkYellow;
                strings.AppendLine($"Пользователь {userName} не был подключен к серверу, поскольку запрос ничего не вернул.");
            }

            reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            return result;
        }

        public void Disconnect(uint id)
        {
            var user = users.FirstOrDefault(i => i.Id == id);
            if (user != null)
            {
                users.Remove(user);
                ConsoleLog($"Пользователь {user.Name} с идентификатором {user.Id} был отключен от сервера.", MessageImportance.SysInfo);
            }
        }

        public uint Registering(string userName, string userPassword)
        {
            StringBuilder strings = new StringBuilder();

            connection.Open();
            ConsoleColor consoleColor = ConsoleColor.White;

            string sql = $"SELECT name, password, id, usertype FROM users WHERE name = '{userName}' and password = '{userPassword}'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command?.ExecuteReader();
            uint result = 0;
            if (reader.HasRows && reader.Read())
            {
                UInt32 id = reader.GetUInt32(2);
                UInt32 type = reader.GetUInt32(3);
                strings.AppendLine($"Результат запроса к БД:\nName: {reader.GetString(0) ?? "Null"}\nPassword: {reader.GetString(1) ?? "Null"}\n" +
                                   $"Id: {id}\nUserType: {GetUserTypeInString(type)}");
                strings.AppendLine($"Пользователь {reader.GetString(0)} уже зарегистрирован под идентификатором {id}.");
            }
            else
            {
                sql = $"INSERT INTO `users` (`name`, `password`, `usertype`, `cardid`, `id`) VALUES ('{userName}', '{userPassword}', '0', NULL, NULL)";
                reader.Close();
                command = new MySqlCommand(sql, connection);
                int? rows = command?.ExecuteNonQuery();

                if (rows != null)
                {
                    strings.Append($"Пользователь {userName} был зарегистрирован");
                    sql = $"SELECT id FROM users WHERE name = '{userName}' and password = '{userPassword}'";

                    command = new MySqlCommand(sql, connection);
                    reader = command?.ExecuteReader();

                    if (reader.HasRows && reader.Read())
                    {
                        UInt32 id = reader.GetUInt32(0);
                        strings.Append($"под идентификатором {id}.");
                        result = id;
                    }
                    else
                    {
                        strings.Append($".");
                    }
                    consoleColor = ConsoleColor.Green;
                }
                else
                {
                    consoleColor = ConsoleColor.DarkYellow;
                    strings.AppendLine($"Пользователь {userName} не был зарегистрирован.");
                }
            }

            reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            return result;
        }

        public string GetUserData(uint id, DataTypes dataType = DataTypes.UserData)
        {
            throw new NotImplementedException();
        }

        public void ConsoleLog(string msg, MessageImportance importance = 0, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.ForegroundColor = foregroundColor;
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
