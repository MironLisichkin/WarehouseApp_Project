using System;

namespace WarehouseApp.Core.Models
{
    public class Organization
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
    }
}
