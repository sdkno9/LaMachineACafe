using System;
using System.Globalization;

namespace LaMachineACafe.Common
{
    public class WhereCondition : SqlCondition
    {
        public string FieldName { get; private set; }
        public object Value { get; private set; }
        public SqlComparisonEnum Comparison { get; private set; }

        public WhereCondition(string tableName, string fieldName, object value, SqlComparisonEnum comparison) : base(tableName)
        {
            FieldName = fieldName;
            Value = value;
            Comparison = comparison;
        }

        public override string ToString()
        {
            string thisConditionStr;
            if (Value == null && Comparison == SqlComparisonEnum.Equals)
                thisConditionStr = string.Format(CultureInfo.InvariantCulture, "{0}.{1} is null",
                        TableName,
                        FieldName);
            else if (Value == null && Comparison == SqlComparisonEnum.Different)
                thisConditionStr = string.Format(CultureInfo.InvariantCulture, "{0}.{1} is not null",
                        TableName,
                        FieldName);
            else
                thisConditionStr = string.Format(CultureInfo.InvariantCulture, "{0}.{1} {2} {3}",
                    TableName,
                    FieldName,
                    SqlHelpers.SqlComparisonEnumToString[Comparison],
                    SqlHelpers.DecorateSqlValue(Value.AsSqlString()));

            if (this.And != null)
            {
                thisConditionStr = string.Format(CultureInfo.InvariantCulture, "(({0}) AND ({1}))",
                    thisConditionStr,
                    this.And.ToString());
            }

            if (this.Or != null)
            {
                thisConditionStr = string.Format(CultureInfo.InvariantCulture, "(({0}) OR ({1}))",
                    thisConditionStr,
                    this.Or.ToString());
            }

            return thisConditionStr;
        }
    }
}