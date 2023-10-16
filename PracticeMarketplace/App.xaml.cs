using PracticeMarketplace.ADO;
using System.Windows;

namespace PracticeMarketplace
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static PracticeMarketplaceEntities Connection = new PracticeMarketplaceEntities();
        public static User CurrentUser;
    }
}
