using System;

namespace VGRental.Model
{
    public class OrderDetails
    {
        public Guid OrderID { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? ShipDate { get; set; }
        public string Status { get; set; }

        public virtual Promotion Promotion { get; set; }
    }
}
