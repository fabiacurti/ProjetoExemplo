using Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IExemploRepository
    {
        Task<Exemplo> Add(Exemplo exemplo);
        Task<Exemplo> Update(Exemplo exemplo);
        Task<IEnumerable<Exemplo>> GetAll(bool listaCompleta = false);
        Task<bool> ExemploExistente(Guid? id, string descricao);
        Task<MemoryStream> ExportarExcel(string caminho);
    }
}
