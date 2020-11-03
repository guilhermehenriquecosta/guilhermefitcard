﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fitcard_estabelecimentos.domain.Domain.Utils
{
    public class CustomValidationEmailAttribute : ValidationAttribute, IClientModelValidator
    {
        public CustomValidationEmailAttribute()
        {

        }

        //public override bool IsValid(object value)
        //{
        //    if (value == null || string.IsNullOrEmpty(value.ToString()))
        //        return true;
        //    return Util.emailValido(value.ToString());
        //}

        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    yield return new ModelClientValidationRule
        //    {
        //        ErrorMessage = this.FormatErrorMessage(null),
        //        ValidationType = "customvalidationemail"
        //    };
        //}

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;
            var emailValid = Util.emailValido(value.ToString());
            if (emailValid)
                return ValidationResult.Success;
            return new ValidationResult("E-mail inválido");
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Attributes.Add("data-val-customvalidationemail", ErrorMessageString);
        }
    }
}
