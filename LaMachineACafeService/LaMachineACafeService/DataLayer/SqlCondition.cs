using System;
using System.Collections.Generic;

namespace LaMachineACafe.Common
{
    public abstract class SqlCondition
    {
        public string TableName { get; protected set; }
        public SqlCondition(string tableName)
        {
            this.Or = null;
            this.And = null;
            this.TableName = tableName;
        }

        public SqlCondition And { get; private set; }
        public SqlCondition Or { get; private set; }

        public SqlCondition OrWhere(string fieldName, object value, SqlComparisonEnum comparison = SqlComparisonEnum.Equals, string tableName = null)
        {
            return OrWhere(new WhereCondition(tableName ?? TableName, fieldName, value, comparison));
        }

        public SqlCondition AndWhere(string fieldName, object value, SqlComparisonEnum comparison = SqlComparisonEnum.Equals, string tableName = null, bool isLiteral = false)
        {
            return AndWhere(new WhereCondition(fieldName, tableName ?? TableName, value, comparison));
        }

        public virtual SqlCondition AndWhere(SqlCondition condition)
        {
            if (this.And == null)
                this.And = condition;
            else
                this.And.AndWhere(condition);

            return this;
        }

        public virtual SqlCondition OrWhere(SqlCondition condition)
        {
            if (this.Or == null)
                this.Or = condition;
            else
                this.Or.OrWhere(condition);

            return this;
        }
    }
}
