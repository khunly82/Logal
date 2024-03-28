using System.ComponentModel.DataAnnotations;

namespace Logal.Validators
{
    public class AfterAttribute : ValidationAttribute
    {
        private string compareField;

        public AfterAttribute(string compareField)
        {
            this.compareField = compareField;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }
            DateTime? startDate = validationContext.ObjectInstance.GetType()
                .GetProperty(compareField)?.GetValue(validationContext.ObjectInstance) as DateTime?;

            if(startDate == null)
            {
                return ValidationResult.Success;
            }

            return startDate < (DateTime)value ? ValidationResult.Success : new ValidationResult("LA valeur est incorrecte");
        }
    }
}
