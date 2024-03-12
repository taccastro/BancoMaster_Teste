using System.Collections.Generic;

namespace CalculoMelhorRota.CrossCutting.Util.Configs
{
    public interface INotifier
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
        bool HasErroCode();
        int ErrorCore();
        void AddErrorStatusCode(int errorCode);
        void ClearErros();
    }

}
