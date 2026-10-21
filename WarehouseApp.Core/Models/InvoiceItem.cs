using System;

namespace WarehouseApp.Core.Models
{
    public class InvoiceItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
