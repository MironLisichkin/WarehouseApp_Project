using System.Windows;

namespace WarehouseApp.UI.Views
{
    public partial class OrganizationEditWindow : Window
    {
        public string OrganizationName { get; private set; }

        public OrganizationEditWindow(string title, string currentName)
        {
            InitializeComponent();
            Title = title;
            NameTextBox.Text = currentName;
            OrganizationName = currentName;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Наименование не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OrganizationName = NameTextBox.Text.Trim();
            DialogResult = true; 
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}