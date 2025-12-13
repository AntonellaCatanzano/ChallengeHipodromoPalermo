using System.ComponentModel.DataAnnotations;


namespace ReservasTucson.Domain.Support.Helpers
{
    public class RequiredListAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var collection = value as System.Collections.IEnumerable;

            if (collection != null && collection.GetEnumerator().MoveNext())
            {
                return ValidationResult.Success;
            }

            var errorMessage = FormatErrorMessage(validationContext.DisplayName);
            return new ValidationResult(errorMessage);
        }
    }
}
