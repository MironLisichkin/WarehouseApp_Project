using System;
using System.Collections.Generic;

namespace WarehouseApp.Core.Models
{
    public class Invoice
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid WarehouseId { get; set; }
        public InvoiceType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<InvoiceItem> Items { get; set; } = new();
    }
}
