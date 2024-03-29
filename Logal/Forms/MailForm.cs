using Logal.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Logal.Forms
{
    public class MailForm
    {
        [Required()]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z]*$")]
        public string Subject { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        [ContentType("image/jpeg", "image/png")]
        public IFormFile Attachment { get; set; } = null!;

        public virtual DateTime StartDate { get; set; }

        [After("StartDate")]
        public DateTime EndDate { get; set; }
    }
}
