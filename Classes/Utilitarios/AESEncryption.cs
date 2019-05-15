using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

public class Encryption
{
    private static int _AESKEYSIZE = -1;
    public static int AESKEYSIZE
    {
        get
        {
            if (_AESKEYSIZE == -1)
                _AESKEYSIZE = Convert.ToInt32(ConfigurationManager.AppSettings["AESKEYSIZE"]);
            return _AESKEYSIZE;
        }
    }

    private static int _AESBLOCKSIZE = -1;
    public static int AESBLOCKSIZE
    {
        get
        {
            if (_AESBLOCKSIZE == -1)
                _AESBLOCKSIZE = Convert.ToInt32(ConfigurationManager.AppSettings["AESBLOCKSIZE"]);
            return _AESBLOCKSIZE;
        }
    }
    private static string _CAMINHOAPISISTEMAS = "";
    public static string CAMINHOAPISISTEMAS
    {
        get
        {
            if (_CAMINHOAPISISTEMAS == "")
                _CAMINHOAPISISTEMAS = Convert.ToString(ConfigurationManager.AppSettings["CAMINHOAPISISTEMAS"]);
            return _CAMINHOAPISISTEMAS;
        }
    }

    public static string CalcularMD5(string input)
    {
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i <= hash.Length - 1; i++)
            sb.Append(hash[i].ToString("X2"));
        return sb.ToString();
    }

    public static PortalApiModel.RetornoTransicionarSistema TransicionarSistema(PortalApiModel.TransicionarSistema parametros)
    {
        // 
        WebReturn<bool> retr = new WebReturn<bool>();
        try
        {
            PortalApiModel.RetornoTransicionarSistema retorno = new PortalApiModel.RetornoTransicionarSistema();
            // 
            ControleDeAcessoTO.ObterSistemasPermitidos sistemaSelecionado;
            if (ControleDeAcesso.MODODESENVOLVIMENTO == 0)
                sistemaSelecionado = ControleDeAcesso.ObterConjuntoDePermissoesUsuario(HttpContext.Current.Session).SistemasPermitidos.Where(x => x.CODSISINF == parametros.CODSISINF).FirstOrDefault();
            else
                sistemaSelecionado = new ControleDeAcessoTO.ObterSistemasPermitidos() { DESURLLNK = ControleDeAcesso.URLSISTEMAMODODESENVOLVIMENTO };
            if (sistemaSelecionado != null)
            {
                var PRIVATEKEY = GerarChavePrivada();
                var codFnc = ControleDeAcesso.ObterConjuntoDePermissoesUsuario(HttpContext.Current.Session).InformacoesUsuario.CODFNC;
                // 
                retorno.PUBLICTOKEN = Encriptar(PRIVATEKEY, new JavaScriptSerializer().Serialize(new ControleDeAcesso.ParametrosCriptografia() { ParametrosTransicao = parametros, CODFNC = codFnc }));
                // 
                retorno.PUBLICKEY = CalcularMD5(PRIVATEKEY);
                retorno.DESURLLNK = sistemaSelecionado.DESURLLNK + CAMINHOAPISISTEMAS + "?PUBLICKEY=" + HttpUtility.UrlEncode(retorno.PUBLICKEY) + "&PUBLICTOKEN=" + HttpUtility.UrlEncode(retorno.PUBLICTOKEN);
                // 
                lock (ControleDeAcesso.PENDINGTOKENS)
                    ControleDeAcesso.PENDINGTOKENS.Add(new ControleDeAcesso.PENDINGTOKEN
                                                       ()
                    {
                        DTEXP = DateTime.Now.AddSeconds(10),
                        PRIVATEKEY = PRIVATEKEY,
                        PUBLICKEY = retorno.PUBLICKEY
                    });
            }
            return retorno;
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
                                  "ERRO TRANSACIONAR SISTEMA.!!");
            return null/* TODO Change to default(_) if this is not a reference type */;
        }
    }

    public static ControleDeAcesso.ConjuntoDePermissoesUsuarioModulos ObterInformacoesUsuario(PortalApiModel.ObterInformacoesUsuario parametros)
    {
        // 
        WebReturn<bool> retr = new WebReturn<bool>();
        try
        {
            ControleDeAcesso.ConjuntoDePermissoesUsuarioModulos retorno = null/* TODO Change to default(_) if this is not a reference type */;
            // 
            var pendingToken = ControleDeAcesso.PENDINGTOKENS.Where(x => x.PUBLICKEY == parametros.PUBLICKEY).FirstOrDefault();
            if (pendingToken != null)
            {
                var desCriptografia = Decriptar(pendingToken.PRIVATEKEY, parametros.PUBLICTOKEN);
                var parametrosDescriptografados = new JavaScriptSerializer().Deserialize<ControleDeAcesso.ParametrosCriptografia>(desCriptografia);
                if (parametrosDescriptografados.ParametrosTransicao.CODSISINF == parametros.CODSISINF)
                {
                    lock (ControleDeAcesso.PENDINGTOKENS)
                        ControleDeAcesso.PENDINGTOKENS.Remove(pendingToken);
                    retorno = ControleDeAcesso.ObterConjuntoDeModulosUsuario(parametrosDescriptografados.CODFNC, parametrosDescriptografados.ParametrosTransicao.CODSISINF);
                }
            }
            // 
            return retorno;
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
                                  "ERRO AO OBTER INFORMAÇÕES DO USUARIO.!!");
            return null/* TODO Change to default(_) if this is not a reference type */;
        }
    }

    private static string GerarChavePrivada()
    {
        RijndaelManaged aesEncryption = new RijndaelManaged();
        aesEncryption.KeySize = AESKEYSIZE;
        aesEncryption.BlockSize = AESBLOCKSIZE;
        aesEncryption.Mode = CipherMode.CBC;
        aesEncryption.Padding = PaddingMode.PKCS7;
        aesEncryption.GenerateIV();
        string ivStr = Convert.ToBase64String(aesEncryption.IV);
        aesEncryption.GenerateKey();
        string keyStr = Convert.ToBase64String(aesEncryption.Key);
        string completeKey = Convert.ToString(ivStr + Convert.ToString(",")) + keyStr;

        return Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(completeKey));
    }

    private static string Encriptar(string AESKey, string iPlainStr)
    {
        var iCompleteEncodedKey = AESKey;

        RijndaelManaged aesEncryption = new RijndaelManaged();
        aesEncryption.KeySize = AESKEYSIZE;

        aesEncryption.BlockSize = AESBLOCKSIZE;
        aesEncryption.Mode = CipherMode.CBC;
        aesEncryption.Padding = PaddingMode.PKCS7;
        aesEncryption.IV = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(iCompleteEncodedKey)).Split(',')[0]);
        aesEncryption.Key = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(iCompleteEncodedKey)).Split(',')[1]);
        byte[] plainText = ASCIIEncoding.UTF8.GetBytes(iPlainStr);
        ICryptoTransform crypto = aesEncryption.CreateEncryptor();
        byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);
        return Convert.ToBase64String(cipherText);
    }
    private static string Decriptar(string AESKey, string iEncryptedText)
    {
        var iCompleteEncodedKey = AESKey;

        RijndaelManaged aesEncryption = new RijndaelManaged();
        aesEncryption.KeySize = AESKEYSIZE;

        aesEncryption.BlockSize = AESBLOCKSIZE;
        aesEncryption.Mode = CipherMode.CBC;
        aesEncryption.Padding = PaddingMode.PKCS7;
        aesEncryption.IV = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(iCompleteEncodedKey)).Split(',')[0]);
        aesEncryption.Key = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(iCompleteEncodedKey)).Split(',')[1]);
        ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
        byte[] encryptedBytes = Convert.FromBase64CharArray(iEncryptedText.ToCharArray(), 0, iEncryptedText.Length);
        return ASCIIEncoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
    }
}
