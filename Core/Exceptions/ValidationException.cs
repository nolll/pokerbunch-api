using System.Collections.Generic;
using System.Linq;

namespace Core.Exceptions
{
    public class ValidationException : PokerBunchException
    {
        public IEnumerable<string> Messages { get; private set; }

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