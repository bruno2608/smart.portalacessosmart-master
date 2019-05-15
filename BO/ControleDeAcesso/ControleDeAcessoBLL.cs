using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ControleDeAcessoBLL
{
    public List<ControleDeAcessoTO.ObterSistemasPermitidos> ObterSistemasPermitidos(int CODSISINFPORTAL, int CODFNC)
    {
        var DAL = new ControleDeAcessoDAL();
        return DAL.ObterSistemasPermitidos(CODSISINFPORTAL, CODFNC);
    }

    public List<ControleDeAcessoTO.ObterModulosPermitidos> ObterModulosPermitidos(int CODSISINF, int CODFNC)
    {
        var DAL = new ControleDeAcessoDAL();
        return DAL.ObterModulosPermitidos(CODSISINF, CODFNC);
    }

    /// <summary>
    ///     ''' Obtem os controles permitidos de cada tela.
    ///     ''' </summary>
    ///     ''' <returns></returns>
    public List<ControleDeAcessoTO.ObterControles> ObterControlesPermitidos(decimal CODMDUSIS, int CODSISINF, int CODFNC)
    {
        var DAL = new ControleDeAcessoDAL();
        return DAL.ObterControlesPermitidos(CODMDUSIS, CODSISINF, CODFNC);
    }

    public List<ControleDeAcessoTO.ObterInformacoesUsuario> ObterInformacoesUsuario(int CODFNC, string NOMUSRRCF)
    {
        var DAL = new ControleDeAcessoDAL();
        return DAL.ObterInformacoesUsuario(CODFNC, NOMUSRRCF);
    }

    /// <summary>
    ///     ''' Valida e Atualiza os Grupos do Usuário.
    ///     ''' </summary>
    ///     ''' <param name="CODFNC"></param>
    ///     ''' <param name="SISTEMAS"></param>
    ///     ''' <returns></returns>
    ///     ''' <remarks>Leon Denis Paiva e Silva [PrimeTeam]</remarks>
    public bool ValidarGrupoSistemas(int CODFNC, List<string> SISTEMAS)
    {
        var DAL = new ControleDeAcessoDAL();

        var GruposDeSistemas = DAL.ObterSistemasUsuario(CODFNC, SISTEMAS);

        bool Atualizacao = false;

        if (GruposDeSistemas != null && GruposDeSistemas.Count > 0)
            DAL.AtualizarRelacaoSistemasUsuario(CODFNC, GruposDeSistemas);

        foreach (var Item in GruposDeSistemas)
        {
            if (Item.ACAO.Equals("INSERIR"))
                Atualizacao = DAL.InserirRelacaoSistemaUsuario(CODFNC, Item.CODGRPRDESISSMA);
        }

        return Atualizacao;
    }

    /// <summary>
    ///     ''' Obtem os Sistemas do Usuário.
    ///     ''' </summary>
    ///     ''' <param name="CODFNC"></param>
    ///     ''' <param name="SISTEMAS"></param>
    ///     ''' <returns></returns>
    ///     ''' <remarks>Leon Denis Paiva e Silva [PrimeTeam]</remarks>
    public List<ControleDeAcessoTO.ObterSistemasUsuario> ObterSistemasUsuario(int CODFNC, List<string> SISTEMAS)
    {
        return (new ControleDeAcessoDAL()).ObterSistemasUsuario(CODFNC, SISTEMAS);
    }

    /// <summary>
    ///     ''' Atualiza os Sistemas relacionados ao Usuário.
    ///     ''' </summary>
    ///     ''' <param name="CODFNC"></param>
    ///     ''' <param name="GruposSistema"></param>
    ///     ''' <returns></returns>
    ///     ''' <remarks>Leon Denis Paiva e Silva [PrimeTeam]</remarks>
    public bool AtualizarRelacaoSistemasUsuario(int CODFNC, List<ControleDeAcessoTO.ObterSistemasUsuario> GruposSistema)
    {
        return (new ControleDeAcessoDAL()).AtualizarRelacaoSistemasUsuario(CODFNC, GruposSistema);
    }

    /// <summary>
    ///     ''' Insere uma nova Relação de Sistemas do Usuário.
    ///     ''' </summary>
    ///     ''' <param name="CODFNC"></param>
    ///     ''' <param name="CODGRPRDESISSMA"></param>
    ///     ''' <returns></returns>
    ///     ''' <remarks>Leon Denis Paiva e Silva [PrimeTeam]</remarks>
    public bool InserirRelacaoSistemaUsuario(int CODFNC, long CODGRPRDESISSMA)
    {
        return (new ControleDeAcessoDAL()).InserirRelacaoSistemaUsuario(CODFNC, CODGRPRDESISSMA);
    }
}