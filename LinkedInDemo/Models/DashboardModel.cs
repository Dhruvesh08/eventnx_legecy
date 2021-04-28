namespace LinkedInDemo.Models
{
    public class DashboardModel
    {
        public int TotalEvent { get; set; }
        public int RegisteredUser { get; set; }
        public int LinkedinUser { get; set; }
        public int FacebookUser { get; set; }
        public int AvailableCredits { get; set; }
        public decimal TotalDeposit { get; set; }
        public int CreditsUsed { get ; set;}
        public decimal TotalCustomers { get; set; }
        public decimal ActiveCustomers { get; set; }
        public decimal TotalPurchase { get; set; }
        public string Name { get; set; }
      
    }
}