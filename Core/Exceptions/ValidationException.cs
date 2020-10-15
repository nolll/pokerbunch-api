using System.Collections.Generic;
using System.Linq;

namespace Core.Exceptions
{
    public class ValidationException : PokerBunchException
    {
        public override string Message => string.Join(' ', Messages);

        public IEnumerable<string> Messages { get; }

        public ValidationException(Validator validator)
        {
            Messages = validator.Errors.ToList();
        }

        public ValidationException(string message)
        {
            Messages = new List<string> {message};
        }
    }
}