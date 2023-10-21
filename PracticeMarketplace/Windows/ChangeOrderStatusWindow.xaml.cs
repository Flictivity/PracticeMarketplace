using PracticeMarketplace.ADO;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;

namespace PracticeMarketplace.Windows
{
    /// <summary>
    /// Interaction logic for ChangeOrderStatusWindow.xaml
    /// </summary>
    public partial class ChangeOrderStatusWindow : Window
    {
        private Order _order;
        public ChangeOrderStatusWindow(Order order)
        {
            InitializeComponent();
            cbStatuses.ItemsSource = App.Connection.OrderStatus.ToList();
            _order = order;
        }

        private void SelectBtnClick(object sender, RoutedEventArgs e)
        {
            if (cbStatuses.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать статус!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            _order.OrderStatus = cbStatuses.SelectedItem as OrderStatus;
            App.Connection.Order.AddOrUpdate(_order);
            App.Connection.SaveChanges();
            this.DialogResult = true;
        }
    }
}
