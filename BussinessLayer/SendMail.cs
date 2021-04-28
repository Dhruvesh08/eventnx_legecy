using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public static class SendMailService
    {
        public static void SendMail(string toMailId, string subject,string body)
        {
            try
            {
                var Adminsetting = AdminService.GetAdminSetting();
                var fromEmail = new MailAddress(Adminsetting.Smtpusername, Adminsetting.Companyname);
                var fromEmailPassword = Adminsetting.Smtpassword;

                var smtp = new SmtpClient
                {
                    Host = Adminsetting.Hostname,
                    Port = Convert.ToInt32(Adminsetting.portno),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword),
                };
                
                var message = new MailMessage()
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.From = fromEmail;
                var toEmails = toMailId.Split(',');
                foreach (var email in toEmails)
                {
                    message.To.Add(new MailAddress(email));
                }
                smtp.Send(message);
            }
            catch
            {

            }
        }
    }
}
