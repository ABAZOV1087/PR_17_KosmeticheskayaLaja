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
            DPickerSchedule.SelectedDate = DateTime.Now.Date;

            if (Core.CurrentUser != null)
            {
                TxtMasterInfo.Text = $"Мастер: {Core.CurrentUser.FullName}";
            }

            UpdateSchedule();
        }

        private void UpdateSchedule()
        {
            if (Core.CurrentUser == null || DPickerSchedule.SelectedDate == null) return;

            DateTime selectedDate = DPickerSchedule.SelectedDate.Value.Date;

            // Выбираем только те записи, которые НЕ отменены (IsCanceled == false)
            // И которые принадлежат текущему авторизованному мастеру
            var schedule = Core.Context.Appointments
                .Where(a => a.FID_Master == Core.CurrentUser.ID_User
                         && a.AppointmentDate == selectedDate
                         && a.IsCanceled == false)
                .OrderBy(a => a.AppointmentTime)
                .ToList();

            DGridAppointments.ItemsSource = null;
            DGridAppointments.ItemsSource = schedule;
        }

        private void DPickerSchedule_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSchedule();
        }

        private void BtnToday_Click(object sender, RoutedEventArgs e)
        {
            DPickerSchedule.SelectedDate = DateTime.Now.Date;
        }

        private void BtnDeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            var selected = DGridAppointments.SelectedItem as Appointments;
            if (selected == null)
            {
                MessageBox.Show("Выберите запись для удаления из списка!");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите отменить эту запись клиента?", "Отмена записи", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var appointmentInDb = Core.Context.Appointments.FirstOrDefault(a => a.ID_Appointment == selected.ID_Appointment);
                    if (appointmentInDb != null)
                    {
                        // Вместо присвоения null (которое вызывало ошибку компиляции), 
                        // выставляем встроенный в БД флаг отмены в значение true
                        appointmentInDb.IsCanceled = true;

                        Core.Context.SaveChanges();
                        MessageBox.Show("Запись успешно отменена!");
                        UpdateSchedule();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при отмене записи: " + ex.Message);
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
