using Microsoft.EntityFrameworkCore;
using Sqlite_Practice.Models;

namespace Sqlite_Practice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TodoItemModel> TodoItems { get; set; }

        public DbSet<TransactionModel> Transactions { get; set; }

        public DbSet<UserModel> Users { get; set; }
    }
}
