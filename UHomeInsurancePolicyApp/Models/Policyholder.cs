namespace UHomeInsurancePolicyApp.Models
{
    public class Policyholder
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
    }
}