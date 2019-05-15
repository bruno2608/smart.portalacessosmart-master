using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Routing;

namespace Arquitetura.Components
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DontValidateApiAntiForgeryRulesAttribute : FilterAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ValidateApiAntiForgeryRulesAttribute : FilterAttribute, IAuthorizationFilter
    {
        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (!actionContext.ActionDescriptor.GetCustomAttributes<DontValidateApiAntiForgeryRulesAttribute>().Any())
            {
                WebReturn<bool> retr = new WebReturn<bool>();
                try
                {
                    if (actionContext == null)
                    {
                        throw new ArgumentNullException("actionContext null ( ExecuteAuthorizationFilterAsync -> ValidateJsonAntiForgeryTokenAttribute )");
                    }

                    AntiForgery.Validate(actionContext.Request.Headers.GetCookies(AntiForgeryConfig.CookieName).FirstOrDefault()[AntiForgeryConfig.CookieName].Value, actionContext.Request.Headers.GetValues("__RequestVerificationToken").FirstOrDefault());
                }
                catch (Exception ex)
                {
                    retr.Code = 1;
                    retr.Message = ex.Message;
                    Utilitarios.CriaLogErro(ex);
                    Utilitarios.InserirLog(ex,
                                          System.Reflection.MethodInfo.GetCurrentMethod().Name,
                                          string.Join(";", System.Reflection.MethodInfo.GetCurrentMethod().GetParameters().Select(val => val.Name)),
                                          ex.GetType().Name,
                                          "ERRO NO METODO ExecuteAuthorizationFilterAsync.!!");

                    var response = actionContext.Request.CreateResponse(HttpStatusCode.Moved);
                    //response.Headers.Location = actionContext.Request.RequestUri;
                    response.Headers.Location = new Uri(new UrlHelper().Content("~") + "/Paginas/Login/Index");
                    return FromResult(response);
                }
            }

            return continuation();
        }

        private Task<HttpResponseMessage> FromResult(HttpResponseMessage result)
        {
            var source = new TaskCompletionSource<HttpResponseMessage>();
            source.SetResult(result);
            return source.Task;
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ValidateApiModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext null ( OnActionExecuting -> ValidateModelAttribute )");
            }

            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext contex)
        {
            contex.Response = contex.Request.CreateResponse(HttpStatusCode.InternalServerError, string.Concat(contex.Exception.Message, " - ", contex.Exception.StackTrace));
        }
    }
}