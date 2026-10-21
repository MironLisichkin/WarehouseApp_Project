using System;
using System.Collections.Generic;
using WarehouseApp.Core.Models;

namespace WarehouseApp.Core.Services
{
    public interface IWarehouseService
    {
        IReadOnlyList<Warehouse> GetByOrganization(Guid organizationId);
        Warehouse Add(Guid organizationId, string name);
        void Update(Guid id, string newName);
        void Delete(Guid id);
    }
}