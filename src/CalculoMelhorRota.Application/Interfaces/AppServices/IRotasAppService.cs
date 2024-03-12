using CalculoMelhorRota.Application.ViewsModels;
using System.Collections.Generic;
using System.Threading;

namespace CalculoMelhorRota.Application.Interfaces.AppServices
{
    public interface IRotasAppService : IGlobalAppService
    {
        IEnumerable<RotasViewModel> AdicionarRotas(IEnumerable<RotasViewModel> rotas, CancellationToken cancellationToken);
        string MelhorRota(string rotas, CancellationToken cancellationToken);
    }
}
