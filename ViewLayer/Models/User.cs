using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewLayer.Models
{
    public class User : INotifyPropertyChanged
    {
        public User(string fullName, string phoneNumber, string password)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Password = password;
            Balance = 0;
            IsAdmin = 0;
        }
        public User() { }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        private int balance;
        public int Balance
        {
            get => balance;
            set
            {
                balance = value;
                OnPropertyChanged();
            }
        }
        public int IsAdmin { get; set; }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                if (value == null)
                {
                    email = "";
                    
                }
                else
                {
                    email = value;
                    OnPropertyChanged();
                }
            }
        }
        private int maillingIsOn;
        public int MailingIsOn
        {
            get => maillingIsOn;
            set
            {
                maillingIsOn = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
