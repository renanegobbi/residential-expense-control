using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using System.Collections.Generic;

namespace ResidentialExpenseControl.Domain.Commands.Output
{
    public class Output : IOutput
    {
        public bool Success { get; }

        public IEnumerable<string> Messages { get; }

        public object Result { get; }

        public Output(bool success, IEnumerable<string> messages, object result)
        {
            Success = success;
            Messages = messages;
            Result = result;
        }
    }
}
