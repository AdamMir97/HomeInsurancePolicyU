namespace UHomeInsurancePolicyApp.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public string PaymentReference { get; set; } = string.Empty;

        public PolicyType PolicyType { get; set; }

        public decimal Amount { get; set; }
    }
}
