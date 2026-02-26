using System.Collections.Generic;

namespace ResidentialExpenseControl.Domain.Interfaces.Commands.Output
{
    public interface IOutput
    {
        /// <summary>Indicates whether the operation succeeded.</summary>
        bool Success { get; }

        /// <summary>Messages returned by the operation.</summary>
        IEnumerable<string> Messages { get; }

        /// <summary>Returned payload.</summary>
        object Result { get; }
    }
}
