using System.Web.Http;
using System.Security.Cryptography;
using Arquitetura.Components;

[RoutePrefix("api/Portal")]
public class PortalController : ApiController
{
    [HttpPost]
    [Route("TransicionarSistema")]
    public PortalApiModel.RetornoTransicionarSistema TransicionarSistema(PortalApiModel.TransicionarSistema parametro)
    {
        return Encryption.TransicionarSistema(parametro);
    }

    [HttpPost]
    [Route("ObterInformacoesUsuario")]
    [DontAuthorizeFilterApi]
    [DontValidateApiAntiForgeryRules]
    public ControleDeAcesso.ConjuntoDePermissoesUsuarioModulos ObterInformacoesUsuario(PortalApiModel.ObterInformacoesUsuario parametro)
    {
        return Encryption.ObterInformacoesUsuario(parametro);
    }
}
