using System;

namespace VGRental.Model
{
    public class Order
    {
        public Guid OrderID { get; set; }

        public string GameName { get; set; }
        public string SystemName { get; set; }
        public virtual Game Game { get; set; }

        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual Promotion Promotion { get; set; }
    }
}
