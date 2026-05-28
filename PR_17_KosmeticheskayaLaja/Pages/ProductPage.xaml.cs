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

            var manufacturers = Core.Context.Manufacturers.ToList();
            manufacturers.Insert(0, new Manufacturers { Name = "Все производители" });
            ComboFilter.ItemsSource = manufacturers;
            ComboFilter.SelectedIndex = 0;
            ComboSort.SelectedIndex = 0;

            UpdateProducts();
        }

        private void UpdateProducts()
        {
            var currentProducts = Core.Context.Products.ToList();

            if (ComboFilter.SelectedIndex > 0)
            {
                var selectedManufacturer = ComboFilter.SelectedItem as Manufacturers;
                currentProducts = currentProducts.Where(p => p.FID_Manufacturer == selectedManufacturer.ID_Manufacturer).ToList();
            }

            string searchText = TBoxSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                currentProducts = currentProducts.Where(p => p.Title.ToLower().Contains(searchText)).ToList();
            }

            if (ComboSort.SelectedIndex == 1)
            {
                currentProducts = currentProducts.OrderBy(p => p.Price).ToList();
            }
            else if (ComboSort.SelectedIndex == 2)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.Price).ToList();
            }

            LBoxProducts.ItemsSource = currentProducts;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateProducts();
        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateProducts();
        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateProducts();

        private void BtnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Products;
            if (product != null)
            {
                MessageBox.Show($"Товар \"{product.Title}\" успешно добавлен в корзину!");
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
