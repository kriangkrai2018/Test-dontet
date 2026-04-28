using TodoApi.Dtos;

namespace TodoApi.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskReadDto>> GetAllAsync();
        Task<TaskReadDto?> GetByIdAsync(int id);
        Task<TaskReadDto> AddAsync(TaskCreateDto createDto);
        Task<bool> UpdateAsync(int id, TaskUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
    }
}
