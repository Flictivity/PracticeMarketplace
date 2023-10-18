using PracticeMarketplace.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            this.DialogResult = true;
        }
    }
}
