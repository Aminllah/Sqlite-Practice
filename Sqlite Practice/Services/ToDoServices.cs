using Microsoft.EntityFrameworkCore;
using Sqlite_Practice.Data;
using Sqlite_Practice.DTO;
using Sqlite_Practice.Models;

namespace Sqlite_Practice.Services
{
    public class TodoService : ITodoService
    {
        private readonly AppDbContext _context;

        public TodoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResponseToDoDTO>> GetAllAsync()
        {
            return await _context.TodoItems
                .Select(t => new ResponseToDoDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    IsComplete = t.IsComplete
                })
                .ToListAsync();
        }


        public async Task<ResponseToDoDTO> GetByIdAsync(long id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null) return null;

            return new ResponseToDoDTO
            {
                Id = item.Id,
                Name = item.Name,
                IsComplete = item.IsComplete
            };
        }

        public async Task<ResponseToDoDTO> CreateAsync(CreateTodoDto dto)
        {
            var item = new TodoItemModel
            {
                Name = dto.Name,
                IsComplete = false
            };

            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return new ResponseToDoDTO
            {
                Id = item.Id,
                Name = item.Name,
                IsComplete = item.IsComplete
            };
        }

        public async Task<bool> UpdateAsync(long id, UpdateToDoDTO dto)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null) return false;

            item.Name = dto.Name;
            item.IsComplete = dto.IsComplete;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null) return false;

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

    }

}
