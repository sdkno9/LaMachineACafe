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

        public static DrinkSelection GetDrinkSelectionByBadgeReference(this SqlDataLayer dataLayer, string badgeReference)
        {
            var query = new SqlQueryBuilder(tableType: typeof(DrinkSelection));
            if (badgeReference != null)
                query.Where(SqlHelpers.BadgeReferenceFieldName, badgeReference, SqlComparisonEnum.Equals);
            return dataLayer.ExecuteQuery<DrinkSelection>(query).FirstOrDefault();
        }
    }
}