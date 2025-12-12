using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sqlite_Practice.Data;
using Sqlite_Practice.Models;

namespace Sqlite_Practice.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet("GetAllItems")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<IEnumerable<TodoItemModel>>> GetAllItems()
        {
            var Items = await _context.TodoItems.ToListAsync();
            if (Items == null || Items.Count==0)
            {
                return NotFound("No item Available");
            }
            return Ok(Items);
        }

        [HttpPost("AddItem")]
        public async Task<ActionResult<TodoItemModel>> AddItem(TodoItemModel item)
        {
            if (item == null)
            {
                return BadRequest("No data has been Entered");
            }
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();
            return Ok(item);

        }

        [HttpGet("GetItembyId/{id}")]
        public async Task<ActionResult<TodoItemModel>> GetItemById(long id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound("No Item Found with this Id");
            }
            return item;
        }

        [HttpPut("UpdateItem/{id}")]
        public async Task<ActionResult<TodoItemModel>> UpdateItem(long id,TodoItemModel updateditem)
        {
            if(updateditem == null)
            {
                return BadRequest();
            }
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound("No Item Found with this Id");
            }
            item.Name = updateditem.Name;
            item.IsComplete = updateditem.IsComplete;
            await _context.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPut("UpdateItemStatus/{id}")]
        public async Task<ActionResult<TodoItemModel>> UpdateItemStatus(long id, bool status)
        {
            
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound("No Item Found with this Id");
            }
            
            item.IsComplete = status;
            await _context.SaveChangesAsync();
            return Ok(item);
        } 
        [HttpDelete("DeleteItem/{id}")]
        public async Task<ActionResult<TodoItemModel>> DeleteItem(long id)
        {
            var item =await _context.TodoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound("No Item Found with this Id");
            }
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return Ok("Item Deleted Successfully");
        }

        [HttpGet("CompletedItems")]
        public async Task<ActionResult<IEnumerable<TodoItemModel>>> GetCompletedItems()
        {
            var items = await _context.TodoItems
                .Where(t => t.IsComplete)
                .ToListAsync();
            return items.Any() ? Ok(items) : NotFound("No completed items found.");
        }

    }
}
