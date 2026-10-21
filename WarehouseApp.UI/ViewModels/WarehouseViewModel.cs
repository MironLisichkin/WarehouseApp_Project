using System;
using System.Collections.ObjectModel;
using System.ComponentModel; 
using System.Linq;
using System.Windows.Data;   
using System.Windows.Input;
using WarehouseApp.Core.Infrastructure;
using WarehouseApp.Core.Models;
using WarehouseApp.Core.Services;
using WarehouseApp.UI.Infrastructure;

namespace WarehouseApp.UI.ViewModels
{
    public class WarehouseViewModel : ViewModelBase
    {
        private readonly IWarehouseService _warehouseService;
        private readonly IProductService _productService;
        private readonly Organization _currentOrganization;

        private Warehouse? _selectedWarehouse;
        private Product? _selectedProduct;

        public string WindowTitle => $"Склады и товары: {_currentOrganization.Name}";

        public ObservableCollection<Warehouse> Warehouses { get; }
        public ObservableCollection<Product> Products { get; } = new();

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                ProductsView.Refresh(); 
            }
        }

        public ICollectionView ProductsView { get; }

        public Warehouse? SelectedWarehouse
        {
            get => _selectedWarehouse;
            set
            {
                SetProperty(ref _selectedWarehouse, value);
                LoadProducts();
            }
        }

        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICommand CreateIncomingCommand { get; }
        public ICommand CreateOutgoingCommand { get; }
        public ICommand ShowInvoicesCommand { get; }

        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public ICommand OpenOrganizationsCommand { get; }
        public ICommand FocusWarehousesCommand { get; }
        public ICommand FocusProductsCommand { get; }

        public WarehouseViewModel(Organization organization)
        {
            _currentOrganization = organization;
            _warehouseService = new WarehouseService();
            _productService = new ProductService();

            if (!_warehouseService.GetByOrganization(_currentOrganization.Id).Any())
            {
                DataSeeder.SeedWarehouses(_warehouseService, _currentOrganization.Id);
            }

            Warehouses = new ObservableCollection<Warehouse>(_warehouseService.GetByOrganization(_currentOrganization.Id));

            AddCommand = new RelayCommand(_ => AddWarehouse());
            EditCommand = new RelayCommand(_ => EditWarehouse(), _ => SelectedWarehouse != null);
            DeleteCommand = new RelayCommand(_ => DeleteWarehouse(), _ => SelectedWarehouse != null);

            CreateIncomingCommand = new RelayCommand(_ => OpenCart(InvoiceType.Incoming), _ => SelectedWarehouse != null);
            CreateOutgoingCommand = new RelayCommand(_ => OpenCart(InvoiceType.Outgoing), _ => SelectedWarehouse != null);
            ShowInvoicesCommand = new RelayCommand(_ => ShowInvoices(), _ => SelectedWarehouse != null);

            AddProductCommand = new RelayCommand(_ => AddProduct(), _ => SelectedWarehouse != null);
            EditProductCommand = new RelayCommand(_ => EditProduct(), _ => SelectedProduct != null);
            DeleteProductCommand = new RelayCommand(_ => DeleteProduct(), _ => SelectedProduct != null);

            OpenOrganizationsCommand = new RelayCommand(_ => CloseWindowToOrganizations());
            FocusWarehousesCommand = new RelayCommand(_ => SelectedWarehouse = Warehouses.FirstOrDefault());
            FocusProductsCommand = new RelayCommand(_ =>
                System.Windows.MessageBox.Show("Таблица товаров находится в правой части окна.", "Справка"));


            ProductsView = CollectionViewSource.GetDefaultView(Products);
            ProductsView.Filter = obj =>
            {
                if (string.IsNullOrWhiteSpace(SearchText)) return true;

                if (obj is Product p)
                {
                    return (p.Name?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                           (p.Category?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                           (p.Manufacturer?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false);
                }
                return false;
            };

            SelectedWarehouse = Warehouses.FirstOrDefault();
        }

        private void LoadProducts()
        {
            Products.Clear();
            if (SelectedWarehouse == null) return;

            var productsFromDb = _productService.GetByWarehouse(SelectedWarehouse.Id);

            if (!productsFromDb.Any())
            {
                DataSeeder.SeedProducts(_productService, SelectedWarehouse.Id);
                productsFromDb = _productService.GetByWarehouse(SelectedWarehouse.Id);
            }

            foreach (var p in productsFromDb)
                Products.Add(p);
        }

        private void AddWarehouse()
        {
            var dialog = new Views.OrganizationEditWindow("Добавление склада", "");
            if (dialog.ShowDialog() == true)
            {
                var newWh = _warehouseService.Add(_currentOrganization.Id, dialog.OrganizationName);
                Warehouses.Add(newWh);
            }
        }

        private void EditWarehouse()
        {
            if (SelectedWarehouse == null) return;
            var dialog = new Views.OrganizationEditWindow("Редактирование склада", SelectedWarehouse.Name);
            if (dialog.ShowDialog() == true)
            {
                _warehouseService.Update(SelectedWarehouse.Id, dialog.OrganizationName);
                var index = Warehouses.IndexOf(SelectedWarehouse);
                Warehouses[index] = new Warehouse { Id = SelectedWarehouse.Id, OrganizationId = _currentOrganization.Id, Name = dialog.OrganizationName };
                SelectedWarehouse = Warehouses[index];
            }
        }

        private void DeleteWarehouse()
        {
            if (SelectedWarehouse == null) return;
            var dialog = new Views.DeleteConfirmationWindow(SelectedWarehouse.Name);
            if (dialog.ShowDialog() == true)
            {
                _warehouseService.Delete(SelectedWarehouse.Id);
                Warehouses.Remove(SelectedWarehouse);
                Products.Clear();
            }
        }

        private void OpenCart(InvoiceType type)
        {
            if (SelectedWarehouse == null) return;
            if (Products.Count == 0)
            {
                System.Windows.MessageBox.Show("Сначала добавьте товары на склад.");
                return;
            }

            var dialog = new Views.InvoiceCartWindow(type, SelectedWarehouse.Id, Products.ToList());
            if (dialog.ShowDialog() == true)
            {
                RefreshProducts();
            }
        }

        private void ShowInvoices()
        {
            if (SelectedWarehouse == null) return;
            var dialog = new Views.InvoiceListWindow(SelectedWarehouse.Id, RefreshProducts);
            dialog.ShowDialog();
        }

        private void RefreshProducts()
        {
            if (SelectedWarehouse == null) return;
            var selectedId = SelectedProduct?.Id;
            LoadProducts();
            if (selectedId.HasValue)
                SelectedProduct = Products.FirstOrDefault(p => p.Id == selectedId);
        }
        private void AddProduct()
        {
            if (SelectedWarehouse == null) return;
            var dialog = new Views.ProductEditWindow("Добавление товара", SelectedWarehouse.Id);
            if (dialog.ShowDialog() == true)
            {
                RefreshProducts();
            }
        }

        private void EditProduct()
        {
            if (SelectedWarehouse == null || SelectedProduct == null) return;
            var dialog = new Views.ProductEditWindow("Редактирование товара", SelectedWarehouse.Id, SelectedProduct);
            if (dialog.ShowDialog() == true)
            {
                RefreshProducts();
            }
        }

        private void DeleteProduct()
        {
            if (SelectedProduct == null) return;
            var dialog = new Views.DeleteConfirmationWindow(SelectedProduct.Name);
            if (dialog.ShowDialog() == true)
            {
                _productService.Delete(SelectedProduct.Id);
                RefreshProducts();
            }
        }
        private void CloseWindowToOrganizations()
        {
            System.Windows.Application.Current.Windows.OfType<Views.WarehousesWindow>().FirstOrDefault()?.Close();
        }
    }
}