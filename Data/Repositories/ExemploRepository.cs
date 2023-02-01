using Dapper;
using Data.Constantes;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ExemploRepository : IExemploRepository
    {
        private readonly Func<IDbConnection> _connection;
        private readonly ILogger<Exemplo> _log;
        private readonly IExportarExcel _exportarExcel;

        public ExemploRepository(Func<IDbConnection> connection,
            ILogger<Exemplo> log,
            IExportarExcel exportarExcel)
        {
            _connection = connection;
            _log = log;
            _exportarExcel = exportarExcel;
        }

        public async Task<Exemplo> Add(Exemplo exemplo)
        {
            try
            {
                exemplo.Id = Guid.NewGuid();

                const string sql_script = @"
                    INSERT INTO Exemplo (
                    Id, 
                    Descricao,
                    DataHora,
                    Quantidade,
                    Valor,
                    Ativo
                    )
                    VALUES (
                    @id,
                    @descricao,
                    @dataHora,
                    @quantidade,
                    @valor,
                    @ativo
                    );
                ";
                using (IDbConnection connection = _connection.Invoke())
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@id", exemplo.Id);
                    parametros.Add("@descricao", exemplo.Descricao);
                    parametros.Add("@dataHora", exemplo.DataHora);
                    parametros.Add("@quantidade", exemplo.Quantidade);
                    parametros.Add("@valor", exemplo.Valor);
                    parametros.Add("@ativo", exemplo.Ativo);

                    await connection.ExecuteAsync(sql_script, parametros);
                    return exemplo;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, Resources.ERRO_REPOSITORIO);
                return null;
            }
        }

        public async Task<Exemplo> Update(Exemplo exemplo)
        {
            try
            {
                const string sql_script = @"
                    UPDATE Exemplo SET
                    Descricao = @descricao,
                    DataHora = @dataHora,
                    Quantidade = @quantidade,
                    Valor = @valor,
                    Ativo = @ativo
                    WHERE Id = @id;
                ";
                using (IDbConnection connection = _connection.Invoke())
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@id", exemplo.Id);
                    parametros.Add("@descricao", exemplo.Descricao);
                    parametros.Add("@dataHora", exemplo.DataHora);
                    parametros.Add("@quantidade", exemplo.Quantidade);
                    parametros.Add("@valor", exemplo.Valor);
                    parametros.Add("@ativo", exemplo.Ativo);

                    await connection.ExecuteAsync(sql_script, parametros);
                    return exemplo;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, Resources.ERRO_REPOSITORIO);
                return null;
            }
        }

        public async Task<IEnumerable<Exemplo>> GetAll(bool listaCompleta = false)
        {
            try
            {
                string sql_script = @"
                    SELECT
                    Id, 
                    Descricao,
                    DataHora,
                    Quantidade,
                    Valor,
                    Ativo
                    FROM Exemplo
                ";
                if (!listaCompleta)
                    sql_script += " WHERE Ativo = 1";

                var exemplos = new List<Exemplo>();

                using (IDbConnection connection = _connection.Invoke())
                {
                    exemplos = (await connection.QueryAsync<Exemplo>(sql_script)).ToList();
                }

                foreach(var exemplo in exemplos)
                {
                    const string sql_sub_script = @"
                        SELECT
                        Id, 
                        Descricao,
                        Ordem,
                        ExemploId
                        FROM ExemploSubItem
                        WHERE ExemploId = @exemploId
                        ORDER BY Ordem
                    ";
                    using (IDbConnection connection = _connection.Invoke())
                    {
                        var parametros = new DynamicParameters();
                        parametros.Add("@exemploId", exemplo.Id);
                        exemplo.SubItens = (await connection.QueryAsync<ExemploSubItem>(sql_sub_script, parametros));
                    }
                }

                return exemplos;
            } 
            catch (Exception ex)
            {
                _log.LogError(ex, Resources.ERRO_REPOSITORIO);
                return null;
            }
        }

        public async Task<IEnumerable<Exemplo>> GetAllMatchAgainst(bool listaCompleta = false)
        {
            // A coluna pesquisada deve ter o indice FULL TEXT SEARCH
            try
            {
                string sql_script = @"
                    SELECT
                    Id, 
                    Descricao,
                    DataHora,
                    Quantidade,
                    Valor,
                    Ativo
                    FROM Exemplo
                ";
                if (!listaCompleta)
                    sql_script += " WHERE Ativo = 1";

                var exemplos = new List<Exemplo>();

                using (IDbConnection connection = _connection.Invoke())
                {
                    exemplos = (await connection.QueryAsync<Exemplo>(sql_script)).ToList();
                }

                foreach (var exemplo in exemplos)
                {
                    const string sql_sub_script = @"
                        SELECT
                        Id, 
                        Descricao,
                        Ordem,
                        ExemploId
                        MATCH(Descricao) AGAINST (@Descricao IN BOOLEAN MODE) as RankMatchAgainst
                        FROM ExemploSubItem
                        WHERE
                        MATCH(Descricao) AGAINST (@Descricao IN BOOLEAN MODE)
                        ORDER BY RankMatchAgainst
                    ";
                    using (IDbConnection connection = _connection.Invoke())
                    {
                        var parametros = new DynamicParameters();
                        parametros.Add("@Descricao", exemplo.Descricao);
                        exemplo.SubItens = (await connection.QueryAsync<ExemploSubItem>(sql_sub_script, parametros));
                    }
                }

                return exemplos;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, Resources.ERRO_REPOSITORIO);
                return null;
            }
        }

        public async Task<bool> ExemploExistente(Guid? id, string descricao)
        {
            try
            {
                string sql_script = @"
                    SELECT
                    Id
                    FROM Exemplo
                    WHERE
                    Id = ISNULL(@id, Id) AND
                    Descricao LIKE @descricao;
                ";
                
                using (IDbConnection connection = _connection.Invoke())
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@id", id);
                    parametros.Add("@descricao", descricao);
                    return (await connection.QueryAsync<Exemplo>(sql_script, parametros)).Any();
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, Resources.ERRO_REPOSITORIO);
                return false;
            }
        }

        public async Task<MemoryStream> ExportarExcel(string caminho)
        {
            var lista = await GetAll(true);
            var props = new List<string>() {
                "Descricao",
                "DataHoraFormatada",
                "Quantidade",
                "ValorFormatado",
                "AtivoFormatado"
            };
            return _exportarExcel.Excel("Exemplos.xlsx", "Exemplos", lista, props, caminho);
        }
    }
}
