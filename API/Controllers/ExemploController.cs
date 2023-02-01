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
    public class ExemploController : BaseController
    {
        public ExemploController(ILogger<ExemploController> _logger) : base(_logger) { }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromServices] IExemploService _service,
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
        public async Task<IActionResult> Post([FromBody] ExemploDto exemplo,
            [FromServices] IExemploService _service)
        {
            try
            {
                var validacao = await _service.ExemploExistente(exemplo.Id, exemplo.Descricao);

                if (validacao)
                    return Conflict(new MensagemErroDto("Exemplo já cadastrado."));

                LogExecucao($"Exemplo.Add : Descrição - {exemplo.Descricao}");
                return Ok(await _service.Add(exemplo));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao adicionar exemplo.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] ExemploDto exemplo,
            [FromServices] IExemploService _service)
        {
            try
            {
                var validacao = await _service.ExemploExistente(exemplo.Id.Value, exemplo.Descricao);

                if (validacao)
                    return Conflict(new MensagemErroDto("Existe outro exemplo com a mesma descrição."));

                LogExecucao($"Exemplo.Update : ID - {exemplo.Id.Value} / Descrição - {exemplo.Descricao}");
                return Ok(await _service.Update(exemplo));
            }
            catch (Exception ex)
            {
                LogError(ex, "Erro ao atualizar exemplo.");
                return BadRequest(new MensagemErroDto(Resources.ERRO_EXEC_METODO + ex.Message));
            }
        }

        [HttpGet("validarCabecalho")]
        public IActionResult ValidarCabecalho()
        {
            //Este é um exemplo de aplicação para o a classe "ApplicationFilter"
            return Ok("Exemplo de mensagem");
        }

        [HttpPost("exemplosExcel")]
        public async Task<IActionResult> AreasExcel(
            [FromServices] IExemploService _service,
            [FromServices] IWebHostEnvironment _hostingEnvironment)
        {
            string caminho = _hostingEnvironment.ContentRootPath;
            var stream = await _service.ExportarExcel(caminho);
            return this.File(
                fileContents: stream.ToArray(),
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "Exemplos.xlsx"
            );
        }
    }
}
