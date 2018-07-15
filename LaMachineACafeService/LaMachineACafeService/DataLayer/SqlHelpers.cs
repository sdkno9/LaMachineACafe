using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace LaMachineACafe.Common
{
    public static class SqlHelpers
    {
        public static readonly string BadgeReferenceFieldName = "BadgeReference";

        public static Dictionary<Type, string> TypeToTableName = new Dictionary<Type, string>()
        {
            { typeof(Drink), "DrinksTable" },
            { typeof(DrinkSelection), "DrinkPreferencesTable" }
        };

        public static Dictionary<Type, string> TypeToIdentityFieldName = new Dictionary<Type, string>()
        {
            { typeof(Drink), "Id" },
            { typeof(DrinkSelection), "BadgeReference" }
        };

        public static readonly Dictionary<SqlComparisonEnum, string> SqlComparisonEnumToString = new Dictionary<SqlComparisonEnum, string>()
        {
            { SqlComparisonEnum.Equals, "=" },
            { SqlComparisonEnum.Different, "!=" }
        };

        public static string AsSqlString(this object value)
        {
            if (value == null)
                return null;

            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        public static string DecorateSqlValue(string sqlValue)
        {
            if (sqlValue == null)
                return "NULL";
            else
                return "'" + sqlValue + "'";
        }

        public static Dictionary<string, PropertyInfo> GetTypeProperties<T>()
        {
            var properties = typeof(T)
                .GetRuntimeProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .ToDictionary(p => p.Name, p => p);
            return properties;
        }
    }
}