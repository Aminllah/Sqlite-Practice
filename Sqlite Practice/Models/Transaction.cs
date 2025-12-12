namespace Sqlite_Practice.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public string Title { get; set;}
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
    }
}
