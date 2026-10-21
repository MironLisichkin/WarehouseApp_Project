using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WarehouseApp.Core.Models;
using WarehouseApp.Core.Services;

namespace WarehouseApp.UI.Views
{
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    public partial class InvoiceCartWindow : Window
    {
        private readonly InvoiceType _type;
        private readonly Guid _warehouseId;
        private readonly IProductService _productService;
        private readonly ObservableCollection<CartItem> _cartItems = new();
        private readonly List<Product> _availableProducts;

        public InvoiceCartWindow(InvoiceType type, Guid warehouseId, List<Product> availableProducts)
        {
            InitializeComponent();
            _type = type;
            _warehouseId = warehouseId;
            _productService = new ProductService();
            _availableProducts = availableProducts;

            Title = type == InvoiceType.Incoming ? "Создание накладной: Приход" : "Создание накладной: Расход";

            ProductBox.ItemsSource = _availableProducts;
            if (_availableProducts.Any()) ProductBox.SelectedIndex = 0;

            CartGrid.ItemsSource = _cartItems;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductBox.SelectedItem is not Product selectedProduct) return;
            if (!int.TryParse(QtyBox.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Некорректное количество.");
                return;
            }

            var existing = _cartItems.FirstOrDefault(c => c.ProductId == selectedProduct.Id);
            if (existing != null)
            {
                existing.Quantity += qty;
                CartGrid.Items.Refresh(); 
            }
            else
            {
                _cartItems.Add(new CartItem { ProductId = selectedProduct.Id, ProductName = selectedProduct.Name, Quantity = qty });
            }

            QtyBox.Text = "1"; 
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_cartItems.Any())
            {
                MessageBox.Show("Накладная пуста. Добавьте товары.");
                return;
            }

            try
            {
                var invoiceItems = _cartItems.Select(c => new InvoiceItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity
                }).ToList();

                var invoiceService = new InvoiceService(_productService);
                invoiceService.CreateInvoice(_warehouseId, _type, invoiceItems);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
