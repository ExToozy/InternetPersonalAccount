using System;

namespace ViewLayer.Models
{
    public class Payment
    {
        public Payment(int userId, string dateOnly, string status, int value)
        {
            UserId = userId;
            DateOnly = dateOnly;
            Status = status;
            Value = value;
        }
        public Payment() { }
        public int Id { get; set; }
        public int UserId { get; set; }
        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                DateOnly = date.ToShortDateString();
            }
        }
        public string DateOnly { get; set; }
        public string Status { get; set; }
        public int Value { get; set; }
    }
}
