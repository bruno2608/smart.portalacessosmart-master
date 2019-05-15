
using Microsoft.VisualBasic;
using System.Data.Common;
using System.Data;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;

public static class TrataDbCommand
{

    /// <summary>
    /// Método de extensão para tratar o objeto DbCommand, alinhando os parâmetros já adicionados com os parâmetros da query.
    /// </summary>
    /// <param name="dbCommand">Objeto DbCommand com as informações da query a ser executada</param>
    /// <param name="removeParametrosDaQuery">
    /// -----------------------------------------------------------------------------------------------------------------------------------------------------
    /// Flag para definir se o método irá remover as linhas da query que tenham parâmetros inválidos (nulos, numérico menos um ou vazios)
    /// OBS: Utilizar StringBuilder.AppendLine() ou Environment.NewLine - A linha da query que tem o parâmetro inválido será removida, exemplo:
    /// -----------------------------------------------------------------------------------------------------------------------------------------------------
    /// ----------------------------------------------------------------------------
    /// WHERE 1=1 -- LINHA NÃO REMOVIDA
    ///            AND CODBAI = :CODBAI -- LINHA REMOVIDA SE O PARÂMETRO NUMÉRICO FOR -1
    ///            AND NOMBAI = :NOMBAI -- LINHA REMOVIDA SE O PARÂMETRO STRING É VAZIO
    ///            AND DATDST = :DATDST -- LINHA REMOVIDA SE O PARÂMETRO FOR NULO
    /// ORDER BY CODBAI -- LINHA NÃO REMOVIDA
    /// ----------------------------------------------------------------------------
    /// </param>
    /// <param name="removeParametrosDeValoresNulos">Define se parâmetros com valores nulos do comando serão removidos</param>
    /// <param name="removeParametosDeValoresNumericoMenosUm">Define se parâmetros numérico -1 do comando serão removidos</param>
    /// <param name="removeParametrosDeValoresStringVazios">Define se parâmetros string vazios do comando serão removidos</param>
    /// <remarks>Desenvolvido por Michel Oliveira @ Prime Team Tecnologia 2016</remarks>
    /// 
    public static void TrataDbCommandUniversal(this DbCommand dbCommand, bool removeParametrosDaQuery = false, bool removeParametrosDeValoresNulos = false, bool removeParametosDeValoresNumericoMenosUm = false, bool removeParametrosDeValoresStringVazios = false, bool removeParametosDeValoresNumericoZero = false)
    {
        StringBuilder queryToManage = new StringBuilder();
        List<string> paramLinesToRemove = new List<string>();
        SortedDictionary<int, DbParameter> parsToManage = new SortedDictionary<int, DbParameter>();
        List<DbParameter> parsToAdd = new List<DbParameter>();
        string baseRegExIni = "(?:^|\\W)";
        string baseRegExFim = "(?:$|\\W)";
        long tempConvert;
        foreach (DbParameter par in dbCommand.Parameters)
        {
            if (removeParametrosDeValoresNulos &&
                            (par.Value == null ||
                             par.Value == DBNull.Value))
            {
                paramLinesToRemove.Add(par.ParameterName);
                continue;
            }
            //
            if (removeParametosDeValoresNumericoMenosUm &&
                (par.DbType == DbType.Int16 ||
                 par.DbType == DbType.UInt32 ||
                 par.DbType == DbType.Int32 ||
                 par.DbType == DbType.Int64 ||
                 par.DbType == DbType.Decimal) &&
                 (par.Value != null &&
                 Int64.TryParse(par.Value.ToString(), out tempConvert) &&
                 tempConvert == -1))
            {
                paramLinesToRemove.Add(par.ParameterName);
                continue;
            }
            //
            if (removeParametosDeValoresNumericoZero &&
                (par.DbType == DbType.Int16 ||
                 par.DbType == DbType.UInt32 ||
                 par.DbType == DbType.Int32 ||
                 par.DbType == DbType.Int64 ||
                 par.DbType == DbType.Decimal) &&
                 (par.Value != null &&
                 Int64.TryParse(par.Value.ToString(), out tempConvert) &&
                 tempConvert == 0))
            {
                paramLinesToRemove.Add(par.ParameterName);
                continue;
            }
            //
            if (removeParametrosDeValoresStringVazios && (par.DbType == DbType.String) &&
                (par.Value != null && par.Value != DBNull.Value &&
                (par.Value.ToString().Trim().Equals(String.Empty) ||
                par.Value.ToString().Trim().Equals("%") ||
                par.Value.ToString().Trim().Equals("%%"))))
            {
                paramLinesToRemove.Add(par.ParameterName);
                continue;
            }
            //
            var rEx = Regex.Match(dbCommand.CommandText, string.Concat(baseRegExIni, par.ParameterName.StartsWith(":") ? "" : ":", par.ParameterName, baseRegExFim));
            if (rEx.Success)
            {
                parsToManage.Add(rEx.Index, par);
                continue;
            }
        }
        //
        if (removeParametrosDaQuery)
        {
            if (paramLinesToRemove.Count > 0)
            {
                using (var sReader = new StringReader(dbCommand.CommandText))
                {
                    string line = string.Empty;
                    do
                    {
                        line = sReader.ReadLine();
                        if (line != null)
                        {
                            bool canAdd = true;
                            //
                            foreach (var parName in paramLinesToRemove)
                            {
                                if (Regex.IsMatch(line, string.Concat(baseRegExIni, parName.StartsWith(":") ? "" : ":", parName, baseRegExFim), RegexOptions.IgnoreCase))
                                {
                                    canAdd = false;
                                    continue;
                                }
                            }
                            //
                            if(canAdd)
                                queryToManage.AppendLine(line);
                        }
                    }
                    while (line != null);
                }
                //
                dbCommand.CommandText = queryToManage.ToString();
            }
        }
        //
        dbCommand.Parameters.Clear();
        foreach (DbParameter par in parsToManage.OrderBy(p => p.Key).Select(p => p.Value))
        {
            var parCount = Regex.Matches(dbCommand.CommandText, string.Concat(baseRegExIni, par.ParameterName.StartsWith(":") ? "" : ":", par.ParameterName, baseRegExFim), RegexOptions.IgnoreCase).Count;
            for (int i = 0; i <= parCount - 1; i += 1)
            {
                parsToAdd.Add(par);
            }
        }
        //
        parsToManage.Clear();
        foreach (var groupedPar in parsToAdd.GroupBy(p => p.ParameterName).Select(p => p.FirstOrDefault()))
        {
            foreach (Match rEx in Regex.Matches(dbCommand.CommandText, string.Concat(baseRegExIni, groupedPar.ParameterName.StartsWith(":") ? "" : ":", groupedPar.ParameterName, baseRegExFim), RegexOptions.IgnoreCase))
            {
                parsToManage.Add(rEx.Index, groupedPar);
            }
        }
        //
        parsToAdd.Clear();
        dbCommand.Parameters.AddRange(parsToManage.OrderBy(p => p.Key).Select(p => p.Value).ToArray());
    }
}