using System;
using System.Collections.Generic;
using Fabillio.Ordering.Public.Contracts.Entities;

namespace Fabillio.Ordering.Public.Contracts.Requests
{

    public class PlaceOrderRequest
    {
        public Guid CustomerId { get; set; }

        public List<OrderItem> Items { get; set; }
    }

}