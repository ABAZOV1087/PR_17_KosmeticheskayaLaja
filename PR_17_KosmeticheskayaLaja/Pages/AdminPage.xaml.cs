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
            RefreshData();

            // Загружаем доступные роли в выпадающий список для смены прав
            ComboRoles.ItemsSource = Core.Context.Roles.ToList();
        }

        private void RefreshData()
        {
            // Обновляем таблицу пользователей, исключая самого себя, чтобы случайно не заблокироваться
            if (Core.CurrentUser != null)
            {
                DGridUsers.ItemsSource = Core.Context.Users
                    .Where(u => u.ID_User != Core.CurrentUser.ID_User)
                    .ToList();
            }
            else
            {
                DGridUsers.ItemsSource = Core.Context.Users.ToList();
            }
        }

        private void BtnToggleFreeze_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = DGridUsers.SelectedItem as Users;
            if (selectedUser == null)
            {
                MessageBox.Show("Выберите учетную запись пользователя из таблицы!");
                return;
            }

            // Инвертируем флаг заморозки аккаунта
            selectedUser.IsFrozen = !selectedUser.IsFrozen;
            Core.Context.SaveChanges();

            MessageBox.Show($"Статус аккаунта для {selectedUser.FullName} изменен на: {(selectedUser.IsFrozen ? "ЗАБЛОКИРОВАН" : "АКТИВЕН (РАЗМОРОЖЕН)")}");
            RefreshData();
        }

        private void BtnChangeRole_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = DGridUsers.SelectedItem as Users;
            var selectedRole = ComboRoles.SelectedItem as Roles;

            if (selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя для изменения роли!");
                return;
            }

            if (selectedRole == null)
            {
                MessageBox.Show("Выберите новую роль из выпадающего списка!");
                return;
            }

            // Меняем внешний ключ роли на ID выбранной роли
            selectedUser.FID_Role = selectedRole.ID_Role;
            Core.Context.SaveChanges();

            MessageBox.Show($"Пользователю {selectedUser.FullName} успешно присвоена роль: {selectedRole.RoleName}");
            RefreshData();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Core.CurrentUser = null;
            NavigationService.Navigate(new LoginPage());
        }
    }
}
