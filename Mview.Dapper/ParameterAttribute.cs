using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Dapper
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParameterAttribute : Attribute
    {
        public string Name { get; }

        public ParameterDirection Direction { get; }

        public string Table { get; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="parameterDirection">Direction of the Parameter. Default is ParameterDirection.Input</param>
        public ParameterAttribute(string name, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            Name = name;
            Direction = parameterDirection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="targetTable"></param>
        public ParameterAttribute(string name, string targetTable)
        {
            Name = name;
            Table = targetTable;
            Direction = ParameterDirection.Input;
        }
    }
}
