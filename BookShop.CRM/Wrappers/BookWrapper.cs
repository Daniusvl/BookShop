using BookShop.CRM.Models;
using BookShop.CRM.Wrappers.Base;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.CRM.Wrappers
{
    public class BookWrapper : ModelWrapper<BookModel>
    {
        private BookModel model;

        public BookWrapper(BookModel model) : base(model, false)
        {
            this.model = model;
        }

        protected override IEnumerable<string> ValidateProperties(string propertyName)
        {
            BookValidator validator = new(propertyName);
            ValidationResult result = validator.Validate(model);
            return result.Errors.Select(e => e.ErrorMessage).ToArray();
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

        public string Description 
        { 
            get => GetValue<string>(nameof(Description)); 
            set => SetValue(value, nameof(Description)); 
        }

        public decimal Price 
        { 
            get => GetValue<decimal>(nameof(Price)); 
            set => SetValue(value, nameof(Price)); 
        }

        public bool Hidden 
        { 
            get => GetValue<bool>(nameof(Hidden)); 
            set => SetValue(value, nameof(Hidden)); 
        }

        public DateTime DateReleased 
        { 
            get => GetValue<DateTime>(nameof(DateReleased)); 
            set => SetValue(value, nameof(DateReleased)); 
        }

        public int AuthorId 
        { 
            get => GetValue<int>(nameof(AuthorId)); 
            set => SetValue(value, nameof(AuthorId)); 
        }

        public int CategoryId 
        {
            get => GetValue<int>(nameof(CategoryId));
            set => SetValue(value, nameof(CategoryId));
        }

        protected class BookValidator : AbstractValidator<BookModel>
        {
            public BookValidator(string propertyName)
            {
                switch (propertyName)
                {
                    case nameof(Name):
                        RuleFor(command => command.Name)
                            .NotNull()
                                .WithMessage("{PropertyName} cannot be null")
                            .NotEmpty()
                                .WithMessage("{PropertyName} cannot be empty")
                            .Length(3, 150)
                                .WithMessage("{PropertyName} must contain from 3 to 150 characters");
                        break;
                    case nameof(Description):
                        RuleFor(command => command.Description)
                            .NotNull()
                                .WithMessage("{PropertyName} cannot be null")
                            .NotEmpty()
                                .WithMessage("{PropertyName} cannot be empty")
                            .Length(10, 1000)
                                .WithMessage("Description must contain from 10 to 1000 characters");
                        break;
                    case nameof(Price):
                        RuleFor(command => command.Price)
                            .GreaterThan(0)
                                .WithMessage("{PropertyName} must be more than 0");
                        break;
                    case nameof(AuthorId):
                        RuleFor(command => command.AuthorId)
                            .GreaterThan(0)
                                .WithMessage("{PropertyName} must be more than 0");
                        break;
                    case nameof(CategoryId):
                        RuleFor(command => command.CategoryId)
                            .GreaterThan(0)
                                .WithMessage("{PropertyName} must be more than 0");
                        break;
                }
            }
        }
    }
}
