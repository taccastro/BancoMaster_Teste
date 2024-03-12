using CalculoMelhorRota.Domain.Entity;
using FluentValidation;
using System.Collections.Generic;

namespace CalculoMelhorRota.Domain.Validation
{
    public class RotasValidation : AbstractValidator<Rotas>
    {
        public RotasValidation()
        {
            #region Atributos
            RuleFor(c => c.Origem)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido.");

            RuleFor(c => c.Destino)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido.");

            RuleFor(c => c.Valor)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido.");
            #endregion

        }
    }

    class RotasCollectionValidator : AbstractValidator<IEnumerable<Rotas>>
    {
        public RotasCollectionValidator()
        {
            RuleForEach(list => list).SetValidator(new RotasValidation());
        }
    }
}
