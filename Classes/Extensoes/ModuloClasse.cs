using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System;

/// <summary>
/// Módulo que padroniza as classes para preenchimento dinâmico com DataReader.
/// </summary>
/// <remarks>Michel Oliveira @ Prime Team Tecnologia 2015</remarks>
public static class ModuloClasse
{

    /// <summary>
    /// Atributo utilizado para nomear os parâmetros para preenchimento dinâmico.
    /// </summary>
    /// <remarks>Michel Oliveira @ Prime Team Tecnologia 2015</remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class AtributoPropriedade : Attribute
    {

        private string _NomeDoCampoNoDataReader;
        public string NomeDoCampoNoDataReader
        {
            get
            {
                return _NomeDoCampoNoDataReader;
            }
            set
            {
                _NomeDoCampoNoDataReader = value;
            }
        }

        private bool _NaoPopular;
        public bool NaoPopular
        {
            get
            {
                return _NaoPopular;
            }
            set
            {
                _NaoPopular = value;
            }
        }

    }

    /// <summary>
    /// Retorna a lista da classe parametrizada corretamente populada.
    /// </summary>
    /// <typeparam name="tPe">Classe que será retornada como lista.</typeparam>
    /// <param name="dtr">DataReader para obter os dados e preencher a lista com as classes.</param>
    /// <returns>Lista da classe tipada enviada.</returns>
    /// <remarks>Michel Oliveira @ Prime Team Tecnologia 2015</remarks>
    public static List<tPe> DataReaderParaClasse<tPe>(this IDataReader dtr, Dictionary<string, List<object>> dictApenasColunaValores = null)
    {
        var retornoFill = Activator.CreateInstance<List<tPe>>();
        var atributoPropriedades = ObterAtributoPropriedades<tPe>();
        //
        while (dtr.Read())
        {
            var retornoItem = Activator.CreateInstance<tPe>();
            var continueWhile = false;
            //
            foreach (var propAttr in atributoPropriedades)
            {

                if (!propAttr.Key.CanWrite)
                {
                    continue;
                }
                //
                var nomeColunaBuscar = propAttr.Key.Name;
                bool naoPopularAtributo = false;
                //
                if (propAttr.Value != null)
                {
                    nomeColunaBuscar = propAttr.Value.NomeDoCampoNoDataReader;
                    naoPopularAtributo = System.Convert.ToBoolean(propAttr.Value.NaoPopular);
                }
                //
                if (!naoPopularAtributo)
                {
                    var propVal = dtr[nomeColunaBuscar];
                    //
                    if (propVal != DBNull.Value)
                    {
                        try
                        {
                            var propValConvertida = Convert.ChangeType(propVal, propAttr.Key.PropertyType);
                            //
                            if (dictApenasColunaValores != null)
                            {
                                if (dictApenasColunaValores.ContainsKey(nomeColunaBuscar))
                                {
                                    var dict = dictApenasColunaValores[nomeColunaBuscar];
                                    if (!dict.Contains(propValConvertida))
                                    {
                                        continueWhile = true;
                                        break;
                                    }
                                }
                            }
                            //
                            propAttr.Key.SetValue(retornoItem, propValConvertida, null);
                            //
                        }
                        catch (Exception ex)
                        {
                            throw (new Exception(string.Format("Coluna Busca: {0}, Atributo: {1}, Exceção: {2}",
                                nomeColunaBuscar,
                                propAttr.Key.Name,
                                ex.ToString())));
                        }
                    }
                }
                //
            }
            //
            if (!continueWhile)
                retornoFill.Add(retornoItem);
        }
        dtr.Close();
        //
        return retornoFill;
    }

    /// <summary>
    /// Retorna um dicionário com as propriedades e os atributos customizados.
    /// </summary>
    /// <typeparam name="tPe">Classe que será utilizada para obter as propriedades e atributos.</typeparam>
    /// <returns>Lista com as propriedades e os atributos customizados.</returns>
    /// <remarks>Michel Oliveira @ Prime Team Tecnologia 2015</remarks>
    public static Dictionary<PropertyInfo, AtributoPropriedade> ObterAtributoPropriedades<tPe>()
    {
        Dictionary<PropertyInfo, AtributoPropriedade> propriedadeAtributoClasse = new Dictionary<PropertyInfo, AtributoPropriedade>();
        //
        foreach (var prop in typeof(tPe).GetProperties())
        {
            AtributoPropriedade atrProp = null;
            //
            foreach (var attr in prop.GetCustomAttributes(false))
            {
                atrProp = attr as AtributoPropriedade;
                //
                if (atrProp != null)
                {
                    break;
                }
            }
            //
            propriedadeAtributoClasse.Add(prop, atrProp);
        }
        //
        return propriedadeAtributoClasse;
    }

}