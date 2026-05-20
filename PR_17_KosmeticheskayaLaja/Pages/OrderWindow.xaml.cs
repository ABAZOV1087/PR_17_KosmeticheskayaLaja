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
using System.Windows.Shapes;

namespace PR_17_KosmeticheskayaLaja.Pages
{
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();
            DPickerDelivery.SelectedDate = DateTime.Now.AddDays(1);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (DPickerDelivery.SelectedDate == null || ComboPayment.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            DateTime selectedDate = DPickerDelivery.SelectedDate.Value;
            DateTime maxDate = DateTime.Now.AddDays(7);

            if (selectedDate.Date < DateTime.Now.Date || selectedDate.Date > maxDate.Date)
            {
                MessageBox.Show("Получить товар можно не ранее сегодняшнего дня и не позднее 7 дней от даты заказа!");
                return;
            }

            try
            {
                Orders newOrder = new Orders
                {
                    FID_Client = Core.CurrentUser.ID_User,
                    OrderDate = DateTime.Now,
                    DeliveryDate = selectedDate,
                    PaymentMethod = (ComboPayment.SelectedItem as System.Windows.Controls.ComboBoxItem).Content.ToString(),
                    IsClosed = false
                };

                Core.Context.Orders.Add(newOrder);
                Core.Context.SaveChanges();

                var groupedCart = Core.Cart.GroupBy(p => p.ID_Product);
                foreach (var group in groupedCart)
                {
                    OrderDetails details = new OrderDetails
                    {
                        FID_Order = newOrder.ID_Order,
                        FID_Product = group.Key,
                        Quantity = group.Count()
                    };
                    Core.Context.OrderDetails.Add(details);
                }

                Core.Context.SaveChanges();
                Core.Cart.Clear();

                MessageBox.Show("Заказ успешно оформлен!");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при оформлении: " + ex.Message);
            }
        }
    }
}
