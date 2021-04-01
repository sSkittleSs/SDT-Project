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
using SDT_Project.AdditionalStructures;
using System.Globalization;

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

            string sql = $"SELECT Username, Password, ID, Email FROM users WHERE Username = '{userName}' and Password = '{userPassword}'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command?.ExecuteReader();
            uint result = 0;
            if (reader.HasRows && reader.Read())
            {
                UInt32 id = reader.GetUInt32(2);
                strings.AppendLine($"Результат запроса к БД:\nName: {reader.GetString(0) ?? "Null"}\nPassword: {reader.GetString(1) ?? "Null"}\n" +
                                   $"Id: {id}\nEmail: {reader.GetString(3)}");

                ServerUser user = new ServerUser()
                {
                    Id = id,
                    Name = userName,
                    Password = userPassword,
                    OperationContext = OperationContext.Current
                };

                consoleColor = ConsoleColor.Green;
                users.Add(user);
                strings.AppendLine($"Пользователь {user.Name} был подключен к серверу с идентификатором {user.Id}.");
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
            string sql = $"SELECT Username, ID, UserTypeID FROM users WHERE Email = '{email}'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command?.ExecuteReader();
            uint result = 0;
            if (reader.HasRows && reader.Read())
            {
                Int32 id = reader.GetInt32(2);
                Int32 type = reader.GetInt32(3);
                strings.AppendLine($"Результат запроса к БД:\nName: {reader.GetString(0) ?? "Null"}\nPassword: {reader.GetString(1) ?? "Null"}\n" +
                                   $"Id: {id}\nUserType: {GetUserTypeInString((uint)type)}");
                strings.AppendLine($"Пользователь {reader.GetString(0)} уже зарегистрирован под идентификатором {id}.");
            }
            else
            {
                sql = $"INSERT INTO `users`(`Username`, `Email`, `Password`, `UserTypeID`) VALUES ('{userName}','{email}','{userPassword}',1)";
                reader.Close();
                command = new MySqlCommand(sql, connection);
                int? rows = command?.ExecuteNonQuery();

                if (rows != 0)
                {
                    strings.Append($"Пользователь {userName} был зарегистрирован");
                    sql = $"SELECT ID FROM users WHERE Username = '{userName}' and Password = '{userPassword}'";

                    string code = SendConfirmationLetter(email);

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

                    connection.Close();
                    ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
                    return 0;
                }
                else
                {
                    consoleColor = ConsoleColor.DarkYellow;
                    strings.AppendLine($"Пользователь {userName} не был зарегистрирован.");
                }
            }
            #endregion

            //reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            //return result;
            return 1;
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
            string body = $"<table align=\"center\" border=\"0\" cellpadding=\"1\" cellspacing=\"1\" style=\"border-collapse:collapse;width:621px;color:#ffffff;background:#000000\"> <tbody> <tr> <td align=center> <img alt=\"\" src=\"https://i.ibb.co/CW7NP9N/logo-transp-white.png\" class=\"CToWUd a6T\" tabindex=\"0\"> </td> </tr> <tr> <td style=\"padding:0 40px 80px;font-size:10pt;color:#fef7db;font-family:tahoma,geneva,sans-serif;background:#080705;\"> <p style=\"color:#fef7db\">Ваш e-mail был использован для регистрации на нашем проекте. Код регистрации:</p><p style=\"color:#fef7db\"><b>{code}</b></p><p style=\"color:#fef7db\">Если вы не совершали данного действия, пожалуйста, смените пароль. Мы также рекомендуем вам сменить и пароль от e-mail.</p> </td> </tr> <tr> <td style=\"padding:40px;font-size:10pt;color:rgb(254,247,219);height:65px;vertical-align:bottom;background: #090805;\"> <div style=\"color:#fef7db;float:right\"> <!-- <a href=\"https://www.facebook.com/escapefromtarkov/\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.facebook.com/escapefromtarkov/&amp;source=gmail&amp;ust=1614890131806000&amp;usg=AFQjCNFV5BRZPLAE60tQY57gCG4nKnGbMg\"><img alt=\"\" height=\"30\" src=\"https://ci3.googleusercontent.com/proxy/PE4SOtb0Rm1BbaUwc6uu5-AaohCGDFXexlahZcT6bJzkEhL1gA545RJRtHEXF2AO8JyeY-EwySa2lLEMV6ER-sTkxQwyw4SQnk1GFR2rew=s0-d-e1-ft#https://www.escapefromtarkov.com/uploads/mailtemplate/fb.png\" width=\"30\" class=\"CToWUd\"></a> --> <!-- <a href=\"https://vk.com/e.bushmakin\" target=\"_blank\" data-saferedirecturl=\"https://goo-gl.ru/w5Wcl\"><img alt=\"\" height=\"30\" src=\"https://ci4.googleusercontent.com/proxy/EUjy-REZau-rSvLl2SErIKOZ8Khr0aNwroxKkNVisPDb3tkwGkXs4RxAD6HinuCdbDXQ94A9TMSxPMrx55-QH8RLuSmfhbZDcHYz0--tcA=s0-d-e1-ft#https://www.escapefromtarkov.com/uploads/mailtemplate/vk.png\" width=\"30\" class=\"CToWUd\"></a> --> <!-- <a href=\"https://twitter.com/bstategames\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://twitter.com/bstategames&amp;source=gmail&amp;ust=1614890131807000&amp;usg=AFQjCNG_VmJG1CYAABsASY9vMtOGU7KiUg\"><img alt=\"\" height=\"30\" src=\"https://ci4.googleusercontent.com/proxy/ECIEaOCVKWhigpU-uKOhWPcWH9kmwo8tHFLHX1N7sg3qD4B0K041v3NAd0KDJ2fzagRb8vDZ_q1SNPHL7Njo_XhtRxG7vevYdCj4C7mlsg=s0-d-e1-ft#https://www.escapefromtarkov.com/uploads/mailtemplate/tw.png\" width=\"30\" class=\"CToWUd\"></a> --> </div> <div style=\"color:#fef7db\">С уважением,</div> <div style=\"color:#fef7db\">Команда разработчиков ОплатиПроезд</div> <div> <a href=\"mailto:sadt.project@gmail.com\" style=\"color:#ad9464\" target=\"_blank\">sadt.project@gmail.com</a> </div> </td> </tr> </tbody> </table>";
            
            new Task(() => SendLetter(email, body, subject, isBodyHtml: true)).Start();

            return code;
        }

        public void SendLetter(string email, string body, string subject = "Notification", bool isBodyHtml = false)
        {
            new Task(() =>
            {
                MailMessage message = new MailMessage(new MailAddress("sadt.project@gmail.com"), new MailAddress(email));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isBodyHtml;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                // логин и пароль
                smtp.Credentials = new NetworkCredential("sadt.project@gmail.com", "idwowhupokyexaht");
                smtp.EnableSsl = true;
                smtp.Send(message);
            }).Start();
        }

        public string GetEmail(uint id)
        {
            StringBuilder strings = new StringBuilder();
            ConsoleColor consoleColor = ConsoleColor.White;
            connection.Open();

            string sql = $"SELECT Email FROM users WHERE ID = {id}";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command?.ExecuteReader();
            string result = "";
            if (reader.HasRows && reader.Read())
            {
                result = reader.GetString(0);

                consoleColor = ConsoleColor.Green;
                strings.AppendLine($"Пользователь под идентификатором {id} запросил почту: {result}.");
            }
            else
            {
                consoleColor = ConsoleColor.DarkYellow;
                strings.AppendLine($"Пользователь под идентификатором {id} не получил ответ на запрос почты.");
            }

            reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            return result;
        }

        public TravelCard GetCard(uint id)
        {
            StringBuilder strings = new StringBuilder();
            ConsoleColor consoleColor = ConsoleColor.White;
            connection.Open();

            string sql = $"SELECT `CardID` FROM `user_travelcard` WHERE UserId = {id}";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command?.ExecuteReader();
            TravelCard result = null;
            if (reader.HasRows && reader.Read())
            {
                int cardId = reader.GetInt32(0);

                sql = $"SELECT `ID`, `Type`, `Balance`, `FirstDate`, `LastDate`, `CategoryID`, `StatusID` FROM `travelcards` WHERE ID = {cardId}";
                reader.Close();
                command = new MySqlCommand(sql, connection);
                reader = command?.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    result = new TravelCard();
                    result.Id = reader.GetInt32(0);
                    result.Type = (CardTypes)reader.GetInt32(1);
                    result.Balance = reader.GetInt32(2);

                    if (!reader.IsDBNull(reader.GetOrdinal("FirstDate")))
                        result.FirstDate = reader.GetDateTime(3);
                    else
                        result.FirstDate = new DateTime();

                    if (!reader.IsDBNull(reader.GetOrdinal("LastDate")))
                        result.LastDate = reader.GetDateTime(4);
                    else
                        result.LastDate = new DateTime();


                    result.Category = (CardCategories)reader.GetInt32(5);
                    result.Status = (CardStatuses)reader.GetInt32(6);

                    consoleColor = ConsoleColor.Green;
                    strings.AppendLine($"Пользователь под идентификатором {id} запросил данные о карте: {cardId}.");
                }
                else
                {
                    consoleColor = ConsoleColor.DarkYellow;
                    strings.AppendLine($"Пользователь под идентификатором {id} не получил данные о своей карте ({cardId}).");
                }
            }
            else
            {
                consoleColor = ConsoleColor.DarkYellow;
                strings.AppendLine($"Пользователь под идентификатором {id} не получил данные о своей карте.");
            }

            reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            return result;
        }

        public Transaction GetLastTransaction(uint id)
        {
            StringBuilder strings = new StringBuilder();
            ConsoleColor consoleColor = ConsoleColor.White;
            connection.Open();

            string sql = $"SELECT `ID`, `Date`, `Sum` FROM `transactions` WHERE `UserID` = {id} ORDER BY `Date` DESC LIMIT 1";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command?.ExecuteReader();
            Transaction result = null;
            if (reader.HasRows && reader.Read())
            {
                result = new Transaction();
                result.Id = reader.GetInt32(0);
                result.Date = reader.GetDateTime(1);
                result.Sum = reader.GetDouble(2);

                consoleColor = ConsoleColor.Green;
                strings.AppendLine($"Пользователь под идентификатором {id} запросил данные о последней транзакции:\n ID: {result.Id};\n Дата проведения: {result.Date};\n Сумма: {result.Sum}.");
            }
            else
            {
                consoleColor = ConsoleColor.DarkYellow;
                strings.AppendLine($"Пользователь под идентификатором {id} не получил ответ на запрос последней транзакции.");
            }

            reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            return result;
        }

        public bool PayForTheTrip(uint cardId, int newBalance)
        {
            StringBuilder strings = new StringBuilder();

            connection.Open();
            ConsoleColor consoleColor = ConsoleColor.White;

            #region someRequestToDB
            string sql = $"UPDATE `travelcards` SET `Balance`= {newBalance} WHERE `ID` = {cardId}";
            MySqlCommand command = new MySqlCommand(sql, connection);
            int? rows = command?.ExecuteNonQuery();

            if (rows == 0)
            {
                consoleColor = ConsoleColor.DarkYellow;
                strings.AppendLine($"Запрос на изменение баланса у карты {cardId} неудачен!");
            }
            else
            {
                connection.Close();
                consoleColor = ConsoleColor.Green;
                strings.AppendLine($"Запрос на изменение баланса у карты {cardId} успешен. Новый баланс: {newBalance}");
                ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);

                return true;
            }
            #endregion

            //reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            //return result;
            return false;
        }

        public bool ChangeUsername(uint userId, string newUsername)
        {
            StringBuilder strings = new StringBuilder();

            connection.Open();
            ConsoleColor consoleColor = ConsoleColor.White;

            #region someRequestToDB
            string sql = $"UPDATE `users` SET `Username` = '{newUsername}' WHERE `ID` = {userId}";
            MySqlCommand command = new MySqlCommand(sql, connection);
            int? rows = command?.ExecuteNonQuery();

            if (rows == 0)
            {
                consoleColor = ConsoleColor.DarkYellow;
                strings.AppendLine($"Запрос на изменение никнейма у пользователя {userId} на {newUsername} неудачен!");
            }
            else
            {
                connection.Close();
                consoleColor = ConsoleColor.Green;
                strings.AppendLine($"Запрос на изменение никнейма у пользователя {userId} на {newUsername} успешен.");
                ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
                users[users.FindIndex((x) => x.Id == userId)].Name = newUsername;
                return true;
            }
            #endregion

            //reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            //return result;
            return false;
        }

        public bool ChangeEmail(uint userId, string newEmail)
        {
            StringBuilder strings = new StringBuilder();

            connection.Open();
            ConsoleColor consoleColor = ConsoleColor.White;

            #region someRequestToDB
            string sql = $"SELECT Username, ID, UserTypeID FROM users WHERE Email = '{newEmail}'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command?.ExecuteReader();
            if (reader.HasRows && reader.Read())
            {
                Int32 id = reader.GetInt32(2);
                Int32 type = reader.GetInt32(3);
                consoleColor = ConsoleColor.DarkYellow;
                strings.AppendLine($"Результат запроса к БД:\nName: {reader.GetString(0) ?? "Null"}\nPassword: {reader.GetString(1) ?? "Null"}\n" +
                                   $"Id: {id}\nUserType: {GetUserTypeInString((uint)type)}");
                strings.AppendLine($"Пользователь {reader.GetString(0)} уже зарегистрирован под идентификатором {newEmail}. Смена почты отменена");
            }
            else
            {
                reader.Close();
                sql = $"UPDATE `users` SET `Email`= '{newEmail}' WHERE `ID` = {userId}";
                command = new MySqlCommand(sql, connection);
                int? rows = command?.ExecuteNonQuery();

                if (rows == 0)
                {
                    consoleColor = ConsoleColor.DarkYellow;
                    strings.AppendLine($"Запрос на изменение почты у пользователя {userId} на {newEmail} неудачен!");
                }
                else
                {
                    connection.Close();
                    consoleColor = ConsoleColor.Green;
                    strings.AppendLine($"Запрос на изменение почты у пользователя {userId} на {newEmail} успешен.");
                    ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
                    users[users.FindIndex((x) => x.Id == userId)].Email = newEmail;
                    return true;
                }
            }
            #endregion

            //reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            //return result;
            return false;
        }

        public bool BuyCard(uint userId, double sum, CardTypes type, CardCategories category, int balance)
        {
            StringBuilder strings = new StringBuilder();

            connection.Open();
            ConsoleColor consoleColor = ConsoleColor.White;

            #region someRequestToDB
            string sql = $"DELETE FROM `user_travelcard` WHERE `UserID` = {userId}";
            MySqlCommand command = new MySqlCommand(sql, connection);
            int? rows = command?.ExecuteNonQuery();

            if (rows != 0)
                strings.AppendLine($"Карты пользователя {userId} были удалены из БД.");
            else
                strings.AppendLine($"Карты пользователя {userId} не были удалены из БД.");

            sql = $"INSERT INTO `travelcards`(`Type`, `Balance`, `FirstDate`, `LastDate`, `CategoryID`, `StatusID`) VALUES ({(int)type}, {balance}, '{DateTime.Today.ToString("yyyy-MM-dd")}', '{DateTime.Today.AddMonths(2).ToString("yyyy-MM-dd")}', {(int)category}, 1)";
            command = new MySqlCommand(sql, connection);
            rows = command?.ExecuteNonQuery();
            if (rows != 0)
            {
                sql = $"INSERT INTO `user_travelcard`(`UserID`, `CardID`) VALUES ({userId}, (SELECT `ID` FROM `travelcards` WHERE `Type` = {(int)type} AND `Balance` = {balance} AND `FirstDate` = '{DateTime.Today.ToString("yyyy-MM-dd")}' AND `LastDate` = '{DateTime.Today.ToLocalTime().AddMonths(2).ToString("yyyy-MM-dd")}' AND `CategoryID` = {(int)category} AND `StatusID` = 1 ORDER BY `ID` DESC LIMIT 1))";
                command = new MySqlCommand(sql, connection);
                command?.ExecuteNonQuery();
                
                strings.AppendLine($"Новая карта пользователя {userId} была зарегистрирована.\nType: {(int)type}\nBalance: {balance}\nFirstDate: {DateTime.Today.ToString("yyyy-MM-dd")}\nLastDate: {DateTime.Today.AddMonths(2).ToString("yyyy-MM-dd")}\nCategory: {(int)category}");

                sql = $"INSERT INTO `transactions`(`UserID`, `Date`, `Sum`) VALUES ({userId}, '{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}', {sum.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." })})";
                command = new MySqlCommand(sql, connection);
                rows = command?.ExecuteNonQuery();

                if (rows != 0)
                {
                    consoleColor = ConsoleColor.Green;
                    strings.AppendLine($"Транзакция пользователя {userId} зарегистрирована на сумму {sum}.");
                }
                else
                {
                    consoleColor = ConsoleColor.DarkYellow;
                    strings.AppendLine($"Однако транзакция пользователя {userId} не зарегистрирована.");
                }

                connection.Close();
                ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
                return true;
            }
            else
            {
                consoleColor = ConsoleColor.DarkYellow;
                strings.AppendLine($"Новая карта пользователя {userId} не была зарегистрирована.\nТранзакция не произведена.");
            }
            #endregion

            //reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            //return result;
            return false;
        }

        public bool ExtendCard(uint cardId, int newBalance, DateTime newDate)
        {
            StringBuilder strings = new StringBuilder();

            connection.Open();
            ConsoleColor consoleColor = ConsoleColor.White;

            #region someRequestToDB
            string sql = $"UPDATE `travelcards` SET `Balance`= {newBalance}, `LastDate` = '{newDate.ToString("yyyy-MM-dd")}' WHERE `ID` = {cardId}";
            MySqlCommand command = new MySqlCommand(sql, connection);
            int? rows = command?.ExecuteNonQuery();

            if (rows == 0)
            {
                consoleColor = ConsoleColor.DarkYellow;
                strings.AppendLine($"Запрос на продление карты {cardId} неудачен!");
            }
            else
            {
                connection.Close();
                consoleColor = ConsoleColor.Green;
                strings.AppendLine($"Запрос на продление карты {cardId} успешен.\nНовый баланс: {newBalance}\nНовая конечная дата: {newDate.ToString("yyyy-MM-dd")}");
                ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);

                return true;
            }
            #endregion

            //reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            //return result;
            return false;
        }

        public int GetUserType(uint id)
        {
            StringBuilder strings = new StringBuilder();
            ConsoleColor consoleColor = ConsoleColor.White;
            connection.Open();

            string sql = $"SELECT `UserTypeID` FROM `users` WHERE `ID` = {id}";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader = command?.ExecuteReader();
            int result = 0;
            if (reader.HasRows && reader.Read())
            {
                result = reader.GetInt32(0) - 1;

                consoleColor = ConsoleColor.Green;
                strings.AppendLine($"Пользователь под идентификатором {id} запросил данные о своих правах:\n ID: {id};\n Права: {GetUserTypeInString((uint)result)}.");
            }
            else
            {
                consoleColor = ConsoleColor.DarkYellow;
                strings.AppendLine($"Пользователь под идентификатором {id} не получил ответ на запрос своих прав.");
            }

            reader.Close();
            connection.Close();
            ConsoleLog(strings.ToString(), MessageImportance.SysInfo, consoleColor);
            return result;
        }
    }
}
