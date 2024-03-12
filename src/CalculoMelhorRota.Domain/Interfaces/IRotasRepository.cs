using CalculoMelhorRota.Domain.Entity;
using System.Collections.Generic;

namespace CalculoMelhorRota.Domain.Interfaces
{
    public interface IRotasRepository
    {
        List<Rotas> GetRotas();
        void Insert(List<Rotas> rotasInseridas);
    }
}
