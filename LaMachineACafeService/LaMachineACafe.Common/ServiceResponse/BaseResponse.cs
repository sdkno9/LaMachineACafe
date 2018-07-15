using System.Runtime.Serialization;

namespace LaMachineACafe.Common
{
    [DataContract]
    public class BaseResponse
    {
        public BaseResponse(ResultCodeEnum code, string message)
        {
            ResultCode = code;
            Message = message;
        }

        [DataMember]
        public ResultCodeEnum ResultCode { get; set; }

        [DataMember]
        public string Message { get; set; }
    }

    [DataContract]
    public class BaseResponse<T> : BaseResponse
    {
        public BaseResponse(ResultCodeEnum code, string message, T resultItem) : base(code, message)
        {
            ResultItem = resultItem;
        }

        [DataMember]
        public T ResultItem { get; set; }
    }
}