using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

public class Constantes
{
    #region Constantes
    public static string PATH_IMAGE_ORIGINAL
    {
        get { return ConfigurationManager.AppSettings["PathImagesOriginal"]; }
    }
    public static string PATH_IMAGE_SITE
    {
        get { return ConfigurationManager.AppSettings["PathImagesSite"]; }
    }

    public static string PATH_IMAGE_BACKUP
    {
        get { return ConfigurationManager.AppSettings["PathImagesBackup"]; }
    }

    public static string PATH_TEMP
    {
        get { return ConfigurationManager.AppSettings["PathTemp"]; }
    }

    public static string HOSTNAME_IMAGES
    {
        get { return ConfigurationManager.AppSettings["HostnameImages"]; }
    }

    public static string USERNAME_IMAGES
    {
        get { return ConfigurationManager.AppSettings["UsernameImages"]; }
    }

    public static string PASSWORD_IMAGES
    {
        get { return ConfigurationManager.AppSettings["PasswordImages"]; }
    }

    public static bool FTP_PASSIVE
    {
        get { return Convert.ToBoolean(ConfigurationManager.AppSettings["FtpPassive"]); }
    }

    public static string MAX_WIDTH_IMAGE_SITE = "maxWidthImagensSite";
    public static string MAX_HEIGHT_IMAGE_SITE = "maxHeightImagensSite";

    public static string DS_STATUS_INATIVO = "INATIVO";
    public static string DS_STATUS_ATIVO = "ATIVO";

    public static string DS_ABRANGENCIA_NACIONAL = "N";
    public static string DS_ABRANGENCIA_REGIONAL = "R";
    public static string DS_ABRANGENCIA_LOCAL = "L";
    #endregion

    #region App_Settings
    public static string ModoDesenvolvimento
    {
        get { return ConfigurationManager.AppSettings["MODODESENVOLVIMENTO"]; }
    }
    public static string PortaServidorEmail
    {
        get { return ConfigurationManager.AppSettings["PortaServidorEmail"]; }
    }

    public static string ServidorEmail
    {
        get { return ConfigurationManager.AppSettings["ServidorEmail"]; }
    }

    public static string SenhaEmailAplicacao
    {
        get { return ConfigurationManager.AppSettings["SenhaEmailAplicacao"]; }
    }

    public static string EmailAplicacao
    {
        get { return ConfigurationManager.AppSettings["EmailAplicacao"]; }
    }

    public static string NomeEmailAplicacao
    {
        get { return ConfigurationManager.AppSettings["NomeEmailAplicacao"]; }
    }

    public static string EmailsDesenvolvimentoEnviar
    {
        get { return ConfigurationManager.AppSettings["EmailsDesenvolvimentoEnviar"]; }
    }
    #endregion

    #region WebApi Log
    public static string WebApiLog
    {
        get { return ConfigurationManager.AppSettings["WEBAPILOG"]; }
    }

    public static string CriaLogErro
    {
        get { return ConfigurationManager.AppSettings["CRIALOGERRO"]; }
    }

    public static string InserirLog
    {
        get { return ConfigurationManager.AppSettings["INSERIRLOG"]; }
    }
    #endregion

    #region Enumeradores
    public enum TipoAbrangencia
    {
        Nacional = 1,
        Regional = 2,
        Local = 3
    }

    public enum TipoPoloPesquisa
    {
        ApenasUsuario = 1,
        DoPolo = 2,
        Todos = 3
    }

    public enum TipoAcao
    {
        Visualizar = 1,
        Editar = 2,
        Ativar = 3,
        AtivarOutroPolo = 4
    }
    #endregion
}