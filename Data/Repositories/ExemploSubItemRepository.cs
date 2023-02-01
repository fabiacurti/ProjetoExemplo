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
    public class ExemploSubItemRepository : IExemploSubItemRepository
    {
        private readonly Func<IDbConnection> _connection;
        private readonly ILogger<ExemploSubItem> _log;
        private readonly IExportarExcel _exportarExcel;

        public ExemploSubItemRepository(Func<IDbConnection> connection,
            ILogger<ExemploSubItem> log,
            IExportarExcel exportarExcel)
        {
            _connection = connection;
            _log = log;
            _exportarExcel = exportarExcel;
        }

        public async Task<ExemploSubItem> Add(ExemploSubItem exemploSubItem)
        {
            try
            {
                exemploSubItem.Id = Guid.NewGuid();

                const string sql_script = @"
                    INSERT INTO ExemploSubItem (
                    Id, 
                    Descricao,
                    Ordem,
                    ExemploId,
                    Ativo
                    )
                    VALUES (
                    @id,
                    @descricao,
                    (SELECT MAX(Ordem) FROM ExemploSubItem),
                    @exemploId,
                    @ativo
                    );
                ";
                using (IDbConnection connection = _connection.Invoke())
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@id", exemploSubItem.Id);
                    parametros.Add("@descricao", exemploSubItem.Descricao);
                    parametros.Add("@exemploId", exemploSubItem.ExemploId);
                    parametros.Add("@ativo", exemploSubItem.Ativo);

                    await connection.ExecuteAsync(sql_script, parametros);
                    return exemploSubItem;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, Resources.ERRO_REPOSITORIO);
                return null;
            }
        }

        public async Task<ExemploSubItem> Update(ExemploSubItem exemploSubItem)
        {
            try
            {
                const string sql_script = @"
                    UPDATE ExemploSubItem SET
                    Descricao = @descricao,
                    Ativo = @ativo
                    WHERE Id = @id;
                ";
                using (IDbConnection connection = _connection.Invoke())
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@id", exemploSubItem.Id);
                    parametros.Add("@descricao", exemploSubItem.Descricao);
                    parametros.Add("@ativo", exemploSubItem.Ativo);

                    await connection.ExecuteAsync(sql_script, parametros);
                    return exemploSubItem;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, Resources.ERRO_REPOSITORIO);
                return null;
            }
        }

        public async Task<IEnumerable<ExemploSubItem>> GetAll(bool listaCompleta = false)
        {
            try
            {
                string sql_script = @"
                    SELECT
                    Id, 
                    Descricao,
                    Ordem,
                    ExemploId,
                    Ativo
                    FROM ExemploSubItem
                ";
                if (!listaCompleta)
                    sql_script += " WHERE Ativo = 1";

                using (IDbConnection connection = _connection.Invoke())
                {
                    return await connection.QueryAsync<ExemploSubItem>(sql_script);
                }
            } 
            catch (Exception ex)
            {
                _log.LogError(ex, Resources.ERRO_REPOSITORIO);
                return null;
            }
        }

        public async Task<bool> ExemploSubItemExistente(Guid? id, string descricao)
        {
            try
            {
                string sql_script = @"
                    SELECT
                    Id
                    FROM ExemploSubItem
                    WHERE
                    Id = ISNULL(@id, Id) AND
                    Descricao LIKE @descricao;
                ";
                
                using (IDbConnection connection = _connection.Invoke())
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@id", id);
                    parametros.Add("@descricao", descricao);
                    return (await connection.QueryAsync<ExemploSubItem>(sql_script, parametros)).Any();
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
                "Ordem",
                "AtivoFormatado"
            };
            return _exportarExcel.Excel("ExemploSubItens.xlsx", "Exemplo Sub Itens", lista, props, caminho);
        }

        public async void DeleteByExemploId(Guid exemploId)
        {
            try
            {
                string sql_script = @"DELETE FROM ExemploSubItem WHERE Id = @id;";

                using (IDbConnection connection = _connection.Invoke())
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@id", exemploId);
                    await connection.ExecuteAsync(sql_script, parametros);
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, Resources.ERRO_REPOSITORIO);
            }
        }
    }
}
