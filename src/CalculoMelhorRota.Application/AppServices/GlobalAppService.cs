using CalculoMelhorRota.Application.Interfaces.AppServices;
using CalculoMelhorRota.CrossCutting.Util.Configs;
using System.Collections.Generic;

namespace CalculoMelhorRota.Application.AppServices
{
    public class GlobalAppService : IGlobalAppService
    {

        private readonly INotifier _notifier;
        protected int ErrorHttp;

        public GlobalAppService(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected void Notification(string message)
        {
            _notifier.Handle(new Notification(message));
        }

        protected void ErrorStatusCode(int errorCode)
        {
            _notifier.AddErrorStatusCode(errorCode);
        }


        protected List<Notification> GetNotifications()
        {
            return _notifier.GetNotifications();
        }

    }

}
