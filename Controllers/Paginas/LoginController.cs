using Arquitetura.Components;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;

[RoutePrefix("api/Login")]
public class LoginController : ApiController, IRequiresSessionState
{
    [HttpGet]
    [Route("Deslogar")]
    [DontAuthorizeFilterApi]
    [DontValidateApiAntiForgeryRules]
    public IHttpActionResult Deslogar()
    {
        if (HttpContext.Current.Session != null)
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
        return Redirect(new Uri(Url.Content("~/Paginas/Login/Index")));
    }

    [HttpPost]
    [Route("EfetuarLogin")]
    [DontAuthorizeFilterApi]
    public LoginApiModel.RetornoEfetuarLogin EfetuarLogin(LoginApiModel.EfetuarLogin parametros)
    {
        var ret = new LoginApiModel.RetornoEfetuarLogin();
        // 
        var grupos = ControleDeAcesso.AutenticaUsuario(parametros.USUARIO, parametros.SENHA);

        if (grupos == null || grupos != null && grupos.Count > 0)
        {
            var loginObj = ControleDeAcesso.ObterConjuntoDePermissoesUsuario(HttpContext.Current.Session, parametros.USUARIO, grupos);
            if (loginObj.InformacoesUsuario == null)
                ret.MENSAGEM = "Usuário não existente na base de dados.";
            else
            {
                // 
                if ((grupos != null))
                {
                    List<string> GruposSmart = new List<string>();
                    // 
                    grupos.ForEach(Item =>
                    {
                        if ((Item.ToUpper().StartsWith("SMART.")))
                            GruposSmart.Add(Item);
                    });
                    // 
                    if ((GruposSmart != null && GruposSmart.Count > 0))
                    {
                        ControleDeAcessoBLL BLL = new ControleDeAcessoBLL();
                        BLL.ValidarGrupoSistemas(loginObj.InformacoesUsuario.CODFNC, GruposSmart);
                        // 
                        loginObj = ControleDeAcesso.ObterConjuntoDePermissoesUsuario(HttpContext.Current.Session, parametros.USUARIO, grupos);
                    }
                }
                // 
                if (loginObj.SistemasPermitidos.Count == 0)
                    ret.MENSAGEM = "Usuário sem permissão de acesso associada.";
                else
                    ret.CODIGO = 1;
            }
        }
        else
            ret.MENSAGEM = "Usuário ou senha incorretos.";
        // 
        return ret;
    }
}
