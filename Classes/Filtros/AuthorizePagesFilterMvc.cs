using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class AuthorizePagesFilterMvc : AuthorizeAttribute
{

    public ActionDescriptor actionDesc { get; set; }

    protected override bool AuthorizeCore(HttpContextBase hContext)
    {
        var infoUsr = ControleDeAcesso.ObterConjuntoDePermissoesUsuario(HttpContext.Current.Session);
        if (hContext.Session["URLsAutorizadas"] != null)
        {
            //
            var URLs = (List<string>)(hContext.Session["URLsAutorizadas"]);
            if (URLs.Where(x => x.Equals(hContext.Request.Url.AbsolutePath)).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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