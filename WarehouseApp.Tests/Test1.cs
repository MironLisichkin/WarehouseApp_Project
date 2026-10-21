using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarehouseApp.Core.Services;

namespace WarehouseApp.Tests
{
    [TestClass]
    public class OrganizationServiceTests
    {
        [TestMethod]
        public void Add_ValidName_AddsOrganizationToCollection()
        {
            var service = new OrganizationService();

            var result = service.Add("ООО Ромашка");

            Assert.IsNotNull(result);
            Assert.AreEqual("ООО Ромашка", result.Name);
            Assert.AreEqual(1, service.GetAll().Count);
        }

        [TestMethod]
        public void Add_EmptyName_ThrowsArgumentException()
        {
            var service = new OrganizationService();

            Assert.ThrowsException<ArgumentException>(() => service.Add(""));
        }

        [TestMethod]
        public void Update_ExistingOrganization_ChangesName()
        {
            var service = new OrganizationService();
            var org = service.Add("Старое имя");

            service.Update(org.Id, "Новое имя");

            var updatedOrg = service.GetAll().First();
            Assert.AreEqual("Новое имя", updatedOrg.Name);
        }

        [TestMethod]
        public void Delete_ExistingOrganization_RemovesIt()
        {

            var service = new OrganizationService();
            var org = service.Add("Временная организация");

            service.Delete(org.Id);

            Assert.AreEqual(0, service.GetAll().Count);
        }
    }
}