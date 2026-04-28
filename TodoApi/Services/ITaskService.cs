using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> AddAsync(TaskItem task);
        Task<bool> UpdateAsync(int id, TaskItem updatedTask);
        Task<bool> DeleteAsync(int id);
    }
}
