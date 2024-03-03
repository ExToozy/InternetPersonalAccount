using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ViewLayer.Models;
using ViewLayer.Repositories;

namespace ViewLayer.Views
{
    /// <summary>
    /// Логика взаимодействия для RegistrationView.xaml
    /// </summary>
    public partial class RegistrationView : Window
    {
        UserRepository _userRepository = new UserRepository();
        public RegistrationView()
        {
            InitializeComponent();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {

            if (!Regex.IsMatch(UserPhoneNumberTextBox.Text, @"^89\d{9}$"))
            {
                MessageBox.Show("Неверно указан номер телефона");
                return;
            }
            if (!Regex.IsMatch(UserFullNameTextBox.Text, @"^[А-Яа-я]{2,50} [А-Яа-я]{2,50} [А-Яа-я]{2,50}$"))
            {
                MessageBox.Show("Неверно указано имя");
                return;
            }
            if (!Regex.IsMatch(UserPasswordTextBox.Text, @"(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[А-Яа-я0-9a-zA-Z!@#$%^&*]{6,}$"))
            {
                MessageBox.Show("Пароль не прошел проверку, проверьте есть ли следующие пункты:\n" +
                    "1)Длина пароля более 6 символов\n" +
                    "2)Содержит хотя бы одну английскую букву\n" +
                    "3)Содержит 1 цифру\n" +
                    "4)Содержит специальный символ из следующего набора ! @ # $ % ^ & *\n" +
                    "5)Не содержит пробелов");
                return;
            }
            if (UserPasswordTextBox.Text != UserPasswordRetryTextBox.Text)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }
            if (_userRepository.CheckExist(UserPhoneNumberTextBox.Text) == null)
            {
                _userRepository.Create(new User(UserFullNameTextBox.Text, UserPhoneNumberTextBox.Text, UserPasswordTextBox.Text));
                LoginView loginView = new LoginView();
                loginView.Owner = this;
                loginView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                loginView.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Пользователь с таким номером уже существует");
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
    }
}
