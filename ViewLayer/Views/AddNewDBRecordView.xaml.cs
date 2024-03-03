using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewLayer.Repositories;

namespace ViewLayer.Views
{
    /// <summary>
    /// Логика взаимодействия для AddNewDBRecordView.xaml
    /// </summary>
    public partial class AddNewDBRecordView : Window
    {
        private MainMenuAccountView _mainMenuAccountView;
        private string _tableName;
        private DBRepository _dbRepository;
        private List<StackPanel> FieldsStackPanel = new List<StackPanel>();
        private List<TextBlock> FieldsTextBlock = new List<TextBlock>();
        private List<TextBox> FieldsTextBox = new List<TextBox>();
        private DataTable _record;
        public AddNewDBRecordView(MainMenuAccountView mainMenuAccountView, string tableName, DBRepository dbRepository)
        {
            InitializeComponent();
            _mainMenuAccountView = mainMenuAccountView;
            _tableName = tableName;
            _dbRepository = dbRepository;
            FieldsStackPanel.AddRange(new List<StackPanel>{
                FirstFieldStackPanel,
                SecondFieldStackPanel,
                ThirdFieldStackPanel,
                FourthColumnStackPanel,
                FifthColumnStackPanel,
                SixthColumnStackPanel,
                SeventhColumnStackPanel,
                EighthColumnStackPanel,
                NinthColumnStackPanel});
            FieldsTextBlock.AddRange(new List<TextBlock>{
                FirstFieldTextBlock,
                SecondFieldTextBlock,
                ThirdFieldTextBlock,
                FourthColumnTextBlock,
                FifthColumnTextBlock,
                SixthColumnTextBlock,
                SeventhColumnTextBlock,
                EighthColumnTextBlock,
                NinthColumnTextBlock});
            FieldsTextBox.AddRange(new List<TextBox>{
                FirstFieldTextBox,
                SecondFieldTextBox,
                ThirdFieldTextBox,
                FourthColumnTextBox,
                FifthColumnTextBox,
                SixthColumnTextBox,
                SeventhColumnTextBox,
                EighthColumnTextBox,
                NinthColumnTextBox});
            _record = dbRepository.GetColumsNameByTableName(tableName);
            for (int i = 0; i < _record.Columns.Count; i++)
            {
                FieldsStackPanel[i].Visibility = Visibility.Visible;
                FieldsTextBlock[i].Text = _record.Columns[i].ColumnName;
            }
        }

        private void AddRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            _record.Rows.Add();
            for (int i = 1; i < _record.Columns.Count; i++)
            {
                if (Int32.TryParse(FieldsTextBox[i].Text, out int intField))
                {
                    _record.Rows[0][_record.Columns[i].ColumnName] = intField;
                }
                else
                {
                    _record.Rows[0][_record.Columns[i].ColumnName] = FieldsTextBox[i].Text;
                }
            }
            try
            {
                _dbRepository.AddRecord(_record, _tableName);
                _mainMenuAccountView.UpdateAllObservableCollection();
                MessageBox.Show("Запись успешно добавлена");
            }
            catch (Exception)
            {
                MessageBox.Show("Что-то пошло не так! Проверьте введенные данные");
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimazeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


    }
}
