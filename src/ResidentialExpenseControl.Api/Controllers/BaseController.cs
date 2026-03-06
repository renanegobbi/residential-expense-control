using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Notifications;
using System.Collections.Generic;
using System.Linq;

namespace ResidentialExpenseControl.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private readonly INotifier _notifier;
        protected readonly ILogger _logger;

        protected BaseController(INotifier notifier, ILogger logger = null)
        {
            _notifier = notifier;
            _logger = logger;
        }

        protected bool ValidOperation()
        {
            return !_notifier.HasNotification();
        }

        protected ActionResult CustomResponse(object result = null, List<string> messages = null)
        {
            if (ValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    messages = messages,
                    result = result
                });
            }

            return BadRequest(new
            {
                success = false,
                messages = _notifier.GetNotifications().Select(n => n.Message),
                result = result
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifyErroModelInvalida(modelState);
            return CustomResponse();
        }

        protected void NotifyErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(errorMsg);
            }
        }

        protected void NotifyError(string message)
        {
            _notifier.Handle(new Notification(message));
        }
    }
}
