using AutoMapper;
using Business.TransferObjects;
using Data.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Hubs
{
    public class ExemploHub : Hub
    {
        private readonly IExemploRepository _repo;
        private readonly IMapper _mapper;

        public ExemploHub(IExemploRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExemploDto>> ConsultarExemplos()
        {
            var retornoGeral = new
            {
                mensagem = "Teste de retorno"
            };
            var json = JsonSerializer.Serialize(retornoGeral);
            await Clients.All.SendAsync("RetornoExemplo", json);
            var retornoEspecifico = new
            {
                mensagem = "Retorno específico"
            };
            var jsonEspecifico = JsonSerializer.Serialize(retornoEspecifico);
            await Clients.Client(Context.ConnectionId).SendAsync("RetornoExemplo", jsonEspecifico);
            return _mapper.Map<List<ExemploDto>>(await _repo.GetAll(true));
        }
    }
}
