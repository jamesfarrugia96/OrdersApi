using System;
using System.Collections.Generic;
using Fabillio.Ordering.Public.Contracts.Entities;

namespace Fabillio.Ordering.Public.Contracts.Responses
{

    public class OrderResponse
    {
        public Guid OrderId { get; set; }

        public Guid CustomerId { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}