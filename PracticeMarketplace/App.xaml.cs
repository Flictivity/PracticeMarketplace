using PracticeMarketplace.ADO;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace PracticeMarketplace
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static PracticeMarketplaceEntities Connection = new PracticeMarketplaceEntities();
        public static User CurrentUser = Connection.User.FirstOrDefault(x => x.Id==2);
    }
}
