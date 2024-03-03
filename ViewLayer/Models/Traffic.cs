using System;


namespace ViewLayer.Models
{
    public class Traffic
    {
        public Traffic(int userId, string ip, DateTime date, int useTimeDuration)
        {
            UserId = userId;
            Ip = ip;
            Date = date;
            UseTimeDuration = useTimeDuration;
        }
        public Traffic() { }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Ip { get; set; }
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
        public int UseTimeDuration { get; set; }
    }
}
