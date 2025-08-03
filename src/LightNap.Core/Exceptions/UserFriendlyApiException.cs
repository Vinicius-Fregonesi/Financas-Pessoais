using System;
using System.Collections.Generic;

namespace LightNap.Core.Exceptions
{
    public class UserFriendlyApiException : Exception
    {
        public IEnumerable<string>? Errors { get; }

        public UserFriendlyApiException(string message)
            : base(message)
        {
            Errors = new[] { message };
        }

        public UserFriendlyApiException(IEnumerable<string> errors)
            : base(errors is not null ? string.Join(" | ", errors) : "Erro inesperado.")
        {
            Errors = errors;
        }

        public UserFriendlyApiException(string message, Exception innerException)
            : base(message, innerException)
        {
            Errors = new[] { message };
        }

        public UserFriendlyApiException(IEnumerable<string> errors, Exception innerException)
            : base(errors is not null ? string.Join(" | ", errors) : "Erro inesperado.", innerException)
        {
            Errors = errors;
        }
    }
}
