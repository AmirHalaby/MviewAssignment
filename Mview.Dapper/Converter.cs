using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mview.Dapper;

namespace Mview.Dapper
{
    public static class Converter
    {
        public static DynamicParameters ToParameters<T>(this T data)
        {
            var parameters = new DynamicParameters();

            if (data != null)
            {
                var type = data.GetType();

                foreach (var property in type.GetProperties())
                {
                    var parameter = property.GetCustomAttribute<ParameterAttribute>();
                    var value = property.GetValue(data, null);
                    if (parameter is null || value is null && Nullable.GetUnderlyingType(property.PropertyType) is null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(parameter.Table))
                    {
                        var table = (value as DataTable)
                            .AsTableValuedParameter(parameter.Table);
                        parameters.Add(parameter.Name, table, direction: parameter.Direction);
                    }
                    else
                    {
                        parameters.Add(parameter.Name, value, direction: parameter.Direction);
                    }
                }
            }

            return parameters;
        }
        public static void MapOutputParameters<T>(this DynamicParameters parameters, T target)
        {
            if (target != null)
            {
                var type = target.GetType();

                var outputProperties = type.GetProperties()
                .Where(property =>
                        property.GetCustomAttribute<ParameterAttribute>()?.Direction == ParameterDirection.Output);

                foreach (var property in outputProperties)
                {
                    var options = property.GetCustomAttribute<ParameterAttribute>();
                    if (options?.Direction != ParameterDirection.Output)
                    {
                        continue;
                    }

                    var propertyType = property.PropertyType;

                    var method = typeof(DynamicParameters)
                        .GetMethod(nameof(DynamicParameters.Get))?
                        .MakeGenericMethod(propertyType);


                    var outputValue = method?.Invoke(parameters, new object[] { options.Name });
                    if (outputValue != null)
                    {
                        property.SetValue(target, outputValue, null);
                    }
                }
            }
        }

    }
}
