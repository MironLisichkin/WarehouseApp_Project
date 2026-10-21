using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using WarehouseApp.Core.Models;
using WarehouseApp.Core.Services;

namespace WarehouseApp.Tests
{
    [TestClass]
    public class InvoiceServiceTests
    {
        [TestMethod]
        public void CreateInvoice_Incoming_IncreasesStock()
        {
            var prodService = new ProductService();
            var invoiceService = new InvoiceService(prodService);
            var whId = Guid.NewGuid();

            var product = prodService.Add(new Product { Name = "Гвоздь", WarehouseId = whId, Stock = 10 });

            invoiceService.CreateInvoice(whId, InvoiceType.Incoming, new List<InvoiceItem>
            {
                new InvoiceItem { ProductId = product.Id, Quantity = 5 }
            });

            Assert.AreEqual(15, product.Stock);
        }

        [TestMethod]
        public void CreateInvoice_Outgoing_DecreasesStock()
        {
            var prodService = new ProductService();
            var invoiceService = new InvoiceService(prodService);
            var whId = Guid.NewGuid();

            var product = prodService.Add(new Product { Name = "Гвоздь", WarehouseId = whId, Stock = 10 });

            invoiceService.CreateInvoice(whId, InvoiceType.Outgoing, new List<InvoiceItem>
            {
                new InvoiceItem { ProductId = product.Id, Quantity = 4 }
            });

            Assert.AreEqual(6, product.Stock); 
        }

        [TestMethod]
        public void CreateInvoice_Outgoing_ThrowsIfNotEnoughStock()
        {
            var prodService = new ProductService();
            var invoiceService = new InvoiceService(prodService);
            var whId = Guid.NewGuid();

            var product = prodService.Add(new Product { Name = "Гвоздь", WarehouseId = whId, Stock = 2 });

            Assert.ThrowsException<InvalidOperationException>(() =>
                invoiceService.CreateInvoice(whId, InvoiceType.Outgoing, new List<InvoiceItem>
                {
                    new InvoiceItem { ProductId = product.Id, Quantity = 5 }
                }));
        }
        [TestMethod]
        public void DeleteInvoice_Incoming_RollsBackStock()
        {
            var prodService = new ProductService();
            var invoiceService = new InvoiceService(prodService);
            var whId = Guid.NewGuid();
            var product = prodService.Add(new Product { Name = "Тест", WarehouseId = whId, Stock = 10 });

            var invoice = invoiceService.CreateInvoice(whId, InvoiceType.Incoming, new List<InvoiceItem>
            {
                new InvoiceItem { ProductId = product.Id, Quantity = 5 }
            });

            Assert.AreEqual(15, product.Stock); 

            invoiceService.DeleteInvoice(invoice.Id);

            Assert.AreEqual(10, product.Stock); 
        }
    }
}
