using LaMachineACafe.Common;
using System;
using System.Collections.Generic;

namespace LaMachineACafeService
{
    public class LaMachineACafeService : ILaMachineACafeService
    {
        private SqlDataLayer dataLayer;
        public LaMachineACafeService()
        {
            dataLayer = new SqlDataLayer();
        }
        public BaseResponse<List<Drink>> GetDrinks()
        {
            var drinks = dataLayer.GetEntities<Drink>();
            return new BaseResponse<List<Drink>>(code: ResultCodeEnum.Success, message: null, resultItem: drinks);
        }

        public BaseResponse<DrinkSelection> GetLastDrinkSelection(string badgeReference)
        {
            if (badgeReference == null)
                return new BaseResponse<DrinkSelection>(ResultCodeEnum.InvalidData, message: null, resultItem: null);

            var drinkSelection = dataLayer.GetDrinkSelectionByBadgeReference(badgeReference);
            return new BaseResponse<DrinkSelection>(ResultCodeEnum.Success, message: null, resultItem: drinkSelection);
        }

        public BaseResponse AddOrUpdateDrinkSelection(DrinkSelection drinkSelection)
        {
            if (drinkSelection.BadgeReference == null || drinkSelection.Drink_Id == Guid.Empty)
                return new BaseResponse(ResultCodeEnum.InvalidData, message: null);

            var drinkSelectionInDb = dataLayer.GetDrinkSelectionByBadgeReference(drinkSelection.BadgeReference);
            if (drinkSelectionInDb == null)
                dataLayer.Insert(drinkSelection);
            else
                dataLayer.Update(drinkSelection);
            return new BaseResponse(ResultCodeEnum.Success, message: null);
        }
    }
}
