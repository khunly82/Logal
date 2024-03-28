using Logal.Facades;
using Logal.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace Logal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private IMailer mailer;

        public MailController(IMailer mailer)
        {
            this.mailer = mailer;
        }

        [HttpPost]
        public IActionResult Send([FromForm] MailForm form)
        {
            using MemoryStream stream = new MemoryStream();
            form.Attachment.CopyTo(stream);
            stream.Position = 0;
            mailer.Send("khun.ly@bstorm.be", form.Subject, form.Content, new Attachment(
                stream,
                form.Attachment.FileName,
                form.Attachment.ContentType
            ));
            return NoContent(); //204
        }
    }
}
