using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR_17_KosmeticheskayaLaja.Pages
{
    public partial class ManagerPage : Page
    {
        public ManagerPage()
        {
            InitializeComponent();
            RefreshAllData();
        }

        private void RefreshAllData()
        {
            DGridOrders.ItemsSource = Core.Context.Orders.ToList();
            DGridProducts.ItemsSource = Core.Context.Products.ToList();
            DGridAppointments.ItemsSource = Core.Context.Appointments.ToList();
        }

        private void ManagerTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Автоматически обновляем списки при переключении вкладок
            if (e.Source is TabControl)
            {
                RefreshAllData();
            }
        }

        private void BtnCloseOrder_Click(object sender, RoutedEventArgs e)
        {
            var selected = DGridOrders.SelectedItem as Orders;
            if (selected == null)
            {
                MessageBox.Show("Выберите заказ для выдачи!");
                return;
            }

            selected.IsClosed = true;
            Core.Context.SaveChanges();
            MessageBox.Show("Заказ успешно выдан и закрыт!");
            RefreshAllData();
        }

        private void BtnFreezeProduct_Click(object sender, RoutedEventArgs e)
        {
            var selected = DGridProducts.SelectedItem as Products;
            if (selected == null)
            {
                MessageBox.Show("Выберите товар для изменения статуса заморозки!");
                return;
            }

            // Меняем статус на противоположный (инверсия логического значения)
            selected.IsFrozen = !selected.IsFrozen;
            Core.Context.SaveChanges();
            MessageBox.Show($"Статус товара изменен. Теперь он: {(selected.IsFrozen ? "Заморожен" : "Доступен к продаже")}");
            RefreshAllData();
        }

        private void BtnCancelAppointment_Click(object sender, RoutedEventArgs e)
        {
            var selected = DGridAppointments.SelectedItem as Appointments;
            if (selected == null)
            {
                MessageBox.Show("Выберите запись для отмены!");
                return;
            }

            selected.IsCanceled = true;
            Core.Context.SaveChanges();
            MessageBox.Show("Запись на сеанс отменена менеджером!");
            RefreshAllData();
        }

        private void BtnReschedule_Click(object sender, RoutedEventArgs e)
        {
            var selected = DGridAppointments.SelectedItem as Appointments;
            if (selected == null)
            {
                MessageBox.Show("Выберите запись для переноса!");
                return;
            }

            // Простая имитация переноса: сдвигаем дату на 1 день вперед для демонстрации работоспособности функции
            selected.AppointmentDate = selected.AppointmentDate.AddDays(1);
            Core.Context.SaveChanges();
            MessageBox.Show($"Запись успешно перенесена на следующий день: {selected.AppointmentDate.ToShortDateString()}");
            RefreshAllData();
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            // Логика открытия окна добавления нового товара
        }

        private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            // Логика открытия окна изменения существующего товара
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Core.CurrentUser = null;
            NavigationService.Navigate(new LoginPage());
        }
    }
}