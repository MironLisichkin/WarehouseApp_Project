using System;

namespace WarehouseApp.Core.Models
{
    public class Warehouse
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrganizationId { get; set; } 
        public string Name { get; set; } = string.Empty;
    }
}