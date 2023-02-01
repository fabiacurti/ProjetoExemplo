using Business.TransferObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IExemploService
    {
        Task<ExemploDto> Add(ExemploDto exemplo);
        Task<ExemploDto> Update(ExemploDto exemplo);
        Task<IEnumerable<ExemploDto>> GetAll(bool listaCompleta = false);
        Task<bool> ExemploExistente(Guid? id, string descricao);
        Task<MemoryStream> ExportarExcel(string caminho);
    }
}
