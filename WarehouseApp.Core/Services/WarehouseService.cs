using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseApp.Core.Models;

namespace WarehouseApp.Core.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly List<Warehouse> _warehouses = new();

        public IReadOnlyList<Warehouse> GetByOrganization(Guid organizationId)
        {
            return _warehouses.Where(w => w.OrganizationId == organizationId).ToList().AsReadOnly();
        }

        public Warehouse Add(Guid organizationId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название склада не может быть пустым.", nameof(name));

            var warehouse = new Warehouse { OrganizationId = organizationId, Name = name.Trim() };
            _warehouses.Add(warehouse);
            return warehouse;
        }

        public void Update(Guid id, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Название склада не может быть пустым.", nameof(newName));

            var warehouse = _warehouses.FirstOrDefault(w => w.Id == id);
            if (warehouse == null) throw new InvalidOperationException("Склад не найден.");

            warehouse.Name = newName.Trim();
        }

        public void Delete(Guid id)
        {
            var warehouse = _warehouses.FirstOrDefault(w => w.Id == id);
            if (warehouse == null) throw new InvalidOperationException("Склад не найден.");

            _warehouses.Remove(warehouse);
        }
    }
}