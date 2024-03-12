using CalculoMelhorRota.Domain.Entity;
using System.Collections.Generic;

namespace CalculoMelhorRota.Domain.Interfaces
{
    public interface IRotasService
    {
        IEnumerable<Rotas> AdicionarRotas(IEnumerable<Rotas> rotas);
        string MelhorRota(string rotas);
        IEnumerable<Rotas> GetRotas();
    }
}
