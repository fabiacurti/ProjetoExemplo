using System;
using System.Text.Json.Serialization;

namespace Data.Models
{
    public class Viagem
    {
        [JsonPropertyName("id_produtor")]
        public string IdProdutor { get; set; }
        
    }
}
