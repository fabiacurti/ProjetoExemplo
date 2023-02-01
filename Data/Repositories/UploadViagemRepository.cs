using Dapper;
using Data.Constantes;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UploadViagemRepository : IUploadViagemRepository
    {
        private readonly Func<IDbConnection> _connection;
        private readonly ILogger<UploadViagem> _log;

        public UploadViagemRepository(Func<IDbConnection> connection,
            ILogger<UploadViagem> log)
        {
            _connection = connection;
            _log = log;
        }

        public async Task<IEnumerable<UploadViagem>>Get()
        {
            try
            {
                string sql_script = @"
                    SELECT
                    IdIndustria,
	                Token,
                    Validado,
                    Safra ,
                    DataMovimento,
	                IdProdutor ,
                    NomeProdutor,
                    CpfCnpjProdutor,
                    IdCidadeProdutor,
                    TipoProdutor,
	                IdFundoAgricola,
                    NomeFundoAgricola,
                    CnpjFundoAgricola,
                    IdCidadeFundoAgricola,
                    TipoFundoAgricola,
                    IdAssociacao,
                    Atrr,
                    FatorQualidade,
	                CertificadoPesagem,
	                PesoLiquido,
	                DataEntrada,
	                Amostrada,
	                BoletimAnalise,
	                Clarificante,
                    BrixCaldo,
	                LeituraSacarimetricaOriginal,
	                LeituraSacarimetricaCorrigida,
	                Pbu,
	                CodigoOcorrencia 
                    FROM UploadViagem
                ";

                using (IDbConnection connection = _connection.Invoke())
                {
                    return await connection.QueryAsync<UploadViagem>(sql_script);
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, Resources.ERRO_REPOSITORIO);
                return null;
            }
        }
    }
}
