using Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IExemploSubItemRepository
    {
        Task<ExemploSubItem> Add(ExemploSubItem exemplo);
        Task<ExemploSubItem> Update(ExemploSubItem exemplo);
        Task<IEnumerable<ExemploSubItem>> GetAll(bool listaCompleta = false);
        Task<bool> ExemploSubItemExistente(Guid? id, string descricao);
        Task<MemoryStream> ExportarExcel(string caminho);
        void DeleteByExemploId(Guid exemploId);
    }
}
