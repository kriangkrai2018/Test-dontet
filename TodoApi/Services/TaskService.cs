using TodoApi.Dtos;
using TodoApi.Mappings;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskStore _taskStore;

        public TaskService(ITaskStore taskStore)
        {
            _taskStore = taskStore;
        }

        public Task<IEnumerable<TaskReadDto>> GetAllAsync()
        {
            var tasks = _taskStore.GetAll().Select(task => task.ToReadDto());
            return Task.FromResult(tasks);
        }

        public Task<TaskReadDto?> GetByIdAsync(int id)
        {
            var task = _taskStore.GetById(id);
            return Task.FromResult(task?.ToReadDto());
        }

        public Task<TaskReadDto> AddAsync(TaskCreateDto createDto)
        {
            var task = createDto.ToModel();
            task.Id = _taskStore.NextId();
            task.CreatedAt = DateTime.UtcNow;
            _taskStore.TryAdd(task);
            return Task.FromResult(task.ToReadDto());
        }

        public Task<bool> UpdateAsync(int id, TaskUpdateDto updateDto)
        {
            var existingTask = _taskStore.GetById(id);
            if (existingTask is null)
            {
                return Task.FromResult(false);
            }

            var updatedTask = new TaskItem
            {
                Id = existingTask.Id,
                Title = updateDto.Title,
                Description = updateDto.Description,
                IsCompleted = updateDto.IsCompleted,
                CreatedAt = existingTask.CreatedAt
            };

            var updated = _taskStore.TryUpdate(updatedTask);
            return Task.FromResult(updated);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var result = _taskStore.TryRemove(id);
            return Task.FromResult(result);
        }
    }
}
