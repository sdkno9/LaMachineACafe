using System;
using System.Runtime.Serialization;

namespace LaMachineACafe.Common
{
    [DataContract]
    public class DrinkSelection
    {
        [DataMember]
        public string BadgeReference { get; set; }

        [DataMember]
        public Guid Drink_Id { get; set; }

        [DataMember]
        public int SugarQuantity { get; set; }
    }
}