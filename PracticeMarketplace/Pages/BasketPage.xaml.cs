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
    /// Interaction logic for BasketPage.xaml
    /// </summary>
    public partial class BasketPage : Page
    {
        public decimal TotalCost => (decimal)App.CurrentUser.Basket.Sum(x => x.Product.Cost * x.Count);
        public Visibility BasketEmptyTextVisibility => App.CurrentUser.Basket.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility BasketVisibility => App.CurrentUser.Basket.Count == 0 ? Visibility.Collapsed : Visibility.Visible;

        public BasketPage()
        {
            InitializeComponent();

            UpdateData();
        }

        private void UpdateData()
        {
            lvBasketItems.ItemsSource = App.Connection.Basket.Where(x => x.User_Id == App.CurrentUser.Id).ToList();

            lvBasketItems.Items.Refresh();

            tbTotalCost.Text = TotalCost.ToString();

            panelEmpty.Visibility = BasketEmptyTextVisibility;
            tbBasketCost.Visibility = BasketVisibility;
            btnCreateOrder.Visibility = BasketVisibility;
        }

        private void CreateOrder(object sender, RoutedEventArgs e)
        {
            var newOrder = new Order();
            var window = new OrderPointOfDeliverySelectWindow(newOrder);

            if (window.ShowDialog() == false)
            {
                return;
            }

            newOrder.User_Id = App.CurrentUser.Id;
            newOrder.Status_Id = 1;
            newOrder.CreationDate = DateTime.Today;

            foreach (var item in App.CurrentUser.Basket)
            {
                newOrder.OrderItem.Add(new OrderItem()
                {
                    Product_Id = item.Product_Id,
                    Count = (int)item.Count
                });
            }

            try
            {
                App.Connection.Order.Add(newOrder);
                var basket = App.Connection.Basket.Where(b => b.User_Id == App.CurrentUser.Id).ToList();

                foreach (var item in basket)
                {
                    App.Connection.Basket.Remove(item);
                }

                App.Connection.SaveChanges();

                UpdateData();

                var deliveryDate = DateTime.Now.AddDays(3);

                Snackbar.MessageQueue?.Enqueue($"Заказ оформлен. Номер заказа - {newOrder.Id}. " +
                                                $"Ожидаемая дата доставки - {deliveryDate.Date.ToString("d")} по адресу: {newOrder.PointOfDelivery.Address}",
                                                null, null, null,
                                                false, true, TimeSpan.FromSeconds(5));
            }
            catch
            {
                Snackbar.MessageQueue?.Enqueue($"При оформлении заказа произошла ошибка, пожалуйста, попробуйте позднее",
                                                null, null, null,
                                                false, true, TimeSpan.FromSeconds(3));
            }
        }

        private void RemoveFromBasketBtnClick(object sender, RoutedEventArgs e)
        {
            var productId = (int)((Button)sender).Tag;
            var product = App.CurrentUser.Basket.FirstOrDefault(x => x.Product_Id == productId);

            if (product.Count == 1)
            {
                var basketItem = App.CurrentUser.Basket.FirstOrDefault(x => x.Product_Id == productId);
                App.Connection.Basket.Remove(basketItem);
                App.Connection.SaveChanges();
                UpdateData();
                return;
            }

            product.Count--;
            App.Connection.SaveChanges();

            UpdateData();
        }

        private void AddToBasketBtnClick(object sender, RoutedEventArgs e)
        {
            var productId = (int)((Button)sender).Tag;
            var product = App.CurrentUser.Basket.FirstOrDefault(x => x.Product_Id == productId);

            product.Count++;
            App.Connection.SaveChanges();

            UpdateData();
        }
    }
}
