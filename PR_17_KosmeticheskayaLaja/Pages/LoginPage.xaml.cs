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
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            // Ищем пользователя в базе данных
            var user = Core.Context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль!");
                return;
            }

            // Проверяем, не заморожен ли аккаунт (ТЗ Администратора)
            if (user.IsFrozen)
            {
                MessageBox.Show("Ваш аккаунт временно заблокирован администратором!", "Блокировка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Сохраняем залогиненного пользователя в глобальный контекст
            Core.CurrentUser = user;

            // Перенаправление на страницы в зависимости от роли (ID_Role)
            switch (user.FID_Role)
            {
                case 1: // Клиент
                    MessageBox.Show($"Добро пожаловать, {user.FullName}! Переходим на главную.");
                    NavigationService.Navigate(new StartPage());
                    break;

                case 2: // Мастер
                    MessageBox.Show($"Успешный вход! Рабочее место мастера: {user.FullName}");
                    NavigationService.Navigate(new MasterPage());
                    break;

                case 3: // Менеджер
                    MessageBox.Show("Вход выполнен! Открывается панель Менеджера.");
                    NavigationService.Navigate(new ManagerPage());
                    break;

                case 4: // Администратор
                    MessageBox.Show("Уровень доступа: Администратор. Загрузка панели управления.");
                    NavigationService.Navigate(new AdminPage());
                    break;

                default:
                    MessageBox.Show("Ошибка определения прав доступа. Обратитесь к администратору.");
                    break;
            }
        }

        private void BtnRegisterRedirect_Click(object sender, RoutedEventArgs e)
        {
            // Если есть страница регистрации — переходим, если нет — можно закомментировать
            // NavigationService.Navigate(new RegisterPage());
        }
    }
}