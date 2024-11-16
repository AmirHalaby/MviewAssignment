using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Dapper
{
    public static class Utils
    {
        public static DataTable ToDataTable<T>(this List<T> iList, string? tableTypeName = null)
        {
            var dataTable = tableTypeName != null ? new DataTable(tableTypeName) : new DataTable();

            var propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection)
            {
                var type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);

                if (type != null)
                {
                    dataTable.Columns.Add(propertyDescriptor.Name, type);
                }
            }

            var values = new object[propertyDescriptorCollection.Count];
            foreach (var iListItem in iList)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem)!;
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
