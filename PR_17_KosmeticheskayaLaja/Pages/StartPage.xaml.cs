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

            ComboServices.ItemsSource = Core.Context.ServiceTypes.ToList();
            ComboMasters.ItemsSource = Core.Context.Users.Where(u => u.FID_Role == 2).ToList();
            PickerDate.SelectedDate = DateTime.Now;
        }

        private void BtnBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedService = ComboServices.SelectedItem as ServiceTypes;
            var selectedMaster = ComboMasters.SelectedItem as Users;
            DateTime? selectedDate = PickerDate.SelectedDate;
            string timeStr = TBoxTime.Text.Trim();

            if (selectedService == null || selectedMaster == null || !selectedDate.HasValue || string.IsNullOrEmpty(timeStr))
            {
                MessageBox.Show("Пожалуйста, заполните все поля для записи!");
                return;
            }

            try
            {
                TimeSpan time = TimeSpan.Parse(timeStr);

                Appointments newAppointment = new Appointments
                {
                    AppointmentDate = selectedDate.Value,
                    AppointmentTime = time,
                    FID_Master = selectedMaster.ID_User,
                    FID_Service = selectedService.ID_ServiceType,
                    FID_Client = Core.CurrentUser != null ? Core.CurrentUser.ID_User : 1, // Если зашел гость/клиент
                    IsCanceled = false
                };

                Core.Context.Appointments.Add(newAppointment);
                Core.Context.SaveChanges();

                MessageBox.Show("Вы успешно записаны на сеанс!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                TBoxTime.Clear();
            }
            catch
            {
                MessageBox.Show("Некорректный формат времени! Используйте чч:мм (например, 14:30).");
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Core.CurrentUser = null;
            NavigationService.Navigate(new LoginPage());
        }
        private void BtnGoToCatalog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductPage());
        }
    }
}
