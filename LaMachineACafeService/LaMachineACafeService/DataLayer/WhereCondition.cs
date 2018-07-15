using System;
using System.Globalization;

namespace LaMachineACafe.Common
{
    public class WhereCondition
    {
        public string TableName { get; private set; }
        public string FieldName { get; private set; }
        public object Value { get; private set; }
        public SqlComparisonEnum Comparison { get; private set; }

        public WhereCondition(string fieldName, string tableName, object value, SqlComparisonEnum comparison)
        {
            FieldName = fieldName;
            TableName = tableName;
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
            return thisConditionStr;
        }
    }
}