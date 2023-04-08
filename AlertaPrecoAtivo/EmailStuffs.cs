using System;
using System.Net;
using System.Net.Mail;

namespace EmailStuffs{
    public class EmailServerConfig{
        public string EmailSource { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public Boolean SSL { get; set; }
    }

    public class Email{
        public static void SendEmail(EmailServerConfig account, string dest_email, string subject, string body){
            //email
            MailMessage message = new MailMessage();
            message.From = new MailAddress(account.EmailSource);
            message.To.Add(new MailAddress(dest_email));
            message.Subject = subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.Body = body;

            //smtp
            SmtpClient smtpClient = new SmtpClient(account.Server, account.Port);
            smtpClient.EnableSsl = account.SSL;
            smtpClient.Credentials = new NetworkCredential(account.User, account.Password);
            smtpClient.Host = account.Server;

            try
            {
                // realiza o envio da mensagem
                smtpClient.Send(message);

                Console.WriteLine("enviado");
            }
            catch (Exception)
            {
                Console.WriteLine("Erro no envio de email!");
            }
        }
    }
}

