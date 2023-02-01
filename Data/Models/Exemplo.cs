using System;
using System.Collections.Generic;
using System.Globalization;

namespace Data.Models
{
    public class Exemplo
    {
		public Guid Id { get; set; }
		public string Descricao { get; set; }
		public DateTime DataHora { get; set; }
		public int Quantidade { get; set; }
		public double Valor { get; set; }
		public bool Ativo { get; set; }
		public string DataHoraFormatada { get { return DataHora.ToString("dd/MM/yyyy HH:mm:ss"); } }
		public string AtivoFormatado { get { return Ativo ? "Ativo" : "Inativo"; } }
		public string ValorFormatado { get { return string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Valor); } }
		public IEnumerable<ExemploSubItem> SubItens { get; set; }
	}
}
