using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WarehouseApp.Core.Infrastructure;
using WarehouseApp.Core.Models;
using WarehouseApp.Core.Services;
using WarehouseApp.UI.Infrastructure;

namespace WarehouseApp.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IOrganizationService _orgService;
        private Organization? _selectedOrganization;

        public ObservableCollection<Organization> Organizations { get; }

        public Organization? SelectedOrganization
        {
            get => _selectedOrganization;
            set => SetProperty(ref _selectedOrganization, value);
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand OpenWarehousesCommand { get; }

        public MainViewModel()
        {
            _orgService = new OrganizationService();

            DataSeeder.Seed(_orgService);

            Organizations = new ObservableCollection<Organization>(_orgService.GetAll());

            AddCommand = new RelayCommand(_ => AddOrganization());
            EditCommand = new RelayCommand(_ => EditOrganization(), _ => SelectedOrganization != null);
            DeleteCommand = new RelayCommand(_ => DeleteOrganization(), _ => SelectedOrganization != null);
            OpenWarehousesCommand = new RelayCommand(_ => OpenWarehouses(), _ => SelectedOrganization != null);
        }

        private void AddOrganization()
        {
            var dialog = new Views.OrganizationEditWindow("Добавление организации", "");
            if (dialog.ShowDialog() == true)
            {
                var newOrg = _orgService.Add(dialog.OrganizationName);
                Organizations.Add(newOrg);
            }
        }

        private void EditOrganization()
        {
            if (SelectedOrganization == null) return;

            var dialog = new Views.OrganizationEditWindow("Редактирование организации", SelectedOrganization.Name);
            if (dialog.ShowDialog() == true)
            {
                _orgService.Update(SelectedOrganization.Id, dialog.OrganizationName);

                var index = Organizations.IndexOf(SelectedOrganization);
                Organizations[index] = new Organization { Id = SelectedOrganization.Id, Name = dialog.OrganizationName };
                SelectedOrganization = Organizations[index];
            }
        }

        private void DeleteOrganization()
        {
            if (SelectedOrganization == null) return;

            var dialog = new Views.DeleteConfirmationWindow(SelectedOrganization.Name);
            if (dialog.ShowDialog() == true)
            {
                _orgService.Delete(SelectedOrganization.Id);
                Organizations.Remove(SelectedOrganization);
            }
        }
        private void OpenWarehouses()
        {
            if (SelectedOrganization == null) return;

            var warehousesWindow = new Views.WarehousesWindow(SelectedOrganization);

            warehousesWindow.Owner = System.Windows.Application.Current.MainWindow;

            warehousesWindow.ShowDialog();
        }
    }
}