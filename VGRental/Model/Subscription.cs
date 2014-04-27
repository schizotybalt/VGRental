using System;
using System.ComponentModel.DataAnnotations;

namespace VGRental.Model
{
    public class Subscription
    {
        public int SubscriptionID { get; set; }

        public int customerID { get; set; }
        public virtual Customer Customer { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public float Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ExpirationDate { get; set; }
        public string Description { get; set; }
    }
}
