using System;
using System.Runtime.Serialization;

namespace LaMachineACafe.Common
{
    [DataContract]
    public class Drink
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}