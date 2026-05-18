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
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();

        }
    private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var user = Core.Context.Users.FirstOrDefault(u => u.Login == TBoxLogin.Text && u.Password == PBoxPassword.Password);

            if (user != null)
            {
                if (user.IsFrozen)
                {
                    MessageBox.Show("Ваш аккаунт заморожен!");
                    return;
                }

                // Сохраняем пользователя в Core для доступа с других страниц
                // (Добавь public static Users CurrentUser в класс Core)
                // Core.CurrentUser = user; 

                switch (user.FID_Role)
                {
                    case 1: NavigationService.Navigate(new ClientPage()); break;
                    case 2: NavigationService.Navigate(new MasterPage()); break;
                    case 3: NavigationService.Navigate(new ManagerPage()); break;
                    case 4: NavigationService.Navigate(new AdminPage()); break;
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
