using System.Runtime.Serialization;

namespace LaMachineACafe.Common
{
    [DataContract]
    public enum ResultCodeEnum
    {
        [EnumMember]
        Success = 0,

        [EnumMember]
        Fail = 1,

        [EnumMember]
        InvalidData = 2,
    }
}