using System.Linq;
using System.Windows;
using MaterialDesignThemes.Wpf;

namespace PracticeMarketplace.ADO
{
    public partial class Product
    {
        public Visibility RemoveBtnVisibility => App.Connection.Basket.Any(x => x.Product_Id == this.Id)
            ? Visibility.Visible
            : Visibility.Collapsed;

        public PackIconKind Icon => App.Connection.Basket.Any(x => x.Product_Id == this.Id)
            ? PackIconKind.CartAdd
            : PackIconKind.Plus;
    }
}