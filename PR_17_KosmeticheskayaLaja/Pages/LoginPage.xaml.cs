using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR_17_KosmeticheskayaLaja.Pages
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TBoxLogin.Text.Trim();
            string password = PBoxPassword.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = Core.Context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (user.IsFrozen)
            {
                MessageBox.Show("Ваш аккаунт временно заморожен администратором!", "Доступ ограничен", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            Core.CurrentUser = user;

            switch (user.FID_Role)
            {
                case 1:
                    NavigationService.Navigate(new StartPage());
                    break;
                case 2:
                    NavigationService.Navigate(new MasterPage());
                    break;
                case 3:
                    NavigationService.Navigate(new ManagerPage());
                    break;
                case 4:
                    NavigationService.Navigate(new AdminPage());
                    break;
                default:
                    MessageBox.Show("Роль пользователя не определена в системе!", "Ошибка прав", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
    }
}