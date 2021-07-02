using BookShop.CRM.Models;
using BookShop.CRM.Wrappers.Base;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.CRM.Wrappers
{
    public class AuthorWrapper : ModelWrapper<AuthorModel>
    {
        public AuthorWrapper(AuthorModel model) : base(model, false)
        {

        }

        public int Id
        {
            get => GetValue<int>(nameof(Id));
            set => SetValue(value, nameof(Id));
        }

        public string Name
        {
            get => GetValue<string>(nameof(Name));
            set => SetValue(value, nameof(Name));
        }

        protected override IEnumerable<string> ValidateProperties(string propertyName)
        {
            AuthorValidator validator = new(propertyName);
            ValidationResult result = validator.Validate(Model);
            return result.Errors.Select(v => v.ErrorMessage);
        }

        protected class AuthorValidator : AbstractValidator<AuthorModel> 
        {
            public AuthorValidator(string propertyName)
            {
                switch (propertyName)
                {
                    case nameof(Name):
                        RuleFor(command => command.Name)
                            .NotNull()
                                .WithMessage("{PropertyName} cannot be null")
                            .NotEmpty()
                                .WithMessage("{PropertyName} cannot be empty")
                            .Length(5, 200)
                                .WithMessage("{PropertyName} must contain from 5 to 200 characters");
                        break;
                }
            }
        }
    }
}
