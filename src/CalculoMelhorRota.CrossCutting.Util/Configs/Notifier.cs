using System.Collections.Generic;
using System.Linq;

namespace CalculoMelhorRota.CrossCutting.Util.Configs
{
    public class Notifier : INotifier
    {
        protected List<Notification> _notifications;
        protected int _errorCodeHttp;

        public Notifier()
        {
            _notifications = new List<Notification>();
            _errorCodeHttp = 0;
        }

        public void Handle(Notification notification)
        {
            _notifications.Add(notification);
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }

        public bool HasErroCode()
        {
            if (_errorCodeHttp > 0)
                return true;

            return false;
        }

        public int ErrorCore()
        {
            return _errorCodeHttp;
        }

        public void AddErrorStatusCode(int errorCode)
        {
            _errorCodeHttp = errorCode;
        }

        public void ClearErros()
        {
            _notifications = new List<Notification>();
        }
    }

}
