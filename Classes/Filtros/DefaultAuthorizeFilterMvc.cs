using System.Linq;
using System.Web;
using System.Web.Mvc;


public class DontAuthorizeFilterMvcAttribute : FilterAttribute
{
}

public class DefaultAuthorizeFilterMvcAttribute : AuthorizeAttribute
{

    public ActionDescriptor actionDesc { get; set; }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        if (actionDesc.GetCustomAttributes(typeof(DontAuthorizeFilterMvcAttribute), false).Any())
        {
            return true;
        }
        if (ReferenceEquals(ControleDeAcesso.ObterConjuntoDePermissoesUsuario(HttpContext.Current.Session).InformacoesUsuario, null))
        {
            httpContext.Response.Redirect("~/Paginas/Login/Index");
            return false;
        }
        else
        {   
            return true;
        }
    }

    public override void OnAuthorization(AuthorizationContext filterContext)
    {
        actionDesc = filterContext.ActionDescriptor;
        base.OnAuthorization(filterContext);
    }

}