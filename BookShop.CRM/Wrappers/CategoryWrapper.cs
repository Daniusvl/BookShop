using BookShop.CRM.Models;
using BookShop.CRM.Wrappers.Base;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.CRM.Wrappers
{
    public class CategoryWrapper : ModelWrapper<CategoryModel>
    {
        public CategoryWrapper(CategoryModel model) : base(model, false)
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
            CategoryValidator validator = new(propertyName);
            ValidationResult result = validator.Validate(Model);
            return result.Errors.Select(e => e.ErrorMessage);
        }

        protected class CategoryValidator : AbstractValidator<CategoryModel> 
        {
            public CategoryValidator(string propertyName)
            {
                switch (propertyName)
                {
                    case nameof(Name):
                        RuleFor(command => command.Name)
                            .NotEmpty()
                                .WithMessage("{PropertyName} cannot be empty")
                            .Length(2, 100)
                                .WithMessage("{PropertyName} must contain from 2 to 100 characters");
                        break;
                }
            }
        }
    }
}
