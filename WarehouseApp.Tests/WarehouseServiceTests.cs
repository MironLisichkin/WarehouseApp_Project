using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarehouseApp.Core.Services;

namespace WarehouseApp.Tests
{
    [TestClass]
    public class WarehouseServiceTests
    {
        [TestMethod]
        public void Add_ValidData_AddsWarehouse()
        {
            var service = new WarehouseService();
            var orgId = Guid.NewGuid();

            var result = service.Add(orgId, "Склад №1");

            Assert.AreEqual("Склад №1", result.Name);
            Assert.AreEqual(1, service.GetByOrganization(orgId).Count);
        }

        [TestMethod]
        public void GetByOrganization_ReturnsOnlyRelatedWarehouses()
        {
            var service = new WarehouseService();
            var org1 = Guid.NewGuid();
            var org2 = Guid.NewGuid();

            service.Add(org1, "Склад А");
            service.Add(org2, "Склад Б");

            var org1Warehouses = service.GetByOrganization(org1);

            Assert.AreEqual(1, org1Warehouses.Count);
            Assert.AreEqual("Склад А", org1Warehouses.First().Name);
        }

        [TestMethod]
        public void Delete_ExistingWarehouse_RemovesIt()
        {
            var service = new WarehouseService();
            var orgId = Guid.NewGuid();
            var wh = service.Add(orgId, "Склад для удаления");

            service.Delete(wh.Id);

            Assert.AreEqual(0, service.GetByOrganization(orgId).Count);
        }
    }
}