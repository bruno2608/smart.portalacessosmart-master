using System.Web.Hosting;
using System.IO;
using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using RestSharp;

public class Utilitarios
{
    public static object Information { get; private set; }

    /// <summary>
    ///     ''' Transforma a lista genérica em array de string
    ///     ''' </summary>
    ///     ''' <remarks>Desenvolvido por Michel Oliveira @ Prime Team Tecnologia</remarks>
    public static List<string[]> TransformaListaGenericaEmArrayOfString<T>(List<T> lst)
    {
        var ret = new List<string[]>();
        var tpe = typeof(T);
        var tpeProps = tpe.GetProperties();
        // 
        foreach (var itm in lst)
        {
            List<string> arr = new List<string>();
            foreach (var prop in tpeProps)
            {
                var val = prop.GetValue(itm);
                if (val != null && val != DBNull.Value)
                    arr.Add(val.ToString());
                else
                    arr.Add(string.Empty);
            }
            ret.Add(arr.ToArray());
        }
        // 
        return ret;
    }
   
    /// <summary>
    ///     ''' Cria um redirecionamento permanente na aplicação
    ///     ''' </summary>
    ///     ''' <remarks>Desenvolvido por Michel Oliveira @ Prime Team Tecnologia</remarks>
    public static void CriaRedirecionamentoPermanente(HttpRequest Request, System.Web.HttpResponse Response, string redirecionarDe, string redirecionarPara)
    {
        var fullFixedURI = Request.RawUrl;
        fullFixedURI = fullFixedURI.ToLower();
        if (fullFixedURI.Contains(redirecionarDe.ToLower()))
        {
            // 
            string substrUri = string.Empty;
            string redirectURI;
            var indexOfAssets = fullFixedURI.ToLower().IndexOf(redirecionarDe.ToLower());
            var urlRoot = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath;
            // 
            substrUri = fullFixedURI.ToLower().Substring(indexOfAssets, fullFixedURI.Length - indexOfAssets);
            substrUri = substrUri.Replace(redirecionarDe.ToLower(), "/" + redirecionarPara.ToLower());
            // 
            redirectURI = urlRoot + substrUri;
            // 
            Response.Redirect(redirectURI);
        }
    }

    /// <summary>
    ///     ''' Cria um novo nó filho para a Tree à partir do nó pai
    ///     ''' </summary>
    ///     ''' <param name="id">id do nó</param>
    ///     ''' <param name="valor">valor a ser exibido no nó</param>
    ///     ''' <param name="aberto">Nó aberto ou fechado</param>
    ///     ''' <param name="selecionado">Nó selecionado</param>
    ///     ''' <param name="desabilitado">Nó desabilitado</param>
    ///     ''' <returns>O novo nó criado</returns>
    ///     ''' <remarks>Alex Araújo - Prime</remarks>
    public static Utilitarios.ComponenteTree CriarNoTree(string id, string valor, bool aberto = false, bool selecionado = false, bool desabilitado = false)
    {
        Utilitarios.ComponenteTreeState estado = new Utilitarios.ComponenteTreeState();

        estado.opened = aberto;
        estado.selected = selecionado;
        estado.disabled = desabilitado;

        return new Utilitarios.ComponenteTree() { id = id, text = valor, state = estado };
    }   

    public class ObjetoRetorno
    {
        public int Codigo { get; set; }
        public string Mensagem { get; set; }
    }

    public class ComponenteTree
    {
        public string id { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public ComponenteTreeState state { get; set; }
        public List<ComponenteTree> children { get; set; }
    }

    public class ComponenteTreeState
    {
        public bool? opened { get; set; }
        public bool? disabled { get; set; }
        public bool? selected { get; set; }
    }

    public class ComponenteMultiSelect
    {
        public string value { get; set; }
        public string text { get; set; }
        public int index { get; set; }
        public string nested { get; set; }
        public bool selected { get; set; }
    }

    public static void CriaLogErro(Exception excpt)
    {
        RestClient client = new RestClient(string.Format("{0}/{1}", Constantes.WebApiLog, Constantes.CriaLogErro));
        RestRequest request = new RestRequest();
        request.Method = Method.POST;
        LogEvento param = new LogEvento()
        {
            Sistema = "PORTAL ACESSO SMART",
            Mensagem = excpt.Message,
            Message = (excpt.InnerException != null ? excpt.InnerException.Message : string.Empty),
            FullName = excpt.TargetSite.DeclaringType.FullName,
            Name = excpt.TargetSite.Name,
            Source = excpt.Source,
            StackTrace = excpt.StackTrace
        };
        string body = JsonConvert.SerializeObject(param);
        request.AddParameter("application/json", body, ParameterType.RequestBody);
        IRestResponse response = client.Execute(request);
        WebReturn<bool> api = JsonConvert.DeserializeObject<WebReturn<bool>>(response.Content);
    }

    public static void InserirLog(Exception ex, string metodo, string parametro, string camada, string mensagem)
    {
        RestClient client = new RestClient(string.Format("{0}/{1}", Constantes.WebApiLog, Constantes.InserirLog));
        RestRequest request = new RestRequest();
        request.Method = Method.POST;

        string MessagemL = "Portal Acesso Smart: " + mensagem + ": " + Environment.NewLine + Environment.NewLine +
                            "Verifique a mensagem de erro a seguir para solucionar o problema." + Environment.NewLine +
                            Environment.NewLine + ex.Message + Environment.NewLine + Environment.NewLine + "Metodo: " +
                            ex.TargetSite.DeclaringType.FullName + ex.TargetSite.Name + Environment.NewLine + Environment.NewLine + "Origem: " +
                            ex.Source + Environment.NewLine + Environment.NewLine + "Pilha de execução: " + ex.StackTrace;

        string MessagemReduzida = "";
        string Parametro = "";

        if (ex.Message.Length < 600)
            MessagemReduzida = ex.Message;
        else
            MessagemReduzida = ex.Message.Substring(0, 600);

        if (parametro.Length > 70)
            Parametro = parametro.Substring(0, 70);
        else
            Parametro = parametro;

        int codfnc = ControleDeAcesso.ObterConjuntoDePermissoesUsuario(HttpContext.Current.Session).InformacoesUsuario != null ? ControleDeAcesso.ObterConjuntoDePermissoesUsuario(HttpContext.Current.Session).InformacoesUsuario.CODFNC : 0;
        ControleLog param = new ControleLog()
        {
            CODSISINF = 0,
            CODFNC = codfnc,
            DESMDO = metodo,
            DESPMTSIS = Parametro,
            DESCAMERR = camada,
            DESERRRDC = MessagemReduzida,
            DESERRDDO = MessagemL
        };

        string body = JsonConvert.SerializeObject(param);
        request.AddParameter("application/json", body, ParameterType.RequestBody);
        IRestResponse response = client.Execute(request);
        WebReturn<bool> api = JsonConvert.DeserializeObject<WebReturn<bool>>(response.Content);
    }
}

public class LogEvento
{
    public string Sistema { get; set; }
    public string Mensagem { get; set; }
    //Variáveis de exceção
    public string Message { get; set; }
    public string FullName { get; set; }
    public string Name { get; set; }
    public string Source { get; set; }
    public string StackTrace { get; set; }
}

public class ControleLog
{
    public int CODSISINF { get; set; }
    public int NUMERR { get; set; }
    public string DATCAD { get; set; }
    public int CODFNC { get; set; }
    public string DESMDO { get; set; }
    public string DESPMTSIS { get; set; }
    public string DESCAMERR { get; set; }
    public string DESERRRDC { get; set; }
    public string DESERRDDO { get; set; }
}
