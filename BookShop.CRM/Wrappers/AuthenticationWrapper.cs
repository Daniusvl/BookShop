using BookShop.CRM.Models;
using BookShop.CRM.Wrappers.Base;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.CRM.Wrappers
{
    public class AuthenticationWrapper : ModelWrapper<AuthenticationModel>
    {
        public AuthenticationWrapper(AuthenticationModel model) : base(model)
        {
            
        }

        public string Email 
        { 
            get => GetValue<string>(nameof(Email));
            set => SetValue(value, nameof(Email)); 
        }

        public string Password
        {
            get => GetValue<string>(nameof(Password));
            set => SetValue(value, nameof(Password));
        }

        protected override IEnumerable<string> ValidateProperties(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Email):
                    EmailValidation mail_rules = new();
                    ValidationResult mail_result = mail_rules.Validate(Email);
                    if (!mail_result.IsValid)
                    {
                        return mail_result.Errors.Select(e => e.ErrorMessage).ToArray();
                    }
                    break;
                case nameof(Password):
                    PasswordValidation pass_rules = new();
                    ValidationResult pass_result = pass_rules.Validate(Password);
                    if (!pass_result.IsValid)
                    {
                        return pass_result.Errors.Select(e => e.ErrorMessage).ToArray();
                    }
                    break;
            }
            return null;
        }

        private class EmailValidation : AbstractValidator<string> 
        {
            public EmailValidation()
            {
                RuleFor(email => email)
                    .NotEmpty()
                        .WithMessage("Email cannot be empty")
                    .EmailAddress()
                        .WithMessage("Provide valid email");
            }
        }

        private class PasswordValidation : AbstractValidator<string> 
        {
            public PasswordValidation()
            {
                RuleFor(pass => pass)
                    .Length(6, 20)
                        .WithMessage("Password must contain from 6 to 20 characters")
                    .Must(s => s.Any(char.IsLower))
                        .WithMessage("Password must contain atleast 1 lower case")
                        .Must(s => s.Any(char.IsUpper))
                        .WithMessage("Password must contain atleast 1 upper case")
                    .Must(s => s.Any(char.IsDigit))
                        .WithMessage("Password must contain atleast 1 digit");
            }
        }
    }
}
