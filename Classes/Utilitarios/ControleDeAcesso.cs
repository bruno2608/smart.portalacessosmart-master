using System.DirectoryServices.Protocols;
using System.Net;
using System.DirectoryServices.AccountManagement;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public class ControleDeAcesso
{

    public class ParametrosCriptografia
    {
        public PortalApiModel.TransicionarSistema ParametrosTransicao { get; set; }
        public int CODFNC { get; set; }
    }

    private static List<PENDINGTOKEN> _PENDINGTOKENS;
    public static List<PENDINGTOKEN> PENDINGTOKENS
    {
        get
        {
            if (_PENDINGTOKENS == null)
                _PENDINGTOKENS = new List<PENDINGTOKEN>();
            else
                lock (_PENDINGTOKENS)
                    _PENDINGTOKENS.RemoveAll(x => x.DTEXP < DateTime.Now);
            // 
            return _PENDINGTOKENS;
        }
    }

    public class PENDINGTOKEN
    {
        public string PRIVATEKEY { get; set; }
        public string PUBLICKEY { get; set; }
        public DateTime DTEXP { get; set; }
    }


    private static int _CODSISINF = -1;
    public static int CODSISINF
    {
        get
        {
            if (_CODSISINF == -1)
                _CODSISINF = Convert.ToInt32(ConfigurationManager.AppSettings["CODSISINF"]);
            return _CODSISINF;
        }
    }

    private static string _CAMINHO_LDAP = null;
    public static string CAMINHO_LDAP
    {
        get
        {
            if (string.IsNullOrEmpty(_CAMINHO_LDAP))
                _CAMINHO_LDAP = ConfigurationManager.AppSettings["CAMINHO_LDAP"].ToString();
            return _CAMINHO_LDAP;
        }
    }

    private static int _PORTA_LDAP = -1;
    public static int PORTA_LDAP
    {
        get
        {
            if (_PORTA_LDAP == -1)
                _PORTA_LDAP = Convert.ToInt32(ConfigurationManager.AppSettings["PORTA_LDAP"].ToString());
            return _PORTA_LDAP;
        }
    }

    private static string _DISTINGUISHEDNAME_LDAP = null;
    public static string DISTINGUISHEDNAME_LDAP
    {
        get
        {
            if (string.IsNullOrEmpty(_DISTINGUISHEDNAME_LDAP))
                _DISTINGUISHEDNAME_LDAP = ConfigurationManager.AppSettings["DISTINGUISHEDNAME_LDAP"].ToString();
            return _DISTINGUISHEDNAME_LDAP;
        }
    }

    private static int _MODODESENVOLVIMENTO = -1;
    public static int MODODESENVOLVIMENTO
    {
        get
        {
            if (_MODODESENVOLVIMENTO == -1)
                _MODODESENVOLVIMENTO = Convert.ToInt32(ConfigurationManager.AppSettings["MODODESENVOLVIMENTO"]);
            return _MODODESENVOLVIMENTO;
        }
    }

    private static string _URLSISTEMAMODODESENVOLVIMENTO = string.Empty;
    public static string URLSISTEMAMODODESENVOLVIMENTO
    {
        get
        {
            if (string.IsNullOrEmpty(_URLSISTEMAMODODESENVOLVIMENTO))
                _URLSISTEMAMODODESENVOLVIMENTO = ConfigurationManager.AppSettings["URLSISTEMAMODODESENVOLVIMENTO"].ToString();
            return _URLSISTEMAMODODESENVOLVIMENTO;
        }
    }

    public static ConjuntoDePermissoesUsuarioSistemas ObterConjuntoDePermissoesUsuario(HttpSessionState Session, string nomUsrRde = "", List<string> grupos = null)
    {
        ConjuntoDePermissoesUsuarioSistemas retorno = new ConjuntoDePermissoesUsuarioSistemas();
        // 
        if ((!nomUsrRde.Equals(string.Empty)))
        {
            var bll = new ControleDeAcessoBLL();
            // 
            retorno.InformacoesUsuario = bll.ObterInformacoesUsuario(0, nomUsrRde).FirstOrDefault();
            if (retorno.InformacoesUsuario != null)
            {
                retorno.SistemasPermitidos = bll.ObterSistemasPermitidos(CODSISINF, retorno.InformacoesUsuario.CODFNC);
                if (grupos != null)
                    retorno.SistemasPermitidos = retorno.SistemasPermitidos.Where(x => grupos.Contains(x.DESGRPRDESISSMA)).ToList();
            }
        }
        else if (Session["ConjuntoDePermissoesUsuario"] != null)
            retorno = (ConjuntoDePermissoesUsuarioSistemas)Session["ConjuntoDePermissoesUsuario"];
        // 
        if (retorno != null && retorno.InformacoesUsuario != null && retorno.SistemasPermitidos.Count > 0)
            Session["ConjuntoDePermissoesUsuario"] = retorno;
        else
        {
            Session.Clear();
            Session.Abandon();
        }
        // 
        return retorno;
    }

    public static ConjuntoDePermissoesUsuarioModulos ObterConjuntoDeModulosUsuario(int CODFNC, int CODSISINF)
    {
        ConjuntoDePermissoesUsuarioModulos retorno = new ConjuntoDePermissoesUsuarioModulos();
        // 
        var bll = new ControleDeAcessoBLL();
        retorno.InformacoesUsuario = bll.ObterInformacoesUsuario(CODFNC, string.Empty).FirstOrDefault();
        retorno.ModulosPermitidos = bll.ObterModulosPermitidos(CODSISINF, CODFNC);
        foreach (var Item in retorno.ModulosPermitidos)
            Item.CONTROLES.AddRange(bll.ObterControlesPermitidos(Item.CODMDUSIS, CODSISINF, CODFNC));
        // 
        return retorno;
    }

    public static List<string> AutenticaUsuario(string usuario, string senha)
    {
        List<string> retorno = new List<string>();
        WebReturn<bool> retr = new WebReturn<bool>();
        try
        {
            if (MODODESENVOLVIMENTO != 1)
            {
                LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(CAMINHO_LDAP, PORTA_LDAP));
                con.SessionOptions.ProtocolVersion = 3;
                // 
                con.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback((LdapConnection l, X509Certificate x) => { return true; });
                // 
                con.Credential = new NetworkCredential(usuario, senha);
                con.AuthType = AuthType.Negotiate;
                // 
                var ldapFilter = string.Format("(&(objectCategory=person)(objectClass=user)(&(sAMAccountName={0})))", usuario);
                // 
                var getUserRequest = new SearchRequest(DISTINGUISHEDNAME_LDAP, ldapFilter, SearchScope.Subtree, new[] { "MemberOf" })
                {
                    SizeLimit = 1
                };
                // 
                var SearchControl = new SearchOptionsControl(SearchOption.DomainScope);
                getUserRequest.Controls.Add(SearchControl);
                // 
                var userResponse = (SearchResponse)con.SendRequest(getUserRequest);
                retorno.AddRange(ObterGruposRedeUsuario(userResponse.Entries[0]));
            }
            else
            {
                //retorno.Add("SMART.GESTAOACESSOSMART");
                //retorno.Add("SMART.POS.GESTAOVISITA");
                //retorno.Add("SMART.FINANCEIRO");
                //retorno.Add("SMART.CENTRALCOMPRAS");
                //retorno.Add("SMART.PORTAL.TECNOLOGIAINFO");

                //return retorno;

                return null;
            }
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
                                  "ERRO AUTENTICAR USUARIO SISTEMA.!!");

            return new List<string>();
        }
        return retorno;
    }

    public static string[] ObterGruposRedeUsuario(SearchResultEntry usuario)
    {
        string grupos = "";
        string separador = "";
        for (int x = 0; x <= usuario.Attributes["MemberOf"].Count - 1; x++)
        {
            grupos += separador + usuario.Attributes["MemberOf"][x].ToString().Split(',')[0].Replace("CN=", "");
            separador = ",";
        }
        return grupos.ToUpper().Split(System.Convert.ToChar(Convert.ToChar(",")));
    }

    public class ConjuntoDePermissoesUsuarioSistemas
    {
        public ControleDeAcessoTO.ObterInformacoesUsuario InformacoesUsuario { get; set; }
        public List<ControleDeAcessoTO.ObterSistemasPermitidos> SistemasPermitidos { get; set; }
    }

    public class ConjuntoDePermissoesUsuarioModulos
    {
        public ControleDeAcessoTO.ObterInformacoesUsuario InformacoesUsuario { get; set; }
        public List<ControleDeAcessoTO.ObterModulosPermitidos> ModulosPermitidos { get; set; }
    }
}
