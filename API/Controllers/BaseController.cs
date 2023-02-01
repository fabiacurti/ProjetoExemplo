using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using System;

namespace API.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        protected void LogError(Exception ex, string mensagem)
        {
            _logger.LogError(ex, mensagem);
        }

        protected void LogExecucao(string mensagem)
        {
            try
            {
                var usuario = Request.Headers["Usuario"].ToString();
                var logger = LogManager.GetCurrentClassLogger();
                var eventInfo = new LogEventInfo(NLog.LogLevel.Info, logger.Name, mensagem);
                eventInfo.Properties.Add("usuario", usuario);
                logger.Log(eventInfo);
            }
            catch(Exception ex)
            {
                LogError(ex, "Erro ao registrar log de execução");
            }
        }
    }
}
