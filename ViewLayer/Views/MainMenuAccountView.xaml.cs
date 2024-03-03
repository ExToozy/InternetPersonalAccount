using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ViewLayer.Models;
using ViewLayer.Repositories;
using ViewLayer.Services;

namespace ViewLayer.Views
{
    /// <summary>
    /// Логика взаимодействия для MainMenuAccountView.xaml
    /// </summary>
    public partial class MainMenuAccountView : Window, INotifyPropertyChanged
    {
        #region Initial User 
        private User _user;

        public User UserAccount
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ConnectedUserTariff
        private Tariff connectedUserTariff;
        public Tariff ConnectedUserTariff
        {
            get => connectedUserTariff;
            set
            {
                if (value == null)
                {
                    connectedUserTariff = new Tariff() { Name = "Без тарифа" };
                }
                else
                {
                    connectedUserTariff = value;
                }
                OnPropertyChanged();
            }
        }
        private Tariff GetUserTariff()
        {
            int userTariffId = _personalAccountRepository.GetByUserId(UserAccount.Id).TariffsId;
            Tariff userTariff = _tariffRepository.Get(userTariffId);
            return userTariff;
        }
        #endregion

        #region AdminPanelVisibility
        private string adminPanelVisibility;
        public string AdminPanelVisibility
        {
            get { return adminPanelVisibility; }
            set
            {
                adminPanelVisibility = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Initial Repositories
        private TariffRepository _tariffRepository = new TariffRepository();
        private TrafficRepository _trafficRepository = new TrafficRepository();
        private PaymentRepository _paymentRepository = new PaymentRepository();
        private UserRepository _userRepository = new UserRepository();
        private PersonalAccountRepository _personalAccountRepository = new PersonalAccountRepository();
        private DBRepository _dbRepository = new DBRepository();
        #endregion

        #region Creating ObservableCollection 

        public ObservableCollection<Tariff> Tariffs { get; set; } = new ObservableCollection<Tariff>();
        public ObservableCollection<Traffic> Traffic { get; set; } = new ObservableCollection<Traffic>();
        public ObservableCollection<Payment> Payments { get; set; } = new ObservableCollection<Payment>();
        public ObservableCollection<string> TableNames { get; set; } = new ObservableCollection<string>();

        public void UpdateAllObservableCollection()
        {
            Payments.Clear();
            Traffic.Clear();
            Tariffs.Clear();
            TableNames.Clear();
            foreach (var item in _paymentRepository.GetAllByUserId(UserAccount.Id).Reverse())
            {
                Payments.Add(item);
            }
            foreach (var item in _trafficRepository.GetAllByUserId(UserAccount.Id).Reverse())
            {
                Traffic.Add(item);
            }
            foreach (var item in _tariffRepository.GetAll())
            {
                Tariffs.Add(item);
            }
            foreach (var item in _dbRepository.GetAllTableName())
            {
                TableNames.Add(item);
            }

        }
        #endregion

        #region PropertyChangedEvent
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region Constructor
        public MainMenuAccountView(User user)
        {
            InitializeComponent();
            CountTableFiedlsSelectedItem = OneFieldItem;
            DataContext = this;
            UserAccount = user;
            UserAccount.Email = _userRepository.GetEmailAdress(UserAccount.Id);
            EmailTextBox.Text = UserAccount.Email;
            ConnectedUserTariff = GetUserTariff();
            if (UserAccount.Email == "")
            {
                MailingRadioBtn.IsEnabled = false;
            }
            else
            {
                if (UserAccount.MailingIsOn == 1)
                {
                    MailingRadioBtn.IsChecked = true;
                    MailingRadioBtn.Content = "Рассылка включена";
                }
            }
            if (UserAccount.IsAdmin == 1)
            {
                AdminPanelVisibility = "Visible";
            }
            else
            {
                AdminPanelVisibility = "Hidden";
            }
            UpdateAllObservableCollection();
        }
        #endregion

        #region Window Actions
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
        #endregion

        #region UserPanel

        #region Connect Or Change User Email
        private void ConnectOrChangeEmailAdressBtn_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"^([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\.[a-zA-Z]+)$");
            EmailTextBox.Text = EmailTextBox.Text.Trim();
            if (regex.IsMatch(EmailTextBox.Text))
            {
                _userRepository.ConnectOrChangeEmailAdress(UserAccount.Id, EmailTextBox.Text);
                UserAccount.Email = EmailTextBox.Text;
                MailingRadioBtn.IsEnabled = true;
                MailingRadioBtn.IsChecked = true;
                MailingRadioBtn.Content = "Рассылка включена";
                _userRepository.ChangeMailingStatus(UserAccount.Id, 1);
                UserAccount.MailingIsOn = 1;
            }
            else if (String.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                if (UserAccount.Email != "")
                {
                    _userRepository.ConnectOrChangeEmailAdress(UserAccount.Id, EmailTextBox.Text);
                    UserAccount.Email = EmailTextBox.Text;
                    MailingRadioBtn.IsEnabled = false;
                    MailingRadioBtn.IsChecked = false;
                    MailingRadioBtn.Content = "Рассылка выключена";
                    _userRepository.ChangeMailingStatus(UserAccount.Id, 0);
                    UserAccount.MailingIsOn = 0;
                    
                }
                else
                {
                    MessageBox.Show("Введите почту");
                }
            }
            else
            {
                MessageBox.Show("Почта введена некорректно");
            }
           
        }


        #endregion

        #region Activate or Disactivate Mailing
        private void MailingRadioBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MailingRadioBtn.IsChecked == false)
            {
                MailingRadioBtn.Content = "Рассылка выключена";
                _userRepository.ChangeMailingStatus(UserAccount.Id, 0);
                UserAccount.MailingIsOn = 0;
            }
            if (MailingRadioBtn.IsChecked == true)
            {
                MailingRadioBtn.Content = "Рассылка включена";
                _userRepository.ChangeMailingStatus(UserAccount.Id, 1);
                UserAccount.MailingIsOn = 1;
            }
        }
        #endregion

        #region  Refill Balance Value
        public int RefillBalanceValueFromTextbox { get; set; }
        private void RefillBtn_Click(object sender, RoutedEventArgs e)
        {
            if (RefillBalanceValueFromTextbox != 0)
            {
                try
                {
                    _paymentRepository.Create(new Payment(UserAccount.Id, DateTime.Now.ToShortDateString(), "Пополнение", RefillBalanceValueFromTextbox));
                    _userRepository.RefillBalance(UserAccount.Id, RefillBalanceValueFromTextbox);
                    UserAccount.Balance += RefillBalanceValueFromTextbox;
                    Payments.Insert(0, new Payment(UserAccount.Id, DateTime.Now.ToShortDateString(), "Пополнение", RefillBalanceValueFromTextbox));
                }
                catch (Exception)
                {
                    MessageBox.Show("Что-то пошло не так, попробуйте позже!");
                }
            }
            else
            {
                MessageBox.Show("Введите сумму!");
            }


        }
        #endregion

        #region Connect Tariff To User
        public Tariff SelectedTariff { get; set; }
        private void ConnectSelectedTariffBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UserAccount.Balance - SelectedTariff.Price >= 0)
            {
                try
                {
                    _personalAccountRepository.ConnectTariffToUser(SelectedTariff, UserAccount);
                    Payments.Insert(0, new Payment(UserAccount.Id, DateTime.Now.ToShortDateString(), "Снятие", SelectedTariff.Price));
                    ConnectedUserTariff = SelectedTariff;
                    UserAccount.Balance -= SelectedTariff.Price;
                    MessageBox.Show("Тариф успешно подлючён!");
                    if (UserAccount.MailingIsOn == 1)
                    {
                        SMTPService smtpService = new SMTPService();
                        smtpService.SendMessageToEmail(
                            UserAccount.Email, 
                            "Тариф успешно подлючён", "Здравствуйте", 
                            $"{UserAccount.FullName}, Вы успешно подключили тариф '{ConnectedUserTariff.Name}'. ",
                            $"За него было списано {ConnectedUserTariff.Price} и теперь вам доступен интернет со скоростью {ConnectedUserTariff.ConnectionSpeed} Мбит/сек. Тариф будет доступен до {DateTime.Now.AddDays(30).ToShortDateString()}");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Что - то пошло не так, попробуйте позже!");
                }
            }
            else
            {
                MessageBox.Show("На счёте недостаточно средств!");
            }
        }
        #endregion

        #region Filter Date
        private void FilterPaymentsByDateBtn_Click(object sender, RoutedEventArgs e)
        {

            Payments.Clear();
            foreach (var item in _paymentRepository.FilteredGetAllByUserId(UserAccount.Id, FitlerPaymentTextBox.Text).Reverse())
            {
                Payments.Add(item);
            }
        }

        private void ResetFilterPaymentsBtn_Click(object sender, RoutedEventArgs e)
        {
            FitlerPaymentTextBox.Text = "";
            Payments.Clear();
            foreach (var item in _paymentRepository.GetAllByUserId(UserAccount.Id).Reverse())
            {
                Payments.Add(item);
            }
        }
        #endregion

        #region Get Sum Of Use Time Duration
        private void GetUseTimeDurationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UseTimeDurationTextBox.IsEnabled && UseTimeDurationTextBox2.IsEnabled)
            {
                MessageBox.Show("Введите ip или дату");
                return;
            }
            if (UseTimeDurationTextBox.IsEnabled)
            {
                Regex regex = new Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");

                UseTimeDurationTextBox.Text = UseTimeDurationTextBox.Text.Trim();
                if (regex.IsMatch(UseTimeDurationTextBox.Text))
                {
                    int UseTimeDurationSum = _trafficRepository.GetUseTimeDurationSum(UseTimeDurationTextBox.Text, UserAccount.Id);
                    var ts = TimeSpan.FromMinutes(UseTimeDurationSum);
                    UseTimeDurationTextBlock.Text = $"{ts.Hours} часов {ts.Minutes} минут";
                }
                else
                {
                    MessageBox.Show("Введите корректный ip адрес");
                }
            }
            if (UseTimeDurationTextBox2.IsEnabled)
            {
                Regex regex = new Regex(@"^[0-9.]{1,10}$");

                UseTimeDurationTextBox2.Text = UseTimeDurationTextBox2.Text.Trim();
                if (regex.IsMatch(UseTimeDurationTextBox2.Text))
                {
                    (string, Dictionary<string, string>) filteredData = _trafficRepository.GetFilteredListByDate(UserAccount.Id, UseTimeDurationTextBox2.Text);
                    if (filteredData.Item2.Count > 0)
                    {
                        filteredData = _trafficRepository.GetFilteredListByDate(UserAccount.Id, UseTimeDurationTextBox2.Text);
                        var ts = TimeSpan.FromMinutes(Convert.ToInt32(filteredData.Item1));
                        UseTimeDurationTextBlock.Text = $"{ts.Hours} часов {ts.Minutes} минут";
                        FilteredTrafficListView.ItemsSource = filteredData.Item2;
                    }
                    else
                    {
                        UseTimeDurationTextBlock.Text = "Нет результатов";
                        FilteredTrafficListView.ItemsSource = null;
                    }
                }
                else
                {
                    MessageBox.Show("Введите корректную дату");
                }
            }

        }
        private void UseTimeDurationTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (UseTimeDurationTextBox.Text != "")
            {
                UseTimeDurationTextBox2.IsEnabled = false;
            }
            if (UseTimeDurationTextBox.Text == "")
            {
                UseTimeDurationTextBox2.IsEnabled = true;
            }
        }

        private void UseTimeDurationTextBox2_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (UseTimeDurationTextBox2.Text != "")
            {
                UseTimeDurationTextBox.IsEnabled = false;
            }
            if (UseTimeDurationTextBox2.Text == "")
            {
                UseTimeDurationTextBox.IsEnabled = true;
            }
        }
        #endregion

        #endregion

        #region AdminPanel

        #region CreateTable
        private Object _countTableFiedlsSelectedItem;
        public Object CountTableFiedlsSelectedItem
        {
            get { return _countTableFiedlsSelectedItem; }
            set
            {
                if (value == null)
                {
                    FieldThreeRadioBtn.IsEnabled = false;
                    FieldThreeNullableComboBox.IsEnabled = false;
                    FieldThreeTextBox.IsEnabled = false;
                    FieldThreeTypeComboBox.IsEnabled = false;

                    FieldFourRadioBtn.IsEnabled = false;
                    FieldFourNullableComboBox.IsEnabled = false;
                    FieldFourTextBox.IsEnabled = false;
                    FieldFourTypeComboBox.IsEnabled = false;

                    BindingTableComboBox.IsEnabled = false;

                    FieldOneRadioBtn.IsChecked = false;
                    FieldOneNullableComboBox.SelectedItem = null;
                    FieldOneTextBox.Clear();
                    FieldOneTypeComboBox.SelectedItem = null;

                    FieldTwoRadioBtn.IsChecked = false;
                    FieldTwoNullableComboBox.SelectedItem = null;
                    FieldTwoTextBox.Clear();
                    FieldTwoTypeComboBox.SelectedItem = null;

                    FieldThreeRadioBtn.IsEnabled = false;
                    FieldThreeNullableComboBox.SelectedItem = null;
                    FieldThreeTextBox.Clear();
                    FieldThreeTypeComboBox.SelectedItem = null;

                    FieldFourRadioBtn.IsEnabled = false;
                    FieldFourNullableComboBox.SelectedItem = null;
                    FieldFourTextBox.Clear();
                    FieldFourTypeComboBox.SelectedItem = null;

                    BindingTableComboBox.SelectedItem = null;
                    CountTableFiedlsSelectedItem = OneFieldItem;
                    TableNameTextBox.Clear();

                    OnPropertyChanged();
                    return;

                }
                if (value.ToString().Split(' ')[1] == "2")
                {
                    FieldOneRadioBtn.IsEnabled = true;
                    FieldOneNullableComboBox.IsEnabled = true;
                    FieldOneTextBox.IsEnabled = true;
                    FieldOneTypeComboBox.IsEnabled = true;

                    FieldTwoRadioBtn.IsEnabled = true;
                    FieldTwoNullableComboBox.IsEnabled = true;
                    FieldTwoTextBox.IsEnabled = true;
                    FieldTwoTypeComboBox.IsEnabled = true;

                    FieldThreeRadioBtn.IsEnabled = false;
                    FieldThreeNullableComboBox.IsEnabled = false;
                    FieldThreeTextBox.IsEnabled = false;
                    FieldThreeTypeComboBox.IsEnabled = false;

                    FieldFourRadioBtn.IsEnabled = false;
                    FieldFourNullableComboBox.IsEnabled = false;
                    FieldFourTextBox.IsEnabled = false;
                    FieldFourTypeComboBox.IsEnabled = false;

                    BindingTableComboBox.IsEnabled = true;

                    _countTableFiedlsSelectedItem = value;
                }
                if (value.ToString().Split(' ')[1] == "3")
                {
                    FieldOneRadioBtn.IsEnabled = true;
                    FieldOneNullableComboBox.IsEnabled = true;
                    FieldOneTextBox.IsEnabled = true;
                    FieldOneTypeComboBox.IsEnabled = true;

                    FieldTwoRadioBtn.IsEnabled = true;
                    FieldTwoNullableComboBox.IsEnabled = true;
                    FieldTwoTextBox.IsEnabled = true;
                    FieldTwoTypeComboBox.IsEnabled = true;

                    FieldThreeRadioBtn.IsEnabled = true;
                    FieldThreeNullableComboBox.IsEnabled = true;
                    FieldThreeTextBox.IsEnabled = true;
                    FieldThreeTypeComboBox.IsEnabled = true;

                    FieldFourRadioBtn.IsEnabled = false;
                    FieldFourNullableComboBox.IsEnabled = false;
                    FieldFourTextBox.IsEnabled = false;
                    FieldFourTypeComboBox.IsEnabled = false;

                    BindingTableComboBox.IsEnabled = true;

                    _countTableFiedlsSelectedItem = value;
                }
                if (value.ToString().Split(' ')[1] == "4")
                {
                    FieldOneRadioBtn.IsEnabled = true;
                    FieldOneNullableComboBox.IsEnabled = true;
                    FieldOneTextBox.IsEnabled = true;
                    FieldOneTypeComboBox.IsEnabled = true;

                    FieldTwoRadioBtn.IsEnabled = true;
                    FieldTwoNullableComboBox.IsEnabled = true;
                    FieldTwoTextBox.IsEnabled = true;
                    FieldTwoTypeComboBox.IsEnabled = true;

                    FieldThreeRadioBtn.IsEnabled = true;
                    FieldThreeNullableComboBox.IsEnabled = true;
                    FieldThreeTextBox.IsEnabled = true;
                    FieldThreeTypeComboBox.IsEnabled = true;

                    FieldFourRadioBtn.IsEnabled = true;
                    FieldFourNullableComboBox.IsEnabled = true;
                    FieldFourTextBox.IsEnabled = true;
                    FieldFourTypeComboBox.IsEnabled = true;

                    BindingTableComboBox.IsEnabled = true;

                    _countTableFiedlsSelectedItem = value;
                }
                OnPropertyChanged();
            }
        }

        private void CreateTableBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CountTableFiedlsSelectedItem.ToString().Split(' ')[1] == "2")
            {
                if (!TwoRowsParamsIsEmpty())
                {
                    _dbRepository.CreateTable(2,
                    $"{TableNameTextBox.Text}",
                    $"{FieldOneTextBox.Text}",
                    $"{FieldTwoTextBox.Text}",
                    "",
                    "",
                    GetFieldOneParams(),
                    GetFieldTwoParams(),
                    new List<string> { },
                    new List<string> { },
                    BindingTableComboBox.SelectedItem.ToString());

                    MessageBox.Show($"Таблица {TableNameTextBox.Text} успешно создана и привязана к таблице {BindingTableComboBox.Text}");
                    TableNames.Insert(0, TableNameTextBox.Text);
                    CountTableFiedlsSelectedItem = null;
                }
                else
                {
                    MessageBox.Show("Заполните все поля!");
                }

            }
            if (CountTableFiedlsSelectedItem.ToString().Split(' ')[1] == "3")
            {
                if (!ThreeRowsParamsIsEmpty())
                {
                    _dbRepository.CreateTable(3,
                    $"{TableNameTextBox.Text}",
                    $"{FieldOneTextBox.Text}",
                    $"{FieldTwoTextBox.Text}",
                    $"{FieldThreeTextBox.Text}",
                    "",
                    GetFieldOneParams(),
                    GetFieldTwoParams(),
                    GetFieldThreeParams(),
                    new List<string> { },
                    BindingTableComboBox.SelectedItem.ToString());

                    MessageBox.Show($"Таблица {TableNameTextBox.Text} успешно создана и привязана к таблице {BindingTableComboBox.Text}");
                    TableNames.Insert(0, TableNameTextBox.Text);
                    CountTableFiedlsSelectedItem = null;

                }
                else
                {
                    MessageBox.Show("Заполните все поля!");
                }
            }
            if (CountTableFiedlsSelectedItem.ToString().Split(' ')[1] == "4")
            {
                if (!FourRowsParamsIsEmpty())
                {
                    _dbRepository.CreateTable(4,
                    $"{TableNameTextBox.Text}",
                    $"{FieldOneTextBox.Text}",
                    $"{FieldTwoTextBox.Text}",
                    $"{FieldThreeTextBox.Text}",
                    $"{FieldFourTextBox.Text}",
                    GetFieldOneParams(),
                    GetFieldTwoParams(),
                    GetFieldThreeParams(),
                    GetFieldFourParams(),
                    BindingTableComboBox.SelectedItem.ToString());

                    MessageBox.Show($"Таблица {TableNameTextBox.Text} успешно создана и привязана к таблице {BindingTableComboBox.Text}");
                    TableNames.Insert(0, TableNameTextBox.Text);
                    CountTableFiedlsSelectedItem = null;
                }
                else
                {
                    MessageBox.Show("Заполните все поля!");
                }
            }
        }

        private List<string> GetFieldOneParams()
        {
            List<string> fieldOneParams = new List<string>() { };
            if (FieldOneTypeComboBox.SelectedIndex == 0)
            {
                fieldOneParams.Add("TEXT(255)");
            }
            else
            {
                fieldOneParams.Add("INT");
            }
            if (FieldOneNullableComboBox.SelectedIndex == 0)
            {
                fieldOneParams.Add("NULL");
            }
            else
            {
                fieldOneParams.Add("NOT NULL");
            }
            if (FieldOneRadioBtn.IsChecked == true)
            {
                fieldOneParams.Add("PRIMARY KEY");
            }
            return fieldOneParams;
        }
        private List<string> GetFieldTwoParams()
        {
            List<string> fieldTwoParams = new List<string>() { };
            if (FieldTwoTypeComboBox.SelectedIndex == 0)
            {
                fieldTwoParams.Add("TEXT(255)");
            }
            else
            {
                fieldTwoParams.Add("INT");
            }
            if (FieldTwoNullableComboBox.SelectedIndex == 0)
            {
                fieldTwoParams.Add("NULL");
            }
            else
            {
                fieldTwoParams.Add("NOT NULL");
            }
            if (FieldTwoRadioBtn.IsChecked == true)
            {
                fieldTwoParams.Add("PRIMARY KEY");
            }
            return fieldTwoParams;
        }
        private List<string> GetFieldThreeParams()
        {
            List<string> fieldThreeParams = new List<string>() { };
            if (FieldThreeTypeComboBox.SelectedIndex == 0)
            {
                fieldThreeParams.Add("TEXT(255)");
            }
            else
            {
                fieldThreeParams.Add("INT");
            }
            if (FieldThreeNullableComboBox.SelectedIndex == 0)
            {
                fieldThreeParams.Add("NULL");
            }
            else
            {
                fieldThreeParams.Add("NOT NULL");
            }
            if (FieldThreeRadioBtn.IsChecked == true)
            {
                fieldThreeParams.Add("PRIMARY KEY");
            }
            return fieldThreeParams;
        }
        private List<string> GetFieldFourParams()
        {
            List<string> fieldFourParams = new List<string>() { };
            if (FieldFourTypeComboBox.SelectedIndex == 0)
            {
                fieldFourParams.Add("TEXT(255)");
            }
            else
            {
                fieldFourParams.Add("INT");
            }
            if (FieldFourNullableComboBox.SelectedIndex == 0)
            {
                fieldFourParams.Add("NULL");
            }
            else
            {
                fieldFourParams.Add("NOT NULL");
            }
            if (FieldThreeRadioBtn.IsChecked == true)
            {
                fieldFourParams.Add("PRIMARY KEY");
            }
            return fieldFourParams;
        }
        private bool TwoRowsParamsIsEmpty()
        {
            if (String.IsNullOrWhiteSpace(TableNameTextBox.Text) ||
                String.IsNullOrWhiteSpace(FieldOneTextBox.Text) ||
                FieldOneTypeComboBox.SelectedItem == null ||
                FieldOneNullableComboBox.SelectedItem == null ||
                String.IsNullOrWhiteSpace(FieldTwoTextBox.Text) ||
                FieldTwoTypeComboBox.SelectedItem == null ||
                FieldTwoNullableComboBox.SelectedItem == null)
            {
                return true;
            }
            else { return false; }
        }
        private bool ThreeRowsParamsIsEmpty()
        {
            if (TwoRowsParamsIsEmpty() ||
                String.IsNullOrWhiteSpace(FieldThreeTextBox.Text) ||
                FieldThreeTypeComboBox.SelectedItem == null ||
                FieldThreeNullableComboBox.SelectedItem == null)
            {
                return true;
            }
            else { return false; }
        }
        private bool FourRowsParamsIsEmpty()
        {
            if (ThreeRowsParamsIsEmpty() ||
                String.IsNullOrWhiteSpace(FieldTwoTextBox.Text) ||
                FieldTwoTypeComboBox.SelectedItem == null ||
                FieldTwoNullableComboBox.SelectedItem == null)
            {
                return true;
            }
            else { return false; }
        }

        #endregion

        #region GetAllRecordsByTableName

        private Object _selectedTableNameInDeleteRecordTabItem;
        public Object SelectedTableNameInDeleteRecordTabItem
        {
            get { return _selectedTableNameInDeleteRecordTabItem; }
            set
            {
                if (value != null)
                {
                    _selectedTableNameInDeleteRecordTabItem = value;
                    DataGridForDeleteRecords.ItemsSource = _dbRepository.GetAllRecordsByTableName(value.ToString());
                    OnPropertyChanged();
                    return;
                }
                DataGridForDeleteRecords.ItemsSource = null;
                _selectedTableNameInDeleteRecordTabItem = value;
                OnPropertyChanged();
            }
        }

        private Object _selectedTableNameInEditRecordTabItem;
        public Object SelectedTableNameInEditRecordTabItem
        {
            get { return _selectedTableNameInEditRecordTabItem; }
            set
            {
                if (value != null)
                {
                    _selectedTableNameInEditRecordTabItem = value;
                    DataGridForEditRecords.ItemsSource = _dbRepository.GetAllRecordsByTableName(value.ToString());
                    OnPropertyChanged();
                    return;
                }
                DataGridForEditRecords.ItemsSource = null;
                _selectedTableNameInEditRecordTabItem = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region DeleteTable
        private Object _selectedTableNameInDeleteTableTabItem;
        public Object SelectedTableNameInDeleteTableTabItem
        {
            get { return _selectedTableNameInDeleteTableTabItem; }
            set
            {
                _selectedTableNameInDeleteTableTabItem = value;
                OnPropertyChanged();
            }
        }

        private void DeleteTableBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTableNameInDeleteTableTabItem != null)
            {
                if (MessageBox.Show($"Вы уверены что хотите удалить таблицу {SelectedTableNameInDeleteTableTabItem}? " +
                $"Так же будут удалены все связи с этой таблицей и связные поля", "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _dbRepository.DeleteTable(SelectedTableNameInDeleteTableTabItem.ToString());
                    for (int i = 0; i < TableNames.Count; i++)
                    {
                        if (TableNames[i] == SelectedTableNameInDeleteTableTabItem.ToString())
                        {
                            TableNames.Remove(TableNames[i]);
                            return;
                        }
                    }
                    MessageBox.Show("Таблица была успешно удалена");
                }
                else
                {
                    MessageBox.Show("Удаление было отменено");
                }
            }
            else
            {
                MessageBox.Show("Выберите таблицу");
            }

        }

        #endregion

        #region EditRecords
        public Object SelectedItemToEdit { get; set; }
        private void EditRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItemToEdit != null && SelectedTableNameInEditRecordTabItem != null)
            {
                int recordId = Convert.ToInt32(((DataRowView)SelectedItemToEdit).Row.ItemArray[0]);
                EditDBRecordView editDBRecordView = new EditDBRecordView(this, recordId, SelectedTableNameInEditRecordTabItem.ToString(), _dbRepository);
                editDBRecordView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите таблицу или запись в таблице");
            }
        }
        #endregion

        #region DeleteRecords

        public Object SelectedItemToDelete { get; set; }

        private void DeleteRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItemToDelete != null && SelectedTableNameInDeleteRecordTabItem != null)
            {
                int recordId = Convert.ToInt32(((DataRowView)SelectedItemToDelete).Row.ItemArray[0]);
                _dbRepository.DeleteRecordFromTableNameById(recordId, SelectedTableNameInDeleteRecordTabItem.ToString());
                DataGridForDeleteRecords.ItemsSource = _dbRepository.GetAllRecordsByTableName(SelectedTableNameInDeleteRecordTabItem.ToString());
            }
            else
            {
                MessageBox.Show("Выберите таблицу или запись в таблице");
            }

        }



        #endregion

        #region AddRecords

        private Object _selectedTableNameInAddNewRecordTabItem;
        public Object SelectedTableNameInAddNewRecordTabItem
        {
            get { return _selectedTableNameInAddNewRecordTabItem; }
            set
            {
                _selectedTableNameInAddNewRecordTabItem = value;
                OnPropertyChanged();
            }
        }
        private void AddNewRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTableNameInAddNewRecordTabItem != null)
            {
                AddNewDBRecordView addNewDBRecordView = new AddNewDBRecordView(this, SelectedTableNameInAddNewRecordTabItem.ToString(), _dbRepository);
                addNewDBRecordView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите таблицу");
            }
        }





        #endregion

        #endregion

        
    }
}