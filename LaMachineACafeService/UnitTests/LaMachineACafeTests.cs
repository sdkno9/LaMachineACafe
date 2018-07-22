using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LaMachineACafe.Service;
using LaMachineACafe.Common;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class LaMachineACafeTests
    {
        private LaMachineACafeService _service = null;
        private readonly object lockObject = new object();
        LaMachineACafeService service
        {
            get
            {
                if (_service == null)
                    lock (lockObject)
                        if (_service == null)
                            _service = new LaMachineACafeService();

                return _service;
            }
        }

        private static List<Guid> DrinksToCleanup = new List<Guid>();

        [TestMethod]
        public void AddAndUpdateDrinksTest()
        {
            //Add a new drink
            var coffee = AddNewDrink("Coffee").AssertSuccess().ResultItem;

            //Get the drinks list
            var drinks = service.GetDrinks().AssertSuccess().ResultItem;
            Assert.IsNotNull(drinks);
            Assert.AreEqual(1, drinks.Count, "only one drink should have been added");
            Assert.AreEqual(1, drinks
                .Count(d => d.Id == coffee.Id && d.Name.Equals(coffee.Name, StringComparison.InvariantCulture)),
                "the returned drink's properties should have been correctly saved");

            //Try to add a new drink using the previously added drink's name
            AddNewDrink("Coffee").AssertResultCode(ResultCodeEnum.Fail, "we should not be able to add a new drink using an existing drink name");

            //Update the name of an existing drink
            var teaName = "Tea";
            var tea = UpdateDrink(new Drink() { Id = coffee.Id, Name = teaName }).AssertSuccess().ResultItem;
            Assert.IsNotNull(tea);
            Assert.AreEqual(teaName, tea.Name, "the drink's name should have been successfully updated");

            //Try to update a drink using a non existing Id
            UpdateDrink(new Drink() { Id = Guid.NewGuid(), Name = tea.Name }).AssertResultCode(ResultCodeEnum.Fail);
        }

        [TestMethod]
        public void AddAndUpdateDrinkSelectionTest()
        {
            //Add a new drink
            var coffee = AddNewDrink("Coffee").AssertSuccess().ResultItem;
            var tea = AddNewDrink("Tea").AssertSuccess().ResultItem;
            var chocolate = AddNewDrink("Choclate").AssertSuccess().ResultItem;

            //Get the drinks list
            var drinks = service.GetDrinks().AssertSuccess().ResultItem;
            Assert.IsNotNull(drinks);
            Assert.AreEqual(3, drinks.Count, "we should have three drinks added");

            //Give a badge to the user
            var badgeReference = TestHelpers.GetRandomString(10);
            var sugarQuantityForCoffee = 3;
            var sugarQuantityForTea = 1;

            //Check the last drink selection
            var lastDrinkSelection = service.GetDrinkSelection(badgeReference).AssertSuccess().ResultItem;
            Assert.IsNull(lastDrinkSelection, "no selection is associated with this badge");

            //Buy a coffee
            var coffeeSelection = SelectDrink(badgeReference, coffee.Id, sugarQuantity: sugarQuantityForCoffee).AssertSuccess().ResultItem;
            Assert.IsTrue(coffeeSelection.BadgeReference.Equals(badgeReference, StringComparison.InvariantCultureIgnoreCase));
            Assert.AreEqual(coffee.Id, coffeeSelection.Drink_Id);
            Assert.AreEqual(sugarQuantityForCoffee, coffeeSelection.SugarQuantity);

            //Check the last drink selection
            lastDrinkSelection = service.GetDrinkSelection(badgeReference).AssertSuccess().ResultItem;
            Assert.IsNotNull(lastDrinkSelection, "we should have a selection associated with this badge");
            Assert.IsTrue(lastDrinkSelection.BadgeReference.Equals(badgeReference, StringComparison.InvariantCultureIgnoreCase));
            Assert.AreEqual(coffee.Id, lastDrinkSelection.Drink_Id);
            Assert.AreEqual(sugarQuantityForCoffee, lastDrinkSelection.SugarQuantity);

            //Buy a tea
            var teaSelection = SelectDrink(badgeReference, tea.Id, sugarQuantity: sugarQuantityForTea).AssertSuccess().ResultItem;
            Assert.IsTrue(teaSelection.BadgeReference.Equals(badgeReference, StringComparison.InvariantCultureIgnoreCase));
            Assert.AreEqual(tea.Id, teaSelection.Drink_Id);
            Assert.AreEqual(sugarQuantityForTea, teaSelection.SugarQuantity);

            //Check the last drink selection
            lastDrinkSelection = service.GetDrinkSelection(badgeReference).AssertSuccess().ResultItem;
            Assert.IsNotNull(lastDrinkSelection, "we should have a selection associated with this badge");
            Assert.IsTrue(lastDrinkSelection.BadgeReference.Equals(badgeReference, StringComparison.InvariantCultureIgnoreCase));
            Assert.AreEqual(tea.Id, lastDrinkSelection.Drink_Id);
            Assert.AreEqual(sugarQuantityForTea, lastDrinkSelection.SugarQuantity);
        }

        public BaseResponse<Drink> AddNewDrink(string name)
        {
            var drinkToAdd = new Drink()
            {
                Id = Guid.NewGuid(),
                Name = name
            };
            DrinksToCleanup.Add(drinkToAdd.Id);
            return service.AddDrink(drinkToAdd);
        }

        public BaseResponse<Drink> UpdateDrink(Drink drinkToUpdate)
        {
            return service.UpdateDrink(drinkToUpdate);
        }

        public BaseResponse<DrinkSelection> SelectDrink(string badgeReference, Guid drinkId, int sugarQuantity)
        {
            var drinkSelectionToAdd = new DrinkSelection()
            {
                BadgeReference = badgeReference,
                Drink_Id = drinkId,
                SugarQuantity = sugarQuantity
            };
            return service.AddOrUpdateDrinkSelection(drinkSelectionToAdd);
        }

        [TestCleanup]
        public void CleanupAfterTest()
        {
            service.DeleteDrinks(DrinksToCleanup);
        }
    }
}
