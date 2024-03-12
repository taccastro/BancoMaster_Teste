using AutoMapper;
using CalculoMelhorRota.Application.Interfaces.AppServices;
using CalculoMelhorRota.Application.ViewsModels;
using CalculoMelhorRota.CrossCutting.Util.Configs;
using CalculoMelhorRota.Domain.Entity;
using CalculoMelhorRota.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace CalculoMelhorRota.Application.AppServices
{
    public class RotasAppService : GlobalAppService, IRotasAppService
    {
        private readonly IRotasService _rotaService;
        private readonly IMapper _mapper;

        public RotasAppService(INotifier notifier, IRotasService rotasService, IMapper mapper) : base(notifier)
        {
            _rotaService = rotasService;
            _mapper = mapper;
        }

        public IEnumerable<RotasViewModel> AdicionarRotas(IEnumerable<RotasViewModel> rotas, CancellationToken cancellationToken)
        {
            try
            {
                var rotasResult = _rotaService.AdicionarRotas(_mapper.Map<IEnumerable<Rotas>>(rotas));
                var result = _mapper.Map<IEnumerable<RotasViewModel>>(rotasResult);
                return result;
            }
            catch (Exception ex)
            {
                ErrorHttp = (int)HttpStatusCode.InternalServerError;
                Notification(ex.Message);
                return null;
            }
        }

        public string MelhorRota(string rotas, CancellationToken cancellationToken)
        {
            try
            {
                var rotasResult = _rotaService.MelhorRota(rotas);
                return rotasResult;
            }
            catch (Exception ex)
            {
                ErrorHttp = (int)HttpStatusCode.InternalServerError;
                Notification(ex.Message);
                return null;
            }
        }
    }
}
