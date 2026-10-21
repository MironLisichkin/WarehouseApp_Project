using System;
using System.Linq;
using System.Windows;
using WarehouseApp.Core.Models;
using WarehouseApp.Core.Services;

namespace WarehouseApp.UI.Views
{
    public partial class InvoiceListWindow : Window
    {
        private readonly IProductService _productService;
        private readonly IInvoiceService _invoiceService;
        private readonly Guid _warehouseId;
        private readonly Action _onUpdate; 

        public InvoiceListWindow(Guid warehouseId, Action onUpdate)
        {
            InitializeComponent();
            _warehouseId = warehouseId;
            _onUpdate = onUpdate;
            _productService = new ProductService();
            _invoiceService = new InvoiceService(_productService);

            LoadInvoices();
        }

        private void LoadInvoices()
        {
            var invoices = _invoiceService.GetByWarehouse(_warehouseId).Select(i => new
            {
                Id = i.Id.ToString().Substring(0, 8).ToUpper(), 
                RealId = i.Id,
                i.CreatedAt,
                Type = i.Type == InvoiceType.Incoming ? "Приход" : "Расход",
                i.Items
            }).ToList();

            InvoicesGrid.ItemsSource = invoices;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (InvoicesGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите накладную для удаления.");
                return;
            }

            dynamic selected = InvoicesGrid.SelectedItem;
            Guid realId = selected.RealId;

            if (MessageBox.Show("Удалить накладную? Остатки товаров будут откачены.", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _invoiceService.DeleteInvoice(realId);
                _onUpdate?.Invoke(); 
                LoadInvoices(); 
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}
