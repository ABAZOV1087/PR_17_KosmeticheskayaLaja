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
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            var users = Core.Context.Users.ToList();
            DGridUsers.ItemsSource = users;

            DGridUsers.LoadingRow += (s, e) =>
            {
                if (e.Row.Item is Users user)
                {
                    var cell = DGridUsers.Columns[4].GetCellContent(e.Row) as TextBlock;
                    if (cell != null)
                    {
                        cell.Text = user.IsFrozen ? "❄️ Заморожен" : "🔥 Активен";
                    }
                }
            };
        }

        private void BtnToggleFreeze_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = DGridUsers.SelectedItem as Users;

            if (selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя из таблицы!");
                return;
            }

            if (selectedUser.ID_User == Core.CurrentUser.ID_User)
            {
                MessageBox.Show("Вы не можете заморозить самого себя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var userInDb = Core.Context.Users.FirstOrDefault(u => u.ID_User == selectedUser.ID_User);
            if (userInDb != null)
            {
                userInDb.IsFrozen = !userInDb.IsFrozen;
                Core.Context.SaveChanges();

                MessageBox.Show($"Статус пользователя {userInDb.FullName} успешно изменен!");
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
