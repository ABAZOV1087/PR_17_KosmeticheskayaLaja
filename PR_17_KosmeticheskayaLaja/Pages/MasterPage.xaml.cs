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
    public partial class MasterPage : Page
    {
        public MasterPage()
        {
            InitializeComponent();

            if (Core.CurrentUser != null)
            {
                TBlockWelcome.Text = $"Расписание мастера: {Core.CurrentUser.FullName}";
            }

            UpdateGrid();
        }

        private void UpdateGrid()
        {
            if (Core.CurrentUser == null) return;

            var appointments = Core.Context.Appointments
                .Where(a => a.FID_Master == Core.CurrentUser.ID_User)
                .ToList();

            foreach (var app in appointments)
            {
                app.Comment = app.IsCanceled ? "❌ Отменён" : "✅ Активен";
            }

            DGridAppointments.ItemsSource = appointments;
        }

        private void BtnCancelAppointment_Click(object sender, RoutedEventArgs e)
        {
            var selectedAppointment = DGridAppointments.SelectedItem as Appointments;

            if (selectedAppointment == null)
            {
                MessageBox.Show("Выберите сеанс из таблицы для отмены!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedAppointment.IsCanceled)
            {
                MessageBox.Show("Этот сеанс уже был отменен ранее.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите отменить этот сеанс?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var appInDb = Core.Context.Appointments.FirstOrDefault(a => a.ID_Appointment == selectedAppointment.ID_Appointment);
                if (appInDb != null)
                {
                    appInDb.IsCanceled = true;
                    Core.Context.SaveChanges();
                    MessageBox.Show("Сеанс успешно отменен.");
                    UpdateGrid();
                }
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Core.CurrentUser = null;
            NavigationService.Navigate(new LoginPage());
        }
    }
}
