using System;
using System.Collections.Generic;
using WarehouseApp.Core.Models;

namespace WarehouseApp.Core.Services
{
    public interface IProductService
    {
        IReadOnlyList<Product> GetByWarehouse(Guid warehouseId);
        Product GetById(Guid id);
        Product Add(Product product);
        void Update(Product product);
        void Delete(Guid id);
    }
}