using System;
using System.Collections.Generic;
using System.Linq;

namespace LaMachineACafe.Common
{
    public static class DataAccessLayer
    {
        public static List<T> GetEntities<T>(this SqlDataLayer dataLayer)
            where T : class, new()
        {
            return dataLayer.ExecuteQuery<T>(new SqlQueryBuilder(tableType: typeof(T)));
        }

        public static List<Drink> GetDrinksByIdOrName(this SqlDataLayer dataLayer, Guid id, string name)
        {
            var query = new SqlQueryBuilder(tableType: typeof(Drink));
            var condition = new WhereCondition(tableName: SqlHelpers.TypeToTableName[typeof(Drink)], fieldName: SqlHelpers.IdFieldName, value: id, comparison: SqlComparisonEnum.Equals);
            if (!string.IsNullOrWhiteSpace(name))
                condition.OrWhere(fieldName: SqlHelpers.NameFieldName, value: name);
            query.Where(condition);
            return dataLayer.ExecuteQuery<Drink>(query);
        }

        public static DrinkSelection GetDrinkSelectionByBadgeReference(this SqlDataLayer dataLayer, string badgeReference)
        {
            var query = new SqlQueryBuilder(tableType: typeof(DrinkSelection))
                .Where(SqlHelpers.BadgeReferenceFieldName, badgeReference, SqlComparisonEnum.Equals);
            return dataLayer.ExecuteQuery<DrinkSelection>(query).FirstOrDefault();
        }
    }
}