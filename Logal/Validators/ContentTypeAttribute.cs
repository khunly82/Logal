using System.ComponentModel.DataAnnotations;

namespace Logal.Validators
{
    public class ContentTypeAttribute : ValidationAttribute
    {

        private string[] validContentTypes;

        public ContentTypeAttribute(params string[] validContentTypes)
        {
            this.validContentTypes = validContentTypes;
            ErrorMessage = "Le type est invalide";
        }

        public override bool IsValid(object? value)
        {
            if(value == null)
            {
                return true;
            }

            return validContentTypes.Contains((value as IFormFile)?.ContentType);
        }
    }
}
