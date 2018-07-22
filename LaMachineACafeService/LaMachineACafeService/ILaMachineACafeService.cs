using LaMachineACafe.Common;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace LaMachineACafe.Service
{
    [ServiceContract]
    public interface ILaMachineACafeService
    {

        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "drink/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        BaseResponse<List<Drink>> GetDrinks();

        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "drinkSelection/?badgeNumber={badgeNumber}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        BaseResponse<DrinkSelection> GetDrinkSelection(string badgeReference);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "drinkSelection/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        BaseResponse<DrinkSelection> AddOrUpdateDrinkSelection(DrinkSelection drinkSelection);
    }
}
