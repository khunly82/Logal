using Logal.Facades;
using System.Net.Mail;

namespace Logal.Infrastructures
{
    public class Mailer: IMailer
    {
        private SmtpClient smtpClient;

        public Mailer(SmtpClient smtpClient)
        {
            this.smtpClient = smtpClient;
        }

        public void Send(string dest, string subject, string content, params Attachment[] attachments)
        {
            MailMessage message = new MailMessage();
            message.To.Add(dest);
            message.Subject = subject;
            message.From = new MailAddress("lykhun@gmail.com");
            message.Body = content;
            foreach (Attachment attachment in attachments)
            {
                message.Attachments.Add(attachment);
            }
            smtpClient.Send(message);
        }
    }
}
