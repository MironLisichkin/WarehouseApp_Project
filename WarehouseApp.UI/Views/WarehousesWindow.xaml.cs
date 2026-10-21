using System.Windows;
using WarehouseApp.Core.Models;
using WarehouseApp.UI.ViewModels;

namespace WarehouseApp.UI.Views
{
    public partial class WarehousesWindow : Window
    {
        public WarehousesWindow(Organization organization)
        {
            InitializeComponent();
            DataContext = new WarehouseViewModel(organization);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }
    }
}
