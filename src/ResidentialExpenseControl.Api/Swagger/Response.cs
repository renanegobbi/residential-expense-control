using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using System.Collections.Generic;

namespace ResidentialExpenseControl.Api.Swagger
{
    public class Response: IOutput
    {
        /// <summary>
        /// Indicates whether the operation succeeded.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Messages returned by the operation.
        /// </summary>
        public IEnumerable<string> Messages { get; set; }

        /// <summary>
        /// Returned payload.
        /// </summary>
        public object Result { get; set; }
    }
}



