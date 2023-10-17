using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for OrderPointOfDeliverySelectWindow.xaml
    /// </summary>
    public partial class OrderPointOfDeliverySelectWindow : Window
    {
        private Order _order;
        public OrderPointOfDeliverySelectWindow(Order order)
        {
            InitializeComponent();
            cbPointsOfDelivery.ItemsSource = App.Connection.PointOfDelivery.ToList();
            _order = order;
        }

        private void SelectBtnClick(object sender, RoutedEventArgs e)
        {
            if (cbPointsOfDelivery.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать пункт выдачи!", "Сообщение", MessageBoxButton.OK,MessageBoxImage.Information);
                return;
            }
            _order.PointOfDelivery = cbPointsOfDelivery.SelectedItem as PointOfDelivery;
            this.DialogResult = true;
        }
    }
}
