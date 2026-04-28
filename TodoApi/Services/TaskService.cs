using System.Collections.Concurrent;
using TodoApi.Dtos;
using TodoApi.Mappings;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskStore _taskStore;
        private int _nextId = 1;

        public TaskService(ITaskStore taskStore)
        {
            _taskStore = taskStore;
        }

        public Task<IEnumerable<TaskReadDto>> GetAllAsync()
        {
            var tasks = _taskStore.Tasks.Values.Select(task => task.ToReadDto());
            return Task.FromResult(tasks);
        }

        public Task<TaskReadDto?> GetByIdAsync(int id)
        {
            _taskStore.Tasks.TryGetValue(id, out var task);
            var result = task?.ToReadDto();
            return Task.FromResult(result);
        }

        public Task<TaskReadDto> AddAsync(TaskCreateDto createDto)
        {
            var task = createDto.ToModel();
            task.Id = Interlocked.Increment(ref _nextId);
            task.CreatedAt = DateTime.UtcNow;
            _taskStore.Tasks.TryAdd(task.Id, task);
            return Task.FromResult(task.ToReadDto());
        }

        public Task<bool> UpdateAsync(int id, TaskUpdateDto updateDto)
        {
            if (!_taskStore.Tasks.TryGetValue(id, out var existingTask))
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

            var updated = _taskStore.Tasks.TryUpdate(id, updatedTask, existingTask);
            return Task.FromResult(updated);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var result = _taskStore.Tasks.TryRemove(id, out _);
            return Task.FromResult(result);
        }
    }
}
