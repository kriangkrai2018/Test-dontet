using System.Threading;
using Microsoft.Extensions.Logging;
using TodoApi.Dtos;
using TodoApi.Mappings;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskStore _taskStore;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskStore taskStore, ILogger<TaskService> logger)
        {
            _taskStore = taskStore;
            _logger = logger;
        }

        public Task<IEnumerable<TaskReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogDebug("Retrieving all tasks.");
            var tasks = _taskStore.GetAll().Select(task => task.ToReadDto());
            return Task.FromResult(tasks);
        }

        public Task<TaskReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogDebug("Retrieving task with ID {TaskId}.", id);
            var task = _taskStore.GetById(id);
            return Task.FromResult(task?.ToReadDto());
        }

        public Task<TaskReadDto> AddAsync(TaskCreateDto createDto, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var task = createDto.ToModel();
            task.Id = _taskStore.NextId();
            task.CreatedAt = DateTime.UtcNow;

            if (!_taskStore.TryAdd(task))
            {
                _logger.LogError("Failed to add task with generated ID {TaskId}.", task.Id);
                throw new InvalidOperationException($"Failed to add task with ID {task.Id}.");
            }

            _logger.LogInformation("Created task with ID {TaskId}.", task.Id);
            return Task.FromResult(task.ToReadDto());
        }

        public Task<bool> UpdateAsync(int id, TaskUpdateDto updateDto, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Updating task with ID {TaskId}.", id);

            var existingTask = _taskStore.GetById(id);
            if (existingTask is null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found for update.", id);
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
            if (updated)
            {
                _logger.LogInformation("Task with ID {TaskId} updated successfully.", id);
            }

            return Task.FromResult(updated);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Deleting task with ID {TaskId}.", id);
            var result = _taskStore.TryRemove(id);
            if (!result)
            {
                _logger.LogWarning("Failed to delete task with ID {TaskId}; not found.", id);
            }

            return Task.FromResult(result);
        }
    }
}
