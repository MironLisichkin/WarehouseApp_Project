using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseApp.Core.Models;

namespace WarehouseApp.Core.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly List<Organization> _organizations = new();

        public IReadOnlyList<Organization> GetAll()
        {
            return _organizations.AsReadOnly();
        }

        public Organization Add(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Наименование не может быть пустым.", nameof(name));

            var org = new Organization { Name = name.Trim() };
            _organizations.Add(org);
            return org;
        }

        public void Update(Guid id, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Наименование не может быть пустым.", nameof(newName));

            var org = _organizations.FirstOrDefault(o => o.Id == id);
            if (org == null)
                throw new InvalidOperationException("Организация не найдена.");

            org.Name = newName.Trim();
        }

        public void Delete(Guid id)
        {
            var org = _organizations.FirstOrDefault(o => o.Id == id);
            if (org == null)
                throw new InvalidOperationException("Организация не найдена.");

            _organizations.Remove(org);
        }
    }
}