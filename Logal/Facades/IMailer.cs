using System.Net.Mail;

namespace Logal.Facades
{
    public interface IMailer
    {
        void Send(string dest, string subject, string content, params Attachment[] attachments);
    }
}
