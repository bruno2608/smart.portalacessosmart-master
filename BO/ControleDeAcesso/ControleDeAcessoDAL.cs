using System;
using System.Collections.Generic;

public class ControleDeAcessoDAL : DAL
{
    public List<ControleDeAcessoTO.ObterSistemasPermitidos> ObterSistemasPermitidos(int CODSISINFPORTAL, int CODFNC)
    {
        var DALSQL = new ControleDeAcessoDALSQL();
        string cmdSql = DALSQL.ObterSistemasPermitidos();
        // 
        var dbCommand = MRT001.GetSqlStringCommand(cmdSql);
        // 
        dbCommand.AddWithValue("CODSISINFPORTAL", CODSISINFPORTAL);
        dbCommand.AddWithValue("CODFNC", CODFNC);
        // 
        dbCommand.TrataDbCommandUniversal();
        return MRT001.ExecuteReader(dbCommand).DataReaderParaClasse<ControleDeAcessoTO.ObterSistemasPermitidos>();
    }

    public List<ControleDeAcessoTO.ObterModulosPermitidos> ObterModulosPermitidos(int CODSISINF, int CODFNC)
    {
        var DALSQL = new ControleDeAcessoDALSQL();
        string cmdSql = DALSQL.ObterModulosPermitidos();
        // 
        var dbCommand = MRT001.GetSqlStringCommand(cmdSql);
        // 
        dbCommand.AddWithValue("CODSISINF", CODSISINF);
        dbCommand.AddWithValue("CODFNC", CODFNC);
        // 
        dbCommand.TrataDbCommandUniversal();
        return MRT001.ExecuteReader(dbCommand).DataReaderParaClasse<ControleDeAcessoTO.ObterModulosPermitidos>();
    }

    /// <summary>
    ///     ''' Obtem os controles permitidos de cada tela.
    ///     ''' </summary>
    ///     ''' <returns></returns>
    public List<ControleDeAcessoTO.ObterControles> ObterControlesPermitidos(decimal CODMDUSIS, int CODSISINF, int CODFNC)
    {
        var DALSQL = new ControleDeAcessoDALSQL();
        string cmdSql = DALSQL.ObterControlesPermitidos();

        var dbCommand = MRT001.GetSqlStringCommand(cmdSql);

        dbCommand.AddWithValue("CODMDUSIS", CODMDUSIS);
        dbCommand.AddWithValue("CODSISINF", CODSISINF);
        dbCommand.AddWithValue("CODFNC", CODFNC);

        dbCommand.TrataDbCommandUniversal();

        return MRT001.ExecuteReader(dbCommand).DataReaderParaClasse<ControleDeAcessoTO.ObterControles>();
    }

    public List<ControleDeAcessoTO.ObterInformacoesUsuario> ObterInformacoesUsuario(int CODFNC, string NOMUSRRCF)
    {
        var DALSQL = new ControleDeAcessoDALSQL();
        string cmdSql = DALSQL.ObterInformacoesUsuario(CODFNC);
        // 
        var dbCommand = MRT001.GetSqlStringCommand(cmdSql);
        // 
        if (CODFNC > 0)
            dbCommand.AddWithValue("CODFNC", CODFNC);
        else
            dbCommand.AddWithValue("NOMUSRRCF", NOMUSRRCF);
        // 
        dbCommand.TrataDbCommandUniversal();
        return MRT001.ExecuteReader(dbCommand).DataReaderParaClasse<ControleDeAcessoTO.ObterInformacoesUsuario>();
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
        var DALSQL = new ControleDeAcessoDALSQL();
        string ListaGrupos = "";
        var FirstIn = true;

        SISTEMAS.ForEach(Item =>
        {
            if ((FirstIn))
            {
                FirstIn = false;
                ListaGrupos = string.Format("UPPER('{0}')", Item);
            }
            else
                ListaGrupos = string.Format("{0}, UPPER('{1}')", ListaGrupos, Item);
        });

        string cmdSql = string.Format(DALSQL.ObterSistemasUsuario(), ListaGrupos);

        var dbCommand = MRT001.GetSqlStringCommand(cmdSql);

        dbCommand.AddWithValue("CODFNC", CODFNC);

        dbCommand.TrataDbCommandUniversal();

        return MRT001.ExecuteReader(dbCommand).DataReaderParaClasse<ControleDeAcessoTO.ObterSistemasUsuario>();
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
        var DALSQL = new ControleDeAcessoDALSQL();
        string ListaCODGRPRDESISSMA = "";
        var FirstIn = true;

        GruposSistema.ForEach(Item =>
        {
            if (Item.ACAO.Equals("DESATIVAR"))
            {
                if ((FirstIn))
                {
                    FirstIn = false;
                    ListaCODGRPRDESISSMA = Item.CODGRPRDESISSMA.ToString();
                }
                else
                    ListaCODGRPRDESISSMA = string.Format("{0}, {1}", ListaCODGRPRDESISSMA, Item.CODGRPRDESISSMA);
            }
        });

        if (ListaCODGRPRDESISSMA == "")
        {
            return false;
        }

        string cmdSql = string.Format(DALSQL.AtualizarRelacaoSistemasUsuario(), ListaCODGRPRDESISSMA);

        var dbCommand = MRT001.GetSqlStringCommand(cmdSql);

        dbCommand.AddWithValue("CODFNC", CODFNC);

        dbCommand.TrataDbCommandUniversal();

        return Convert.ToBoolean(MRT001.ExecuteNonQuery(dbCommand));
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
        var DALSQL = new ControleDeAcessoDALSQL();

        string cmdSql = DALSQL.ObterPerfisPorGrupo();

        var dbCommand = MRT001.GetSqlStringCommand(cmdSql);

        bool retorno = false;
        bool inseriu = false;

        dbCommand.AddWithValue("CODGRPRDESISSMA", CODGRPRDESISSMA);

        dbCommand.TrataDbCommandUniversal();

        var Perfis = MRT001.ExecuteReader(dbCommand).DataReaderParaClasse<ControleDeAcessoTO.ObterPerfisPorGrupo>();

        //cmdSql = DALSQL.InserirRelacaoSistemaUsuario();
        cmdSql = DALSQL.AtualizarRelacaoSistemaUsuario();

        foreach (var Item in Perfis)
        {
            dbCommand = MRT001.GetSqlStringCommand(cmdSql);
            dbCommand.AddWithValue("CODGRPRDESISSMA", CODGRPRDESISSMA);
            dbCommand.AddWithValue("CODFNC", CODFNC);
            dbCommand.AddWithValue("CODPFLACS", Item.CODPFLACS);
            dbCommand.AddWithValue("CODGRPRDESISSMA1", CODGRPRDESISSMA);
            inseriu = Convert.ToBoolean(MRT001.ExecuteNonQuery(dbCommand));
            if (inseriu && !retorno)
                retorno = inseriu;
        }

        return retorno;
    }
}
