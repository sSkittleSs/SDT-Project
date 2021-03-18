using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

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

        public uint Registering(string userName, string userPassword, string email)
        {
            StringBuilder strings = new StringBuilder();

            connection.Open();
            ConsoleColor consoleColor = ConsoleColor.White;

            #region someRequestToDB
            //string sql = $"SELECT name, password, id, usertype FROM users WHERE name = '{userName}' and password = '{userPassword}'";
            //MySqlCommand command = new MySqlCommand(sql, connection);
            //MySqlDataReader reader = command?.ExecuteReader();
            //uint result = 0;
            //if (reader.HasRows && reader.Read())
            //{
            //    UInt32 id = reader.GetUInt32(2);
            //    UInt32 type = reader.GetUInt32(3);
            //    strings.AppendLine($"Результат запроса к БД:\nName: {reader.GetString(0) ?? "Null"}\nPassword: {reader.GetString(1) ?? "Null"}\n" +
            //                       $"Id: {id}\nUserType: {GetUserTypeInString(type)}");
            //    strings.AppendLine($"Пользователь {reader.GetString(0)} уже зарегистрирован под идентификатором {id}.");
            //}
            //else
            //{
            //    sql = $"INSERT INTO `users` (`name`, `password`, `usertype`, `cardid`, `id`) VALUES ('{userName}', '{userPassword}', '0', NULL, NULL)";
            //    reader.Close();
            //    command = new MySqlCommand(sql, connection);
            //    int? rows = command?.ExecuteNonQuery();

            //    if (rows != null)
            //    {
            //        strings.Append($"Пользователь {userName} был зарегистрирован");
            //        sql = $"SELECT id FROM users WHERE name = '{userName}' and password = '{userPassword}'";

            //        command = new MySqlCommand(sql, connection);
            //        reader = command?.ExecuteReader();

            //        if (reader.HasRows && reader.Read())
            //        {
            //            UInt32 id = reader.GetUInt32(0);
            //            strings.Append($"под идентификатором {id}.");
            //            result = id;
            //        }
            //        else
            //        {
            //            strings.Append($".");
            //        }
            //        consoleColor = ConsoleColor.Green;
            //    }
            //    else
            //    {
            //        consoleColor = ConsoleColor.DarkYellow;
            //        strings.AppendLine($"Пользователь {userName} не был зарегистрирован.");
            //    }
            //}
            #endregion

            string code = SendConfirmationLetter(email);

            //reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            //return result;
            return 0;
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

        public string SendConfirmationLetter(string email)
        {
            string code = "";
            Random rand = new Random();
            for (int i = 0; i < 8; i++)
                code += rand.Next(2) != 0 ? (char)(rand.Next(60, 90)) : (char)(rand.Next(48, 57));

            string subject = "Registering your account (SADT-Project)";
            string body = $"<table align=\"center\" border=\"0\" cellpadding=\"1\" cellspacing=\"1\" style=\"border-collapse:collapse;width:621px;color:#ffffff;background:#000000\"> <tbody> <tr> <td align=center> <img alt=\"\" src=\"https://i.ibb.co/CW7NP9N/logo-transp-white.png\" class=\"CToWUd a6T\" tabindex=\"0\"> </td> </tr> <tr> <td style=\"padding:0 40px 80px;font-size:10pt;color:#fef7db;font-family:tahoma,geneva,sans-serif;background:#080705;\"> <p style=\"color:#fef7db\">Код подтверждения:</p><p style=\"color:#fef7db\"><b>{code}</b></p><p style=\"color:#fef7db\">Если вы не совершали данного действия, пожалуйста, смените пароль. Мы также рекомендуем вам сменить и пароль от e-mail.</p> </td> </tr> <tr> <td style=\"padding:40px;font-size:10pt;color:rgb(254,247,219);height:65px;vertical-align:bottom;background: #090805;\"> <div style=\"color:#fef7db;float:right\"> <!-- <a href=\"https://www.facebook.com/escapefromtarkov/\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.facebook.com/escapefromtarkov/&amp;source=gmail&amp;ust=1614890131806000&amp;usg=AFQjCNFV5BRZPLAE60tQY57gCG4nKnGbMg\"><img alt=\"\" height=\"30\" src=\"https://ci3.googleusercontent.com/proxy/PE4SOtb0Rm1BbaUwc6uu5-AaohCGDFXexlahZcT6bJzkEhL1gA545RJRtHEXF2AO8JyeY-EwySa2lLEMV6ER-sTkxQwyw4SQnk1GFR2rew=s0-d-e1-ft#https://www.escapefromtarkov.com/uploads/mailtemplate/fb.png\" width=\"30\" class=\"CToWUd\"></a> --> <!-- <a href=\"https://vk.com/e.bushmakin\" target=\"_blank\" data-saferedirecturl=\"https://goo-gl.ru/w5Wcl\"><img alt=\"\" height=\"30\" src=\"https://ci4.googleusercontent.com/proxy/EUjy-REZau-rSvLl2SErIKOZ8Khr0aNwroxKkNVisPDb3tkwGkXs4RxAD6HinuCdbDXQ94A9TMSxPMrx55-QH8RLuSmfhbZDcHYz0--tcA=s0-d-e1-ft#https://www.escapefromtarkov.com/uploads/mailtemplate/vk.png\" width=\"30\" class=\"CToWUd\"></a> --> <!-- <a href=\"https://twitter.com/bstategames\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://twitter.com/bstategames&amp;source=gmail&amp;ust=1614890131807000&amp;usg=AFQjCNG_VmJG1CYAABsASY9vMtOGU7KiUg\"><img alt=\"\" height=\"30\" src=\"https://ci4.googleusercontent.com/proxy/ECIEaOCVKWhigpU-uKOhWPcWH9kmwo8tHFLHX1N7sg3qD4B0K041v3NAd0KDJ2fzagRb8vDZ_q1SNPHL7Njo_XhtRxG7vevYdCj4C7mlsg=s0-d-e1-ft#https://www.escapefromtarkov.com/uploads/mailtemplate/tw.png\" width=\"30\" class=\"CToWUd\"></a> --> </div> <div style=\"color:#fef7db\">С уважением,</div> <div style=\"color:#fef7db\">Команда разработчиков ОплатиПроезд</div> <div> <a href=\"mailto:sadt.project@gmail.com\" style=\"color:#ad9464\" target=\"_blank\">sadt.project@gmail.com</a> </div> </td> </tr> </tbody> </table>";
            
            new Task(() => SendLetter(email, body, subject)).Start();

            return code;
        }

        public void SendLetter(string email, string body, string subject = "Notification")
        {
            new Task(() =>
            {
                MailMessage message = new MailMessage(new MailAddress("sadt.project@gmail.com"), new MailAddress(email));
                message.Subject = subject;
                message.Body = body;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                // логин и пароль
                smtp.Credentials = new NetworkCredential("sadt.project@gmail.com", "idwowhupokyexaht");
                smtp.EnableSsl = true;
                smtp.Send(message);
            }).Start();
        }
    }
}
