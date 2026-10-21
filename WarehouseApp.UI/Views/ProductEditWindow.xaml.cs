using System;
using System.Windows;
using WarehouseApp.Core.Models;
using WarehouseApp.Core.Services;

namespace WarehouseApp.UI.Views
{
    public partial class ProductEditWindow : Window
    {
        public Product Product { get; private set; }
        private readonly IProductService _service;
        private readonly bool _isEdit;

        public ProductEditWindow(string title, Guid warehouseId, Product? existingProduct = null)
        {
            InitializeComponent();
            Title = title;
            _service = new ProductService(); 
            _isEdit = existingProduct != null;

            if (_isEdit)
            {
                Product = existingProduct!;
                NameBox.Text = Product.Name;
                CategoryBox.Text = Product.Category;
                ManufacturerBox.Text = Product.Manufacturer;
                SupplierBox.Text = Product.Supplier;
                PriceBox.Text = Product.Price.ToString();
                StockBox.Text = Product.Stock.ToString();
                DiscountBox.Text = Product.DiscountPercent.ToString();
                PhotoBox.Text = Product.PhotoPath;
            }
            else
            {
                Product = new Product { WarehouseId = warehouseId };
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) || !decimal.TryParse(PriceBox.Text, out decimal price))
            {
                MessageBox.Show("Проверьте правильность заполнения полей (Имя обязательно, Цена должна быть числом).");
                return;
            }

            Product.Name = NameBox.Text.Trim();
            Product.Category = CategoryBox.Text.Trim();
            Product.Manufacturer = ManufacturerBox.Text.Trim();
            Product.Supplier = SupplierBox.Text.Trim();
            Product.Price = price;
            Product.Stock = int.TryParse(StockBox.Text, out int stock) ? stock : 0;
            Product.DiscountPercent = decimal.TryParse(DiscountBox.Text, out decimal disc) ? disc : 0;
            Product.PhotoPath = PhotoBox.Text.Trim();

            try
            {
                if (_isEdit) _service.Update(Product);
                else _service.Add(Product);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
