using Sqlite_Practice.DTO;

namespace Sqlite_Practice.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<ResponseToDoDTO>> GetAllAsync();
        Task<ResponseToDoDTO> GetByIdAsync(long id);
        Task<ResponseToDoDTO> CreateAsync(CreateTodoDto dto);
        Task<bool> UpdateAsync(long id, UpdateToDoDTO dto);
        Task<bool> DeleteAsync(long id);
    }

}
