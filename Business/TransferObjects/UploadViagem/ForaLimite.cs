using System;
using System.Text.Json.Serialization;

namespace Business.TransferObjects.UploadViagem
{
    public class ForaLimite
    {

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("id_industria")]
        public int IdIndustria { get; set; }

        [JsonPropertyName("safra")]
        public int Safra { get; set; }

        [JsonPropertyName("data_movimento")]
        public DateTime DataMovimento { get; set; }

        [JsonPropertyName("certificado_pesagem")]
        public int CertificadoPesagem { get; set; }

        [JsonPropertyName("carregado")]
        public Validacao Validacao { get; set; }
    }

    public class Validacao
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("mensagem")]
        public string Mensagem { get; set; }

    }
}

