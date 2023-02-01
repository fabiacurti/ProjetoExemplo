using System;       
using System.Text.Json.Serialization;

namespace Business.TransferObjects.UploadViagem
{
    public class NaoHabilidato
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("id_industria")]
        public int IdIndustria { get; set; }

        [JsonPropertyName("safra")]
        public int Safra { get; set; }

        [JsonPropertyName("data_movimento")]
        public DateTime DataMovimento {get;set;}

        [JsonPropertyName("certificado_pesagem")]
        public int CertificadoPesagem { get; set; }

        [JsonPropertyName("carregado")]
        public bool Carregado { get; set; }
    }
}
