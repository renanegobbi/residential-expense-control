using ResidentialExpenseControl.Domain.Commands.Output;
using Swashbuckle.AspNetCore.Filters;
using static System.Net.WebRequestMethods;

namespace ResidentialExpenseControl.Api
{
    /// <summary>
    /// Standard API response for HTTP error 400
    /// </summary>
    public class BadRequestApiResponse : Output, IExamplesProvider<Output>
    {
        public BadRequestApiResponse()
            : base(false, new[] { "O campo X é obrigatório e não foi informado.", "O campo Y é obrigatório e não foi informado." }, null)
        {
        }

        public Output GetExamples()
        {
            return new BadRequestApiResponse();
        }
    }
}
