using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace LaMachineACafe.Common
{
    public class SqlDataLayer
    {
        private readonly string connectionString;
        public SqlDataLayer()
        {
            connectionString = ConfigurationManager.ConnectionStrings["sqlServerConnectionString"].ConnectionString;
        }

        public List<T> ExecuteQuery<T>(SqlQueryBuilder query)
            where T : class, new()
        {
            var properties = SqlHelpers.GetTypeProperties<T>();

            return QueryToDicList(query.ToString())
                .Select(fieldDic => ExtractObjectFromFieldDic<T>(properties, fieldDic))
                .ToList();
        }

        public void Insert<T>(T objectToInsert)
             where T : class, new()
        {
            Insert(SqlHelpers.TypeToTableName[objectToInsert.GetType()],
                ObjectToFieldDic(objectToInsert));
        }

        public void Update<T>(T objetToUpdate)
             where T : class, new()
        {
            Update(
                tableName: SqlHelpers.TypeToTableName[objetToUpdate.GetType()],
                fields: ObjectToFieldDic<T>(objetToUpdate),
                identityFieldName: SqlHelpers.TypeToIdentityFieldName[objetToUpdate.GetType()]);
        }

        private T ExtractObjectFromFieldDic<T>(
            Dictionary<string, PropertyInfo> properties,
            Dictionary<string, object> fieldDic)
            where T : new()
        {
            T newObj = new T();

            foreach (var field in fieldDic)
            {
                PropertyInfo property;
                if (!properties.TryGetValue(field.Key, out property))
                {
                    continue;
                }

                var value = field.Value;

                if (property.PropertyType == typeof(decimal))
                    property.SetValue(newObj, Convert.ToDecimal(value, CultureInfo.InvariantCulture), index: null);
                else if (property.PropertyType == typeof(decimal?) && value != null)
                    property.SetValue(newObj, Convert.ToDecimal(value, CultureInfo.InvariantCulture), index: null);
                else if (property.PropertyType == typeof(int?) && value != null)
                    property.SetValue(newObj, Convert.ToInt32(value, CultureInfo.InvariantCulture), index: null);

                else
                    property.SetValue(newObj, value, index: null);
            }

            return newObj;
        }

        internal List<Dictionary<string, object>> QueryToDicList(string queryString)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = queryString;
                    List<Dictionary<string, object>> listToFill = new List<Dictionary<string, object>>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var fieldDic = ReaderToFieldDic(reader);
                            listToFill.Add(fieldDic);
                        }
                    }

                    return listToFill;
                }
            }
        }

        private void Insert(string tableName, Dictionary<string, object> fields)
        {
            ExecuteNonQuery(string.Format(
                "INSERT INTO {0} ({1}) VALUES ({2})",
                tableName,
                string.Join(",", fields.Keys.ToArray()),
                string.Join(",", fields.Select(p => p.Value).ToArray())
                ));
        }

        private void Update(string tableName, Dictionary<string, object> fields, string identityFieldName)
        {
            List<string> updateKeyValuePairs = GetUpdateKeyValuePairs(tableName, fields);

            ExecuteNonQuery(string.Format(
                "UPDATE {0} SET {1} WHERE {2} = '{3}'",
                tableName,
                identityFieldName,
                string.Join(",", updateKeyValuePairs.ToArray()),
                fields[identityFieldName].AsSqlString()
                ));
        }

        private static List<string> GetUpdateKeyValuePairs(string tableName, Dictionary<string, object> fields)
        {
            List<string> updateKeyValuePairs = new List<string>();

            foreach (var field in fields)
            {
                string fieldName = field.Key;
                updateKeyValuePairs.Add(fieldName + " = " + SqlHelpers.DecorateSqlValue(field.Value.AsSqlString()));
            }

            return updateKeyValuePairs;
        }

        private void ExecuteNonQuery(string queryString)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = queryString;

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        private Dictionary<string, object> ReaderToFieldDic(SqlDataReader reader)
        {
            var fieldDic = new Dictionary<string, object>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var value = reader.GetValue(i);

                if (value is DBNull)
                    value = null;

                fieldDic[reader.GetName(i)] = value;
            }
            return fieldDic;
        }

        private Dictionary<string, object> ObjectToFieldDic<T>(T objectToInsert)
            where T : class
        {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            IEnumerable<PropertyInfo> properties = objectToInsert.GetType().GetRuntimeProperties();

            foreach (var prop in properties)
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    var fieldName = prop.Name;
                    fields[fieldName] = prop.GetValue(objectToInsert, index: null); ;
                }
            }

            return fields;
        }
    }
}