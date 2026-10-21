using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WarehouseApp.Core.Models
{
    public class Product : INotifyPropertyChanged
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid WarehouseId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DiscountPercent { get; set; }
        public string? PhotoPath { get; set; }

        private int _stock;
        public int Stock
        {
            get => _stock;
            set { _stock = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}