using Microsoft.AspNetCore.Mvc;
using ResidentialExpenseControl.Domain.Interfaces.Commands.Output;
using System.Net;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Api
{
    /// <summary>
    /// Default result for all API routes
    /// </summary>
    public class ApiResult : IActionResult
    {
        private readonly IOutput _output;

        public ApiResult(IOutput output)
        {
            _output = output;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var jsonResult = new JsonResult(_output)
            {
                StatusCode = !_output.Success
                    ? (int)HttpStatusCode.BadRequest
                    : (int)HttpStatusCode.OK
            };

            await jsonResult.ExecuteResultAsync(context);
        }
    }
}
