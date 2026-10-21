using System.Windows;

namespace WarehouseApp.UI.Views
{
    public partial class DeleteConfirmationWindow : Window
    {
        private readonly string _expectedName;

        public DeleteConfirmationWindow(string organizationName)
        {
            InitializeComponent();
            _expectedName = organizationName;
            PromptText.Text = $"Для подтверждения введите название:\n\"{organizationName}\"";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmTextBox.Text != _expectedName)
            {
                MessageBox.Show("Название введено неверно. Удаление отменено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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