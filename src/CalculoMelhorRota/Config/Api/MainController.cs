using CalculoMelhorRota.CrossCutting.Util.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace CalculoMelhorRota.API.Config.Api
{
    public abstract class MainController : ControllerBase
    {
        private readonly INotifier _notifier;

        public MainController(INotifier notifier)
        {
            _notifier = notifier;
        }
        protected bool OperationValid()
        {
            if (_notifier.HasErroCode() || _notifier.HasNotification())
                return false;

            return true;
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperationValid())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            if (_notifier.HasErroCode())
            {
                return StatusCode(_notifier.ErrorCore(), new
                {
                    success = false,
                    errors = _notifier.GetNotifications().Select(n => n.Message)
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifier.GetNotifications().Select(n => n.Message)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifierErroModelInvalid(modelState);
            return CustomResponse();
        }

        protected void NotifierErroModelInvalid(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifierError(errorMsg);
            }
        }

        protected void NotifierError(string errorMsg)
        {
            _notifier.Handle(new Notification(errorMsg));
        }

        protected bool HasNotification()
        {
            return _notifier.HasNotification();
        }

    }

}
