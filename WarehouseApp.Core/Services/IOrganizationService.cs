using System;
using System.Collections.Generic;
using WarehouseApp.Core.Models;

namespace WarehouseApp.Core.Services
{
    public interface IOrganizationService
    {
        IReadOnlyList<Organization> GetAll();
        Organization Add(string name);
        void Update(Guid id, string newName);
        void Delete(Guid id);
    }
}