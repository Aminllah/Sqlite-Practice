using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sqlite_Practice.Data;
using Sqlite_Practice.DTO;
using Sqlite_Practice.Models;
using Sqlite_Practice.Services;

namespace Sqlite_Practice.Controller
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _todoService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var item = await _todoService.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTodoDto dto)
            => Ok(await _todoService.CreateAsync(dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, UpdateToDoDTO dto)
            => await _todoService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
            => await _todoService.DeleteAsync(id) ? NoContent() : NotFound();
    }

}
