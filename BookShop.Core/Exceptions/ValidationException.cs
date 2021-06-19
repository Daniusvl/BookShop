using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace BookShop.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; private set; }

        public ValidationException(ValidationResult result)
        {
            Errors = new List<string>();

            foreach (ValidationFailure error in result.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
        }

        public ValidationException(params string[] error_messages)
        {
            Errors = new List<string>();
            if(error_messages != null && error_messages.Length > 0)
            {
                Errors.AddRange(error_messages);
            }
        }
    }
}
