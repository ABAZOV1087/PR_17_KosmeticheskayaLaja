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
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
            UpdateCart();
        }

        private void UpdateCart()
        {
            var distinctProducts = Core.Cart.Distinct().ToList();
            LBoxCart.ItemsSource = null;
            LBoxCart.ItemsSource = distinctProducts;

            decimal total = Core.Cart.Sum(p => p.Price);
            TextTotal.Text = $"Итого: {total} руб.";
        }

        private void BtnIncrease_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).Tag);
            var product = Core.Context.Products.FirstOrDefault(p => p.ID_Product == id);
            if (product != null)
            {
                Core.Cart.Add(product);
                UpdateCart();
            }
        }

        private void BtnDecrease_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).Tag);
            var product = Core.Cart.FirstOrDefault(p => p.ID_Product == id);
            if (product != null)
            {
                Core.Cart.Remove(product);
                UpdateCart();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).Tag);
            Core.Cart.RemoveAll(p => p.ID_Product == id);
            UpdateCart();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (Core.Cart.Count == 0)
            {
                MessageBox.Show("Корзина пуста!");
                return;
            }

            OrderWindow orderWin = new OrderWindow();
            orderWin.Owner = Window.GetWindow(this);
            if (orderWin.ShowDialog() == true)
            {
                UpdateCart();
            }
        }
    }
}
