using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fitcard_estabelecimentos.domain.Domain.Utils
{
    public class CustomValidationCNPJAttribute : ValidationAttribute, IClientModelValidator
    {
        public CustomValidationCNPJAttribute()
        {
                
        }

        //public override bool IsValid(object value)
        //{
        //    if (value == null || string.IsNullOrEmpty(value.ToString()))
        //        return true;
        //    return Util.cnpjValido(value.ToString());
        //}

        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    yield return new ModelClientValidationRule
        //    {
        //        ErrorMessage = this.FormatErrorMessage(null),
        //        ValidationType = "customvalidationcnpj"
        //    };
        //}

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;
            var cnpjValid = Util.cnpjValido(value.ToString());
            if (cnpjValid)
                return ValidationResult.Success;
            return new ValidationResult("CNPJ inválido");
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Attributes.Add("data-val-customvalidationcnpj", ErrorMessageString);
        }
    }
}
