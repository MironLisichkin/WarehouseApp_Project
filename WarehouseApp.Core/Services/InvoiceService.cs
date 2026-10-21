using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseApp.Core.Models;

namespace WarehouseApp.Core.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IProductService _productService;
        private readonly List<Invoice> _invoices = new(); 

        public InvoiceService(IProductService productService)
        {
            _productService = productService;
        }

        public Invoice CreateInvoice(Guid warehouseId, InvoiceType type, List<InvoiceItem> items)
        {
            if (items == null || items.Count == 0)
                throw new ArgumentException("Накладная не может быть пустой.");

            var invoice = new Invoice
            {
                WarehouseId = warehouseId,
                Type = type,
                Items = items
            };

            foreach (var item in items)
            {
                var product = _productService.GetById(item.ProductId);

                if (type == InvoiceType.Incoming)
                {
                    product.Stock += item.Quantity;
                }
                else 
                {
                    if (product.Stock < item.Quantity)
                        throw new InvalidOperationException($"Недостаточно товара '{product.Name}' на складе.");

                    product.Stock -= item.Quantity;
                }
            }

            _invoices.Add(invoice);
            return invoice;
        }

        public IReadOnlyList<Invoice> GetByWarehouse(Guid warehouseId)
        {
            return _invoices.Where(i => i.WarehouseId == warehouseId).OrderByDescending(i => i.CreatedAt).ToList().AsReadOnly();
        }

        public void DeleteInvoice(Guid invoiceId)
        {
            var invoice = _invoices.FirstOrDefault(i => i.Id == invoiceId);
            if (invoice == null) return;

            foreach (var item in invoice.Items)
            {
                var product = _productService.GetById(item.ProductId);
                if (invoice.Type == InvoiceType.Incoming)
                {
                    product.Stock -= item.Quantity;
                }
                else
                {
                    product.Stock += item.Quantity;
                }
            }

            _invoices.Remove(invoice);
        }
    }
}