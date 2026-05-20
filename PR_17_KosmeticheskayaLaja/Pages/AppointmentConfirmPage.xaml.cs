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
    public partial class AppointmentConfirmPage : Page
    {
        private ServiceTypes _service;
        private Users _master;
        private Appointments _availableAppointment;

        public AppointmentConfirmPage(ServiceTypes service, Users master, Appointments appointment)
        {
            InitializeComponent();
            _service = service;
            _master = master;
            _availableAppointment = appointment;

            TxtService.Text = $"Тип услуги: {service.Title} (ноготочки и тд)";
            TxtMaster.Text = $"Мастер: {master.FullName}";
            TxtDateTime.Text = $"Дата и время: {appointment.AppointmentDate.ToShortDateString()} в {appointment.AppointmentTime}";
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null)
            {
                MessageBox.Show("Вы должны авторизоваться, чтобы записаться!");
                return;
            }

            if (ComboPayment.SelectedItem == null)
            {
                MessageBox.Show("Выберите способ оплаты!");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите записаться на выбранное время?", "Подтверждение записи", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var dbAppointment = Core.Context.Appointments.FirstOrDefault(a => a.ID_Appointment == _availableAppointment.ID_Appointment);
                    if (dbAppointment != null)
                    {
                        dbAppointment.FID_Client = Core.CurrentUser.ID_User;
                        dbAppointment.PaymentMethod = (ComboPayment.SelectedItem as ComboBoxItem).Content.ToString();
                        dbAppointment.Comment = TBoxComment.Text;

                        Core.Context.SaveChanges();
                        MessageBox.Show("Вы успешно записаны!");
                        NavigationService.Navigate(new StartPage());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения: " + ex.Message);
                }
            }
        }
    }
}
