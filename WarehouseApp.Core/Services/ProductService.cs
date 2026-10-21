using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseApp.Core.Models;

namespace WarehouseApp.Core.Services
{
    public class ProductService : IProductService
    {
        private static readonly List<Product> _products = new();

        public IReadOnlyList<Product> GetByWarehouse(Guid warehouseId)
            => _products.Where(p => p.WarehouseId == warehouseId).ToList().AsReadOnly();

        public Product GetById(Guid id)
            => _products.FirstOrDefault(p => p.Id == id)
               ?? throw new InvalidOperationException("Товар не найден.");

        public Product Add(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Наименование товара не может быть пустым.");

            if (product.Price < 0 || product.Stock < 0)
                throw new ArgumentException("Цена и остаток не могут быть отрицательными.");

            _products.Add(product);
            return product;
        }

        public void Update(Product product)
        {
            if (product.Price < 0 || product.Stock < 0)
                throw new ArgumentException("Цена и остаток не могут быть отрицательными.");

            var existing = GetById(product.Id);
            existing.Name = product.Name;
            existing.Category = product.Category;
            existing.Manufacturer = product.Manufacturer;
            existing.Supplier = product.Supplier;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            existing.DiscountPercent = product.DiscountPercent;
            existing.PhotoPath = product.PhotoPath;
        }

        public void Delete(Guid id)
        {
            var product = GetById(id);
            _products.Remove(product);
        }
    }
}
