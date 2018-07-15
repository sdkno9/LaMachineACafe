using System.Runtime.Serialization;

namespace LaMachineACafe.Common
{
    [DataContract]
    public class Drink : IWithId
    {
        [DataMember]
        public string Name { get; set; }
    }
}