using System;
using System.Collections.Generic;

namespace VGRental.Model
{
    public class Customer
    {
        public int customerID { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ShippingAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZIP { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
