using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace BookShop.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public IList<string> Errors { get; private set; }

        public ValidationException(ValidationResult result)
        {
            Errors = new List<string>();

            foreach (ValidationFailure error in result.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
        }
    }
}
