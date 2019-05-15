using Microsoft.VisualBasic;
using System;
using System.Data.Common;
using System.Runtime.CompilerServices;


static class MetodosDeExtensoes
{
    /// <summary>
    /// Adiciona um parâmetro com o valor na DAL, não sendo necessário passar o tipo
    /// </summary>
    /// <remarks>Desenvolvido por Michel Oliveira @ Prime Team Tecnologia</remarks>

    public static void AddWithValue(this DbCommand dbCommand,
    string parameterName,
    object parameterValue)
    {
        var par = dbCommand.CreateParameter();
        par.ParameterName = parameterName;
        //
        if (ReferenceEquals(parameterValue, null))
        {
            par.Value = DBNull.Value;
        }
        else
        {
            par.Value = parameterValue;
        }
        //
        dbCommand.Parameters.Add(par);
    }

    /// <summary>
    /// Adiciona um modelo inteiro (classe) como parâmetro na DAL, não sendo necessário declarar
    /// </summary>
    /// <remarks>Desenvolvido por Michel Oliveira @ Prime Team Tecnologia</remarks>
    public static void AddModelWithValue(this DbCommand dbCommand, object model)
    {
        Type type = model.GetType();
        foreach (var prop in type.GetProperties())
        {
            if (ReferenceEquals(prop.PropertyType, typeof(string)))
            {
                AddWithValue(dbCommand, prop.Name, "%" + prop.GetValue(model, null).ToString().ToUpper().Trim() + "%");
            }
            else
            {
                AddWithValue(dbCommand, prop.Name, prop.GetValue(model, null));
            }
        }
    }
}