using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mozu.Api.Contracts.CommerceRuntime.Orders;

namespace MozuImport.Models
{
    public class OrderModel
    {
        public OrderModel()
        {
            OrderItems = new List<OrderItem>();
        }

        public Order Order { get; set; }

        public IList<OrderItem> OrderItems { get; set; }

    }
}
