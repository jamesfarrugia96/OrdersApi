using System;

namespace Fabillio.Ordering.Public.Contracts.Entities
{
    public class OrderItem
    {
        public Guid ProductId { get; set; }
    
        public int Count { get; set; }
    }
}