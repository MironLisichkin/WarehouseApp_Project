using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarehouseApp.Core.Models;
using WarehouseApp.Core.Services;

namespace WarehouseApp.Tests
{
    [TestClass]
    public class ProductServiceTests
    {
        [TestMethod]
        public void Add_ValidProduct_AddsToCollection()
        {
            var service = new ProductService();
            var product = new Product { Name = "Гвоздь", WarehouseId = Guid.NewGuid(), Price = 10, Stock = 100 };

            var result = service.Add(product);

            Assert.AreEqual("Гвоздь", result.Name);
            Assert.AreEqual(1, service.GetByWarehouse(product.WarehouseId).Count);
        }

        [TestMethod]
        public void Delete_ExistingProduct_RemovesIt()
        {
            var service = new ProductService();
            var whId = Guid.NewGuid();
            var product = service.Add(new Product { Name = "Молоток", WarehouseId = whId });

            service.Delete(product.Id);

            Assert.AreEqual(0, service.GetByWarehouse(whId).Count);
        }
    }
}
