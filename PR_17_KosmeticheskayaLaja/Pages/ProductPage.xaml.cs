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
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();

            // Заполняем фильтры
            var types = Core.Context.ProductTypes.ToList();
            types.Insert(0, new ProductTypes { TypeName = "Все типы" });
            ComboType.ItemsSource = types;
            ComboType.SelectedIndex = 0;

            var manufs = Core.Context.Manufacturers.ToList();
            manufs.Insert(0, new Manufacturers { Name = "Все производители" });
            ComboManufacturer.ItemsSource = manufs;
            ComboManufacturer.SelectedIndex = 0;

            UpdateData();
        }

        private void UpdateData()
        {
            var list = Core.Context.Products.ToList();

            // Поиск
            if (!string.IsNullOrWhiteSpace(TBoxSearch.Text))
                list = list.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            // Фильтр по типу
            if (ComboType.SelectedIndex > 0)
                list = list.Where(p => p.FID_ProductType == (ComboType.SelectedItem as ProductTypes).ID_ProductType).ToList();

            // Фильтр по производителю
            if (ComboManufacturer.SelectedIndex > 0)
                list = list.Where(p => p.FID_Manufacturer == (ComboManufacturer.SelectedItem as Manufacturers).ID_Manufacturer).ToList();

            // Сортировка по рейтингу
            if (ComboSort.SelectedIndex == 1) list = list.OrderBy(p => p.Rating).ToList();
            else if (ComboSort.SelectedIndex == 2) list = list.OrderByDescending(p => p.Rating).ToList();

            LBoxProducts.ItemsSource = list;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateData();
        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateData();
        private void ComboManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateData();
        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateData();

        private void BtnBack_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();

        private void BtnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null)
            {
                MessageBox.Show("Добавление доступно только после авторизации!");
                return;
            }

            var button = sender as Button;
            if (button != null)
            {
                int productId = Convert.ToInt32(button.Tag);
                var product = Core.Context.Products.FirstOrDefault(p => p.ID_Product == productId);

                if (product != null)
                {
                    Core.Cart.Add(product);
                    MessageBox.Show($"{product.Title} добавлен в корзину!");
                }
            }
        }

        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null)
            {
                MessageBox.Show("Корзина доступна только после авторизации!");
                return;
            }
            NavigationService.Navigate(new CartPage());
        }
    }
}
