using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PracticeMarketplace.ADO
{
    public partial class Order
    {
        public Visibility ChangeStatusBtnVisibility => App.CurrentUser.Role_Id == 1 ? Visibility.Visible : Visibility.Collapsed;
        public decimal TotalCost => OrderItem.Sum(x => x.Count * x.Product.Cost);
        public string CreatedDateString => CreationDate.ToString("d");
    }
}
