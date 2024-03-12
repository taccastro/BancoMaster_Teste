using CalculoMelhorRota.CrossCutting.Util.Configs;

namespace CalculoMelhorRota.Domain.Service
{
    public class BaseService
    {
        private INotifier _notifier;

        public BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected void Notification(string message)
        {
            _notifier.Handle(new Notification(message));
        }
    }
}
