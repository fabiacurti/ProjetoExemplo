using Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace API.Filters
{
    public class ApplicationFilter : ActionFilterAttribute
    {
        private readonly IExemploRepository _repo;
        public ApplicationFilter(IExemploRepository repo)
        {
            _repo = repo;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionDescriptor = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor;
            var controllerName = actionDescriptor.ControllerName;
            var actionName = actionDescriptor.ActionName;
            if (controllerName == "Exemplo" && actionName == "ValidarCabecalho")
            {
                StringValues cabecalho;
                var headerValue = context.HttpContext.Request.Headers.TryGetValue("cabecalho", out cabecalho);
                if (!headerValue)
                    context.Result = new CustomUnauthorizedResult("Cabeçalho não encontrado.");
            }
        }
        public class CustomUnauthorizedResult : JsonResult
        {
            public CustomUnauthorizedResult(string message)
                : base(new CustomError(message))
            {
                StatusCode = StatusCodes.Status401Unauthorized;
            }
        }
        public class CustomError
        {
            public string Error { get; }

            public CustomError(string message)
            {
                Error = message;
            }
        }
    }
}
