using System;
using System.Globalization;
using System.Text;

namespace LaMachineACafe.Common
{
    public class SqlQueryBuilder
    {
        public string TableName { get; private set; }
        public SqlCondition WhereCondition { get; private set; }

        public SqlQueryBuilder(Type tableType)
        {
            TableName = SqlHelpers.TypeToTableName[tableType];
        }

        public SqlQueryBuilder Where(string fieldName, object value, SqlComparisonEnum comparison = SqlComparisonEnum.Equals)
        {
            return Where(new WhereCondition(TableName, fieldName, value, comparison));
        }

        public SqlQueryBuilder Where(SqlCondition condition)
        {
            if (this.WhereCondition == null)
                this.WhereCondition = condition;
            else
                this.WhereCondition.AndWhere(condition);

            return this;
        }

        public override string ToString()
        {
            StringBuilder query = new StringBuilder("SELECT *");
            query.AppendLine(string.Format(CultureInfo.InvariantCulture, "FROM {0}", TableName));
            if (WhereCondition != null)
            {
                query.AppendLine(string.Format(CultureInfo.InvariantCulture, "WHERE {0}", WhereCondition.ToString()));
            }
            return query.ToString();
        }
    }
}