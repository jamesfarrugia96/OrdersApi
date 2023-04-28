using System;
using System.Collections.Generic;

namespace Fabillio.Ordering.Public.Events
{

    public class OrderItem
    {
        public Guid ProductId { get; set; }

        public int Count { get; set; }
    }
}