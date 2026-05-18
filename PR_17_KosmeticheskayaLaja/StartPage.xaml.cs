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

namespace PR_17_KosmeticheskayaLaja

{
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            LoadData();
            ConfigureAccess();
        }

        private void LoadData()
        {
            ComboMaster.ItemsSource = Core.Context.Users.Where(u => u.FID_Role == 2).ToList();
            ComboServiceType.ItemsSource = Core.Context.ServiceTypes.ToList();
            ListServices.ItemsSource = Core.Context.ServiceTypes.ToList();
        }

        private void ConfigureAccess()
        {
            if (Core.CurrentUser == null)
            {
                BtnAccount.Visibility = Visibility.Collapsed;
                BtnLogin.Visibility = Visibility.Visible;
            }
            else
            {
                BtnLogin.Visibility = Visibility.Collapsed;
                if (Core.CurrentUser.FID_Role == 1)
                {
                    BtnAccount.Visibility = Visibility.Visible;
                }
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductsPage());
        }

        private void ListServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListServices.SelectedItem is ServiceTypes selectedService)
            {
                var masters = Core.Context.MasterServices
                    .Where(ms => ms.FID_ServiceType == selectedService.ID_ServiceType)
                    .Select(ms => ms.Users)
                    .ToList();
                ListMasters.ItemsSource = masters;
            }
        }

        private void ListMasters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListMasters.SelectedItem is Users selectedMaster && ListServices.SelectedItem is ServiceTypes selectedService)
            {
                var appointments = Core.Context.Appointments
                    .Where(a => a.FID_Master == selectedMaster.ID_User && a.FID_ServiceType == selectedService.ID_ServiceType && !a.IsCompleted && !a.IsCanceled)
                    .ToList();
                ListAvailableTimes.ItemsSource = appointments;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}