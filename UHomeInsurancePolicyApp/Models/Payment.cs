namespace UHomeInsurancePolicyApp.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public string PaymentReference { get; set; } = string.Empty;

        public PaymentMethod PaymentType { get; set; }

        public decimal Amount { get; set; }

        // Relate to Policy
        public int PolicyId { get; set; }

        public Policy Policy { get; set; } = null!;
    }
}
