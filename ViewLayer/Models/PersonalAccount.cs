namespace ViewLayer.Models
{
    public class PersonalAccount
    {
        public PersonalAccount(int userId, int lastSuccessfullPaymentId, int tariffsId)
        {
            UserId = userId;
            LastSuccessfullPaymentId = lastSuccessfullPaymentId;
            TariffsId = tariffsId;
        }
        public PersonalAccount() { }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LastSuccessfullPaymentId { get; set; }
        public int TariffsId { get; set; }

    }
}
