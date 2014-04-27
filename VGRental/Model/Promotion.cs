using System;

namespace VGRental.Model
{
    public class Promotion
    {
        public int PromotionID { get; set; }

        public int OrderID { get; set; }
        public virtual Order Order { get; set; }
        public virtual OrderDetails OrderDetails { get; set; }

        public DateTime ExpirationDate { get; set; }
        public int Amount { get; set; }
    }
}
