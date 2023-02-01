using System;

namespace Business.TransferObjects
{
    public class ExemploSubItemDto
	{
		public Guid? Id { get; set; }
		public string Descricao { get; set; }
		public int? Ordem { get; set; }
		public Guid? ExemploId { get; set; }
		public bool Ativo { get; set; }
		public string AtivoFormatado { get { return Ativo ? "Ativo" : "Inativo"; } }
	}
}
