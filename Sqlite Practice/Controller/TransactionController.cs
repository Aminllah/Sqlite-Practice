using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sqlite_Practice.Data;
using Sqlite_Practice.Models;

namespace Sqlite_Practice.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllTransactions")]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetAllTransactions()
        {
            var transaction = await _context.Transactions.ToListAsync();
            if (transaction == null || transaction.Count == 0)
            {
                return NotFound("No transaction Available");
            }
            return Ok(transaction);
        }
        [HttpPost("AddTransaction")]
        public async Task<ActionResult<TransactionModel>> AddTransaction(TransactionModel transaction)
        {
            if (transaction == null)
            {
                return BadRequest("No data has been Entered");
            }
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return Ok(transaction);

        }

        [HttpGet("GetTransactionbyId/{id}")]
        public async Task<ActionResult<TransactionModel>> GetTransactionbyId(long id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound("No Transaction Found with this Id");
            }
            return transaction;
        }

        [HttpPut("UpdateTransaction/{id}")]
        public async Task<ActionResult<TransactionModel>> UpdateTransaction(long id, TransactionModel updatedTransaction)
        {
            if (updatedTransaction == null)
            {
                return BadRequest();
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound("No Transaction Found with this Id");
            }
            transaction.Title = updatedTransaction.Title;
            transaction.Amount = updatedTransaction.Amount;
            transaction.Category = updatedTransaction.Category;
            transaction.Date = updatedTransaction.Date;
            transaction .Type = updatedTransaction.Type;
            await _context.SaveChangesAsync();
            return Ok(transaction);
        }

        [HttpDelete("Deletetransaction/{id}")]
        public async Task<ActionResult<TodoItemModel>> Deletetransaction(long id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound("No Transaction Found with this Id");
            }
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return Ok("Transaction Deleted Successfully");
        }
        
        [HttpGet("summary")]
        public async Task<ActionResult<object>> GetMonthlySummary(int month, int year)
        {
            var transactions = await _context.Transactions
                .Where(t => t.Date.Month == month && t.Date.Year == year)
                .ToListAsync();

            if (!transactions.Any())
                return NotFound("No transactions for this month.");

            var totalIncome = transactions
                .Where(t => t.Type == "Income")
                .Sum(t => t.Amount);

            var totalExpense = transactions
                .Where(t => t.Type == "Expense")
                .Sum(t => t.Amount);

            var balance = totalIncome - totalExpense;

            return Ok(new
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = balance,
                Transactions = transactions
            });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetTransactions(
        string? type, string? category, string? sortBy = "date")
        {
            var query = _context.Transactions.AsQueryable();

            if (!string.IsNullOrEmpty(type))
                query = query.Where(t => t.Type == type);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(t => t.Category == category);

            query = sortBy?.ToLower() switch
            {
                "amount" => query.OrderBy(t => t.Amount),
                _ => query.OrderBy(t => t.Date)
            };

            var transactions = await query.ToListAsync();
            return Ok(transactions);
        }

        [HttpGet("GetMonthlyTotalsGroupedByCategory")]
        public async Task<ActionResult<TransactionModel>> GetMonthlyTotalsGroupedByCategory(int month, int year)
        {
            var transactions = await _context.Transactions
                .Where(t => t.Date.Month == month && t.Date.Year == year)
                .ToListAsync();

            if (!transactions.Any())
                return NotFound("No transactions for this month.");

            var Category = transactions.GroupBy(
                t => t.Category).Select(
                g =>new
            {
                category=g.Key,
                TotalIncome = transactions
                .Where(t => t.Type == "Income")
                .Sum(t => t.Amount),
                TotalExpense = transactions
                .Where(t => t.Type == "Expense")
                .Sum(t => t.Amount)
        }).ToList();
            return Ok(Category);
        }
    }
}
