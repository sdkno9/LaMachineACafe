using LaMachineACafe.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaMachineACafe.Service
{
    public class LaMachineACafeService : ILaMachineACafeService
    {
        private SqlDataLayer dataLayer;
        public LaMachineACafeService()
        {
            dataLayer = new SqlDataLayer();
        }

        public BaseResponse<Drink> AddDrink(Drink drink)
        {
            if (drink == null || drink.Id == Guid.Empty)
                return new BaseResponse<Drink>(ResultCodeEnum.InvalidData);

            var drinkInDb = dataLayer.GetDrinksByIdOrName(id: drink.Id, name: drink.Name).FirstOrDefault();
            if (drinkInDb == null)
                dataLayer.Insert(drink);
            else
                return new BaseResponse<Drink>(ResultCodeEnum.Fail);
            return new BaseResponse<Drink>(ResultCodeEnum.Success, resultItem: drink);
        }

        public BaseResponse<Drink> UpdateDrink(Drink drink)
        {
            if (drink == null || drink.Id == Guid.Empty)
                return new BaseResponse<Drink>(ResultCodeEnum.InvalidData);

            var drinks = dataLayer.GetDrinksByIdOrName(id: drink.Id, name: drink.Name);
            if (drinks == null || !drinks.Any(d => d.Id == drink.Id) || drinks.Any(d => d.Name.Equals(drink.Name, StringComparison.InvariantCultureIgnoreCase)))
                return new BaseResponse<Drink>(ResultCodeEnum.Fail);
            else
                dataLayer.Update(drink);
            return new BaseResponse<Drink>(ResultCodeEnum.Success, resultItem: drink);
        }

        public BaseResponse<List<Drink>> GetDrinks()
        {
            var drinks = dataLayer.GetEntities<Drink>();
            return new BaseResponse<List<Drink>>(code: ResultCodeEnum.Success, message: null, resultItem: drinks);
        }

        public BaseResponse<DrinkSelection> GetDrinkSelection(string badgeReference)
        {
            if (badgeReference == null)
                return new BaseResponse<DrinkSelection>(ResultCodeEnum.InvalidData, message: null, resultItem: null);

            var drinkSelection = dataLayer.GetDrinkSelectionByBadgeReference(badgeReference);
            return new BaseResponse<DrinkSelection>(ResultCodeEnum.Success, message: null, resultItem: drinkSelection);
        }

        public BaseResponse<DrinkSelection> AddOrUpdateDrinkSelection(DrinkSelection drinkSelection)
        {
            if (drinkSelection.BadgeReference == null || drinkSelection.Drink_Id == Guid.Empty)
                return new BaseResponse<DrinkSelection>(ResultCodeEnum.InvalidData, message: null);

            var drinkSelectionInDb = dataLayer.GetDrinkSelectionByBadgeReference(drinkSelection.BadgeReference);
            if (drinkSelectionInDb == null)
                dataLayer.Insert(drinkSelection);
            else
                dataLayer.Update(drinkSelection);
            return new BaseResponse<DrinkSelection>(ResultCodeEnum.Success, message: null, resultItem: drinkSelection);
        }

        public BaseResponse DeleteDrinks(List<Guid> ids)
        {
            dataLayer.Delete<DrinkSelection>(SqlHelpers.DrinkIdFieldName, ids.Select(id => (object)id).ToList());
            dataLayer.Delete<Drink>(SqlHelpers.IdFieldName, ids.Select(id => (object)id).ToList());
            return new BaseResponse(ResultCodeEnum.Success);
        }
    }
}
