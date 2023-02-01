using System;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.TransferObjects;
using Business.TransferObjects.Mensagens;
using Data.Constantes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExemploSubItemController : BaseController
    {
        public ExemploSubItemController(ILogger<ExemploSubItemController> _logger) : base(_logger) { }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromServices] IExemploSubItemService _service,
            [FromQuery] bool listaCompleta)
        {
            try
            {
                return Ok(await _service.GetAll(listaCompleta));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao buscar lista de exemplos.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExemploSubItemDto exemplo,
            [FromServices] IExemploSubItemService _service)
        {
            try
            {
                var validacao = await _service.ExemploSubItemExistente(exemplo.Id, exemplo.Descricao);

                if (validacao)
                    return Conflict(new MensagemErroDto("Exemplo sub item já cadastrado."));

                LogExecucao($"ExemploSubItem.Add : Descrição - {exemplo.Descricao}");
                return Ok(await _service.Add(exemplo));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao adicionar exemplo sub item.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] ExemploSubItemDto exemplo,
            [FromServices] IExemploSubItemService _service)
        {
            try
            {
                var validacao = await _service.ExemploSubItemExistente(exemplo.Id.Value, exemplo.Descricao);

                if (validacao)
                    return Conflict(new MensagemErroDto("Existe outro exemplo sub item com a mesma descrição."));

                LogExecucao($"ExemploSubItem.Update : ID - {exemplo.Id.Value} / Descrição - {exemplo.Descricao}");
                return Ok(await _service.Update(exemplo));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao atualizar exemplo sub item.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPost("exemplosExcel")]
        public async Task<IActionResult> AreasExcel(
            [FromServices] IExemploSubItemService _service,
            [FromServices] IWebHostEnvironment _hostingEnvironment)
        {
            string caminho = _hostingEnvironment.ContentRootPath;
            var stream = await _service.ExportarExcel(caminho);
            return this.File(
                fileContents: stream.ToArray(),
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "ExemploSubItens.xlsx"
            );
        }
    }
}
