using TodoApi.Dtos;

namespace TodoApi.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskReadDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TaskReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<TaskReadDto> AddAsync(TaskCreateDto createDto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int id, TaskUpdateDto updateDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
