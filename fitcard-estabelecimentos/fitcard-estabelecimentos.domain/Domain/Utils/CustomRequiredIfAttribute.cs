using System;
using System.ComponentModel.DataAnnotations;

namespace fitcard_estabelecimentos.domain.Domain.Utils
{
    public class CustomRequiredIfAttribute : ValidationAttribute
    {
        private String PropertyName { get; set; }
        private String ErrorMessageCustom { get; set; }
        private Object DesiredValue { get; set; }

        public CustomRequiredIfAttribute(String propertyName, Object desiredvalue, String errormessage)
        {
            this.PropertyName = propertyName;
            this.DesiredValue = desiredvalue;
            this.ErrorMessageCustom = errormessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (proprtyvalue == null)
            {
                if (DesiredValue == null && value == null)
                {
                    return new ValidationResult(ErrorMessageCustom);
                }
                return ValidationResult.Success;
            }
            else if (proprtyvalue.ToString() == DesiredValue.ToString() && value == null)
            {
                return new ValidationResult(ErrorMessageCustom);
            }
            return ValidationResult.Success;
        }
    }
}
