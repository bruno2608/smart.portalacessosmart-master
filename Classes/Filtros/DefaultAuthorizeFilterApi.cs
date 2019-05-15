using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Linq;
using System.Web;

public class DontAuthorizeFilterApiAttribute : FilterAttribute
{
}

public class DefaultAuthorizeFilterApiAttribute : AuthorizeAttribute
{
    protected override bool IsAuthorized(HttpActionContext actionContext)
    {
        if (actionContext.ActionDescriptor.GetCustomAttributes<DontAuthorizeFilterApiAttribute>().Any())
        {
            return true;
        }
        return ControleDeAcesso.ObterConjuntoDePermissoesUsuario(HttpContext.Current.Session).InformacoesUsuario != null;
    }
}