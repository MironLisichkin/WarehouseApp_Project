using System;
using WarehouseApp.Core.Services;

namespace WarehouseApp.Core.Infrastructure
{
    public static class DataSeeder
    {
        public static void Seed(IOrganizationService orgService)
        {
            var org1 = orgService.Add("Рога и копыта, ООО");
            var org2 = orgService.Add("Пупкин и сыновья, ООО");
        }

        public static void SeedWarehouses(IWarehouseService warehouseService, Guid orgId)
        {
            warehouseService.Add(orgId, "Склад Рогов и Копыт №1");
            warehouseService.Add(orgId, "Склад Рогов и Копыт №2");
        }

        public static void SeedProducts(IProductService productService, Guid warehouseId)
        {
            productService.Add(new Models.Product { Name = "Молоток", Category = "Инструменты", Manufacturer = "Зубр", Supplier = "ООО Строй", Price = 500, Stock = 10, DiscountPercent = 5, WarehouseId = warehouseId });
            productService.Add(new Models.Product { Name = "Гвоздь 100мм", Category = "Крепеж", Manufacturer = "Мастер", Supplier = "ООО Метизы", Price = 5, Stock = 1000, DiscountPercent = 0, WarehouseId = warehouseId });
        }
    }
}