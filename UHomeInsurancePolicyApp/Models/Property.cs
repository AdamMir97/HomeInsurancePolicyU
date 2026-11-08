namespace UHomeInsurancePolicyApp.Models
{
    public class Property
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }

        public string? AddressLine3 { get; set; }

        public string Postcode { get; set; } = string.Empty;
    }
}
