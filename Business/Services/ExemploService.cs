using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ExemploService : IExemploService
    {
        private readonly IMapper _mapper;
        private readonly IExemploRepository _repo;
        private readonly IExemploSubItemRepository _exemploSubItemRepo;
        public ExemploService(IMapper mapper,
            IExemploRepository repo,
            IExemploSubItemRepository exemploSubItemRepo)
        {
            _mapper = mapper;
            _repo = repo;
            _exemploSubItemRepo = exemploSubItemRepo;
        }
        public async Task<ExemploDto> Add(ExemploDto exemplo)
        {
            var exemploAdd = await _repo.Add(_mapper.Map<Exemplo>(exemplo));
            foreach(var subItem in exemploAdd.SubItens)
            {
                subItem.ExemploId = exemploAdd.Id;
                var subItemAdd = await _exemploSubItemRepo.Add(subItem);
                subItem.Id = subItemAdd.Id;
                subItem.Ordem = subItemAdd.Ordem;
            }
            return _mapper.Map<ExemploDto>(exemploAdd);
        }
        public async Task<ExemploDto> Update(ExemploDto exemplo)
        {
            var exemploUpdate = await _repo.Update(_mapper.Map<Exemplo>(exemplo));
            _exemploSubItemRepo.DeleteByExemploId(exemploUpdate.Id);
            foreach (var subItem in exemploUpdate.SubItens)
            {
                subItem.ExemploId = exemploUpdate.Id;
                var subItemAdd = await _exemploSubItemRepo.Add(subItem);
                subItem.Id = subItemAdd.Id;
                subItem.Ordem = subItemAdd.Ordem;
            }
            return _mapper.Map<ExemploDto>(exemploUpdate);
        }
        public async Task<IEnumerable<ExemploDto>> GetAll(bool listaCompleta = false)
        {
            return _mapper.Map<List<ExemploDto>>(await _repo.GetAll(listaCompleta));
        }
        public async Task<bool> ExemploExistente(Guid? id, string descricao)
        {
            return await _repo.ExemploExistente(id, descricao);
        }
        public async Task<MemoryStream> ExportarExcel(string caminho)
        {
            return await _repo.ExportarExcel(caminho);
        }
    }
}
