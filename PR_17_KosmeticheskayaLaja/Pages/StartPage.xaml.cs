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

namespace PR_17_KosmeticheskayaLaja.Pages
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
            NavigationService.Navigate(new ProductPage());
        }

        private void ListServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListServices.SelectedItem is ServiceTypes selectedService)
            {
                // Находим мастеров, у которых есть эта услуга, через навигационное свойство Users
                // (EF автоматически связывает их, если таблица MasterServices была промежуточной)
                var masters = selectedService.Users.Where(u => u.FID_Role == 2).ToList();
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
        private void ListAvailableTimes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListAvailableTimes.SelectedItem is Appointments selectedAppointment)
            {
                if (Core.CurrentUser == null)
                {
                    MessageBox.Show("Для записи необходимо войти в систему!");
                    return;
                }

                var service = ListServices.SelectedItem as ServiceTypes;
                var master = ListMasters.SelectedItem as Users;

                if (service != null && master != null)
                {
                    NavigationService.Navigate(new AppointmentConfirmPage(service, master, selectedAppointment));
                }
            }
        }

    }
}
