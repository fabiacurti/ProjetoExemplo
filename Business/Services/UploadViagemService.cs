using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UploadViagemService : IUploadViagemService
    {
        private readonly IMapper _mapper;
        private readonly IUploadViagemRepository _repo;
        private static HttpClient _httpClient;
        private static HttpClient HttpClient => _httpClient ?? (_httpClient = new HttpClient());

        public UploadViagemService(IMapper mapper,
            IUploadViagemRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<IEnumerable<UploadViagem>> UploadViagens()
        {
            var viagens = await _repo.Get();
            foreach (var viagem in viagens)
            {
                var json = JsonSerializer.Serialize(viagem);
                HttpResponseMessage response = await HttpClient.PostAsync(
                    "https://homologacao.sistemaatr.com.br/upload-viagem",
                    new StringContent(json, Encoding.UTF8, "application/json")
                );
                var teste = response.Content.ReadAsStringAsync();
                var teste2 = JsonSerializer.Deserialize<ErrorResult>(teste.Result);
                if (response.IsSuccessStatusCode)
                {
                    string responseBodyAsText = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine("falha");
                }
            }
            return viagens;
        }
    }
}
//teste
