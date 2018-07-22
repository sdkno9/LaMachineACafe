using System.Runtime.Serialization;

namespace LaMachineACafe.Common
{
    [DataContract]
    public class BaseResponse
    {
        public BaseResponse(ResultCodeEnum code, string message = null)
        {
            ResultCode = code;
            Message = message;
        }

        [DataMember]
        public ResultCodeEnum ResultCode { get; set; }

        [DataMember]
        public string Message { get; set; }

        public bool IsSuccess()
        {
            return ResultCode == ResultCodeEnum.Success;
        }
    }

    [DataContract]
    public class BaseResponse<T> : BaseResponse
        where T : class
    {
        public BaseResponse(ResultCodeEnum code, string message = null, T resultItem = null) : base(code, message)
        {
            ResultItem = resultItem;
        }

        [DataMember]
        public T ResultItem { get; set; }
    }
}