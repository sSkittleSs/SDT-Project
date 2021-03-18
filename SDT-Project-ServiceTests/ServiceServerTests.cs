using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDT_Project_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SDT_Project_Service.Tests
{
    [TestClass()]
    public class ServiceServerTests
    {
        [TestMethod()]
        public void RegisteringTest()
        {
            string code = "";
            Random rand = new Random();
            for (int i = 0; i < 8; i++)
                code += rand.Next(2) != 0 ? (char)(rand.Next(65, 90)) : (char)(rand.Next(48, 57));

            MailMessage message = new MailMessage(new MailAddress("sadt.project@gmail.com", "SADT Project Automessage"), new MailAddress("s9005310@gmail.com"));
            message.Subject = "Registering your account (SADT-Project)";
            message.Body = $"<table align=\"center\" border=\"0\" cellpadding=\"1\" cellspacing=\"1\" style=\"border-collapse:collapse;width:621px;color:#ffffff;background:#000000\"> <tbody> <tr> <td align=center> <img alt=\"\" src=\"https://i.ibb.co/CW7NP9N/logo-transp-white.png\" class=\"CToWUd a6T\" tabindex=\"0\"> </td> </tr> <tr> <td style=\"padding:0 40px 80px;font-size:10pt;color:#fef7db;font-family:tahoma,geneva,sans-serif;background:#080705;\"> <p style=\"color:#fef7db\">Код подтверждения:</p><p style=\"color:#fef7db\"><b>{code}</b></p><p style=\"color:#fef7db\">Если вы не совершали данного действия, пожалуйста, смените пароль. Мы также рекомендуем вам сменить и пароль от e-mail.</p> </td> </tr> <tr> <td style=\"padding:40px;font-size:10pt;color:rgb(254,247,219);height:65px;vertical-align:bottom;background: #090805;\"> <div style=\"color:#fef7db;float:right\"> <!-- <a href=\"https://www.facebook.com/escapefromtarkov/\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.facebook.com/escapefromtarkov/&amp;source=gmail&amp;ust=1614890131806000&amp;usg=AFQjCNFV5BRZPLAE60tQY57gCG4nKnGbMg\"><img alt=\"\" height=\"30\" src=\"https://ci3.googleusercontent.com/proxy/PE4SOtb0Rm1BbaUwc6uu5-AaohCGDFXexlahZcT6bJzkEhL1gA545RJRtHEXF2AO8JyeY-EwySa2lLEMV6ER-sTkxQwyw4SQnk1GFR2rew=s0-d-e1-ft#https://www.escapefromtarkov.com/uploads/mailtemplate/fb.png\" width=\"30\" class=\"CToWUd\"></a> --> <!-- <a href=\"https://vk.com/e.bushmakin\" target=\"_blank\" data-saferedirecturl=\"https://goo-gl.ru/w5Wcl\"><img alt=\"\" height=\"30\" src=\"https://ci4.googleusercontent.com/proxy/EUjy-REZau-rSvLl2SErIKOZ8Khr0aNwroxKkNVisPDb3tkwGkXs4RxAD6HinuCdbDXQ94A9TMSxPMrx55-QH8RLuSmfhbZDcHYz0--tcA=s0-d-e1-ft#https://www.escapefromtarkov.com/uploads/mailtemplate/vk.png\" width=\"30\" class=\"CToWUd\"></a> --> <!-- <a href=\"https://twitter.com/bstategames\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://twitter.com/bstategames&amp;source=gmail&amp;ust=1614890131807000&amp;usg=AFQjCNG_VmJG1CYAABsASY9vMtOGU7KiUg\"><img alt=\"\" height=\"30\" src=\"https://ci4.googleusercontent.com/proxy/ECIEaOCVKWhigpU-uKOhWPcWH9kmwo8tHFLHX1N7sg3qD4B0K041v3NAd0KDJ2fzagRb8vDZ_q1SNPHL7Njo_XhtRxG7vevYdCj4C7mlsg=s0-d-e1-ft#https://www.escapefromtarkov.com/uploads/mailtemplate/tw.png\" width=\"30\" class=\"CToWUd\"></a> --> </div> <div style=\"color:#fef7db\">С уважением,</div> <div style=\"color:#fef7db\">Команда разработчиков ОплатиПроезд</div> <div> <a href=\"mailto:sadt.project@gmail.com\" style=\"color:#ad9464\" target=\"_blank\">sadt.project@gmail.com</a> </div> </td> </tr> </tbody> </table>";
            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            
            smtp.Credentials = new NetworkCredential("sadt.project@gmail.com", "idwowhupokyexaht");
            smtp.EnableSsl = true;
            smtp.Send(message);

            Assert.IsTrue(true);
        }
    }
}