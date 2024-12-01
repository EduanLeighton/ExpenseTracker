using SQLite;

namespace ExpenseTracker.Model
{
    public class ExpenseItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public decimal Salary { get; set; }
        public decimal AmountExpense { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
    }
}
