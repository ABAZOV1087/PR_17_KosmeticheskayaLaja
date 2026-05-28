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
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            var orders = Core.Context.Orders.ToList();

            DGridOrders.ItemsSource = orders;

            DGridOrders.LoadingRow += (s, e) =>
            {
                if (e.Row.Item is Orders order)
                {
                    var cell = DGridOrders.Columns[5].GetCellContent(e.Row) as TextBlock;
                    if (cell != null)
                    {
                        cell.Text = order.IsClosed ? "📦 Выдан (Закрыт)" : "⏳ В обработке";
                    }
                }
            };
        }

        private void BtnCloseOrder_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = DGridOrders.SelectedItem as Orders;

            if (selectedOrder == null)
            {
                MessageBox.Show("Выберите заказ для выдачи!");
                return;
            }

            if (selectedOrder.IsClosed)
            {
                MessageBox.Show("Этот заказ уже выдан.");
                return;
            }

            var orderInDb = Core.Context.Orders.FirstOrDefault(o => o.ID_Order == selectedOrder.ID_Order);
            if (orderInDb != null)
            {
                orderInDb.IsClosed = true;
                Core.Context.SaveChanges();
                MessageBox.Show("Заказ успешно переведен в статус 'Выдан'!");
                UpdateGrid();
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Core.CurrentUser = null;
            NavigationService.Navigate(new LoginPage());
        }
    }
}