using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API
{
    /// <summary>
    /// 全局异常处理Filter
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private IHostingEnvironment _environment;
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(IHostingEnvironment env, ILogger<GlobalExceptionFilter> logger)
        {
            _environment = env;
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            var jsonError = new JsonErrorResponse();
            if (_environment.IsDevelopment())
            {
                jsonError.DeveloperMessage = context.Exception.StackTrace;
            }
            if(context.Exception is UserOperationException)
            {
                //已知的自定义异常类型
                jsonError.Messaage = context.Exception.Message;
                context.Result = new BadRequestObjectResult(jsonError);
            }
            else
            {
                //未知的异常类型
                jsonError.Messaage = "发生了未知内部错误";
                context.Result = new InternalServerErrorObjectResult(jsonError);
            }
            _logger.LogError(context.Exception, jsonError.Messaage);
        }
    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error) : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
