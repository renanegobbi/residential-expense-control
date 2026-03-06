using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using ResidentialExpenseControl.Domain.Commands.Output;
using System.Linq;
using Newtonsoft.Json;

namespace ResidentialExpenseControl.Api.Filters
{
    /// <summary>
    /// A filter that extracts messages from ModelState and puts them into the API's output format.
    /// </summary>
    public class CustomModelStateValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var validationResult = new ValidationResultModel(context.ModelState);
                context.Result = new JsonResult(new Output(false, new[] { "Erros foram encontrados na estrutura JSON de entrada." }, validationResult));
            }
        }
    }
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Campo { get; }
        public string Mensagem { get; }
        public ValidationError(string field, string message)
        {
            Campo = field != string.Empty ? field : null;
            Mensagem = message;
        }
    }
    public class ValidationResultModel
    {
        public List<ValidationError> Erros { get; }
        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Erros = modelState.Keys
            .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, string.IsNullOrEmpty(x.ErrorMessage) ? x.Exception?.Message : x.ErrorMessage)))
            .ToList();
        }
    }
}
