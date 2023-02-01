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
    public class ExemploSubItemService : IExemploSubItemService
    {
        private readonly IMapper _mapper;
        private readonly IExemploSubItemRepository _repo;
        public ExemploSubItemService(IMapper mapper,
            IExemploSubItemRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
        public async Task<ExemploSubItemDto> Add(ExemploSubItemDto exemplo)
        {
            var exemploSubItemAdd = await _repo.Add(_mapper.Map<ExemploSubItem>(exemplo));
            return _mapper.Map<ExemploSubItemDto>(exemploSubItemAdd);
        }
        public async Task<ExemploSubItemDto> Update(ExemploSubItemDto exemplo)
        {
            var exemploUpdate = await _repo.Update(_mapper.Map<ExemploSubItem>(exemplo));
            return _mapper.Map<ExemploSubItemDto>(exemploUpdate);
        }
        public async Task<IEnumerable<ExemploSubItemDto>> GetAll(bool listaCompleta = false)
        {
            return _mapper.Map<List<ExemploSubItemDto>>(await _repo.GetAll(listaCompleta));
        }
        public async Task<bool> ExemploSubItemExistente(Guid? id, string descricao)
        {
            return await _repo.ExemploSubItemExistente(id, descricao);
        }
        public async Task<MemoryStream> ExportarExcel(string caminho)
        {
            return await _repo.ExportarExcel(caminho);
        }
    }
}
