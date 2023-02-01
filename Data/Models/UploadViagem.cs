using System;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Data.Models
{
    public class UploadViagem
    {
        [JsonPropertyName("id_industria")]
        public int IdIndustria { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("validado")]
        public bool Validado { get; set; }
        [JsonPropertyName("safra")]
        public int Safra { get; set; }
        [JsonPropertyName("data_movimento")]
        public DateTime DataMovimento { get; set; }
        [JsonPropertyName("id_produtor")]
        public string IdProdutor { get; set; }
        [JsonPropertyName("nome_produtor")]
        public string NomeProdutor { get; set; }
        [JsonPropertyName("cpfcnpj_produtor")]
        public string CpfCnpjProdutor { get; set; }
        [JsonPropertyName("id_cidade_produtor")]
        public string IdCidadeProdutor { get; set; }

        //Tipos de produtor possíveis
        //- P(Própria Unidade Industrial ou Agropecuária vinculada a esta)
        //- A(Acionista da Unidade Industrial)
        //- T(Produtor Terceiro independente da Unidade Industrial)
        //- U(Outra Unidade Industrial entregando)
        //- O(Outras origens, cana SPOT)
        //- E(cana denominada “Cana Energia”)
        [JsonPropertyName("tipo_produtor")]
        public string TipoProdutor { get; set; }

        [JsonPropertyName("id_fundo_agricola")]
        public string IdFundoAgricola { get; set; }
        [JsonPropertyName("nome_fundo_agricola")]
        public string NomeFundoAgricola { get; set; }
        [JsonPropertyName("cnpj_fundo_agricola")]
        public string CnpjFundoAgricola { get; set; }
        [JsonPropertyName("id_cidade_fundo_agricola")]
        public string IdCidadeFundoAgricola { get; set; }
       
        //Tipos de fundo agrícola possíveis
        //- P(Parceria Agrícola)
        //- F(Fornecimento)
        [JsonPropertyName("tipo_fundo_agricola")]
        public string TipoFundoAgricola { get; set; }
        [JsonPropertyName("id_associacao")]
        public int IdAssociacao { get; set; }
        [JsonPropertyName("atrr")]
        public bool Atrr { get; set; }
        [JsonPropertyName("fator_qualidade")]
        public bool FatorQualidade { get; set; }
        [JsonPropertyName("certificado_pesagem")]
        public int CertificadoPesagem { get; set; }
        [JsonPropertyName("peso_liquido")]
        public int PesoLiquido { get; set; }
        [JsonPropertyName("data_entrada")]
        public DateTime DataEntrada { get; set; }
        [JsonPropertyName("amostrada")]
        public bool Amostrada { get; set; }
        [JsonPropertyName("boletim_analise")]
        public BigInteger? BoletimAnalise { get; set; }
        
        //Tipo de Clarificantes Possíveis 
        //- aluminio;
        //- sugarpol;
        //- nir;
        //- filterpol;
        //- claripol;
        //- octapol
        [JsonPropertyName("clarificante")]
        public string Clarificante { get; set; }
        [JsonPropertyName("brix_caldo")]
        public decimal? BrixCaldo { get; set; }
        [JsonPropertyName("leitura_sacarimetrica_original")]
        public decimal? LeituraSacarimetricaOriginal { get; set; }
        [JsonPropertyName("leitura_sacarimetrica_corrigida")]
        public decimal? LeituraSacarimetricaCorrigida { get; set; }
        [JsonPropertyName("pbu")]
        public decimal? Pbu { get; set; }
        [JsonPropertyName("codigo_ocorrencia")]
        public int? CodigoOcorrencia { get; set; }
    }
}
