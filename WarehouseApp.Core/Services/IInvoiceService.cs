using System;
using System.Collections.Generic;
using WarehouseApp.Core.Models;

namespace WarehouseApp.Core.Services
{
    public interface IInvoiceService
    {
        Invoice CreateInvoice(Guid warehouseId, InvoiceType type, List<InvoiceItem> items);
        IReadOnlyList<Invoice> GetByWarehouse(Guid warehouseId);
        void DeleteInvoice(Guid invoiceId);
    }
}
