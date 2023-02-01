using ClosedXML.Excel;
using Data.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Data.Util
{
    public class ExportarExcel : IExportarExcel
    {
        public MemoryStream Excel<T>(string nome, string nomePrimeiraAba, IEnumerable<T> lista, List<string> propriedades, string caminho)
        {
            var workbook = new XLWorkbook(string.Concat(caminho, "\\Templates\\", nome));
            var worksheet = workbook.Worksheets.First(x => x.Name == nomePrimeiraAba);

            var linhaInicial = 3;
            var linhaAtual = linhaInicial;
            var quantidadeColunas = propriedades.Count();

            foreach (var item in lista)
            {
                linhaAtual++;
                for (var i = 0; i < quantidadeColunas; i++)
                {
                    worksheet.Cell(linhaAtual, (i + 2)).SetValue(item.GetType().GetProperty(propriedades[i]).GetValue(item));
                }
            }

            linhaInicial++;
            worksheet.Range(2, 2, 2, (quantidadeColunas + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            worksheet.Range(2, 2, 2, (quantidadeColunas + 1)).Style.Border.OutsideBorderColor = XLColor.FromArgb(205, 205, 205);
            worksheet.Range(linhaInicial, 2, linhaAtual, quantidadeColunas + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            worksheet.Range(linhaInicial, 2, linhaAtual, quantidadeColunas + 1).Style.Border.OutsideBorderColor = XLColor.FromArgb(205, 205, 205);
            worksheet.Range(linhaInicial, 2, linhaAtual, quantidadeColunas + 1).Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            worksheet.Range(linhaInicial, 2, linhaAtual, quantidadeColunas + 1).Style.Border.InsideBorderColor = XLColor.FromArgb(205, 205, 205);
            worksheet.Range(linhaInicial, 2, linhaAtual, quantidadeColunas + 1).Style.Font.FontColor = XLColor.FromArgb(80, 80, 80);
            worksheet.Range(linhaInicial, 2, linhaAtual, quantidadeColunas + 1).Style.Font.FontSize = 14;
            worksheet.Range(linhaInicial, 2, linhaAtual, quantidadeColunas + 1).Style.Font.FontName = "Segoe UI Light";
            worksheet.Range(linhaInicial, 2, linhaAtual, quantidadeColunas + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Columns(2, (quantidadeColunas + 1)).AdjustToContents();

            var ms = new MemoryStream();
            workbook.SaveAs(ms);
            workbook.Dispose();

            ms.Position = 0;

            return ms;
        }
    }
}
