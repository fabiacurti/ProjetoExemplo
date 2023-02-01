using System.Collections.Generic;
using System.IO;

namespace Data.Interfaces
{
    public interface IExportarExcel
    {
        MemoryStream Excel<T>(string nome, string nomePrimeiraAba, IEnumerable<T> lista, List<string> propriedades, string caminho);
    }
}
