using PracticeMarketplace.ADO;
using PracticeMarketplace.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PracticeMarketplace.Pages
{
    /// <summary>
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public List<Order> SourceData { get; set; }
        private Predicate<Order> _statusFilterQuery = x => true;
        public OrdersPage()
        {
            InitializeComponent();
            SetEmptyOrdersTextVisibility();
            FillFilterData();
            UpdateData();
        }

        private void SetEmptyOrdersTextVisibility()
        {
            panelEmpty.Visibility = App.CurrentUser.Order.Count == 0 && App.CurrentUser.Role_Id != 1 ? Visibility.Visible : Visibility.Collapsed;
            cbStatuses.Visibility = App.CurrentUser.Order.Count == 0 && App.CurrentUser.Role_Id != 1 ? Visibility.Collapsed : Visibility.Visible;
        }

        private void FillFilterData()
        {
            var statuses = App.Connection.OrderStatus.ToList();
            statuses.Add(new OrderStatus()
            {
                Name = "Все"
            });

            cbStatuses.ItemsSource = statuses;
            cbStatuses.SelectedItem = statuses.Last();
        }

        private void UpdateData()
        {
            if (lvOrders == null)
            {
                return;
            }

            if (App.CurrentUser.Role.Id == 1)
            {
                SourceData = App.Connection.Order.ToList().Where(x => _statusFilterQuery(x)).OrderByDescending(x => x.CreationDate).ToList();
            }
            else
            {
                SourceData = App.CurrentUser.Order.Where(x => _statusFilterQuery(x)).OrderByDescending(x => x.CreationDate).ToList();
            }

            lvOrders.ItemsSource = SourceData;
        }

        private void StatusFilter(object sender, SelectionChangedEventArgs e)
        {
            if (lvOrders == null || cbStatuses.SelectedItem == null)
            {
                return;
            }

            var item = cbStatuses.SelectedItem as OrderStatus;

            if (item.Name == "Все")
            {
                _statusFilterQuery = x => true;
            }
            else
            {
                _statusFilterQuery = x => x.Status_Id == item.Id;
            }

            UpdateData();
        }

        private void ChangeStatusBtnClick(object sender, RoutedEventArgs e)
        {
            var window = new ChangeOrderStatusWindow((Order)((Button)sender).Tag);

            if (window.ShowDialog() == false)
            {
                return;
            }
            UpdateData();
        }
    }
}
