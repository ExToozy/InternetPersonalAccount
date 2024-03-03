using System.Windows;
using System.Windows.Input;
using ViewLayer.Models;
using ViewLayer.Repositories;
using ViewLayer.Services;

namespace ViewLayer.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            TrafficRepository trafficRepository = new TrafficRepository();
            trafficRepository.GetFilteredListByDate(13,"222222");

        }
        UserRepository _userRepository = new UserRepository();
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!long.TryParse(UserPhoneNumberTextBox.Text.Trim(), out long userPhoneNumber))
            {
                MessageBox.Show("Некорректно указан номер телефона");
                return;
            }
            User user = _userRepository.Get(userPhoneNumber.ToString(), UserPasswordTextBox.Text);
            if (user != null)
            {
                MainMenuAccountView mainMenuAccountView = new MainMenuAccountView(user);
                Application.Current.MainWindow.Hide();
                mainMenuAccountView.Owner = Application.Current.MainWindow;
                mainMenuAccountView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                Application.Current.MainWindow = mainMenuAccountView;
                mainMenuAccountView.Show();
            }
            else
            {
                MessageBox.Show("Такого пользователя не существует или неверные данные");
            }


        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimazeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OpenRegistrationViewBtn_Click(object sender, RoutedEventArgs e)
        {
            RegistrationView registrationView = new RegistrationView();
            registrationView.Owner = this;
            registrationView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            registrationView.Show();
            this.Hide();
        }
    }
}
