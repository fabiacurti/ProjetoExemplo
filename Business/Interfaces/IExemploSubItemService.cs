using Business.TransferObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IExemploSubItemService
    {
        Task<ExemploSubItemDto> Add(ExemploSubItemDto exemplo);
        Task<ExemploSubItemDto> Update(ExemploSubItemDto exemplo);
        Task<IEnumerable<ExemploSubItemDto>> GetAll(bool listaCompleta = false);
        Task<bool> ExemploSubItemExistente(Guid? id, string descricao);
        Task<MemoryStream> ExportarExcel(string caminho);
    }
}
