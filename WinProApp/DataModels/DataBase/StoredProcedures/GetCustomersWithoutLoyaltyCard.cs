namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class GetCustomersWithoutLoyaltyCard
    {
        public long Id { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
    }
}
