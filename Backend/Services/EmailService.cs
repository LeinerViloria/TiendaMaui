using System.Net;
using System.Net.Mail;

namespace Backend.Services
{
    public class EmailService
    {
        public bool SendEmail(string Subject, string Body, string SendTo)
        {
            var Host = "smtp.gmail.com";
            var Port = 587;
            var Alias = "TIENDA - AQUI_LO_ENCUENTRAS";
            var FromEmail = "t84098030@gmail.com";
            var Pass = "vmya ukrp rkec jozx";

            var Sender = new MailAddress(FromEmail, Alias);
            var BodyEmail = new MailMessage()
            {
                Subject = Subject,
                IsBodyHtml = true,
                From = Sender,
                Body = Body
            };

            BodyEmail.To.Add(SendTo);

            var Client = new SmtpClient()
            {
                UseDefaultCredentials = false,
                Host = Host,
                Port = Port,
                Credentials = new NetworkCredential(FromEmail, Pass),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };

            try
            {
                Client.Send(BodyEmail);
                Client.Dispose();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
    }
}
