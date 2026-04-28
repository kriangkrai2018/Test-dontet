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

        public async Task<IEnumerable<TaskReadDto>> GetAllAsync()
        {
            await Task.CompletedTask;
            return _taskStore.Tasks.Select(task => task.ToReadDto());
        }

        public async Task<TaskReadDto?> GetByIdAsync(int id)
        {
            await Task.CompletedTask;
            var task = _taskStore.Tasks.FirstOrDefault(item => item.Id == id);
            return task?.ToReadDto();
        }

        public async Task<TaskReadDto> AddAsync(TaskCreateDto createDto)
        {
            await Task.CompletedTask;
            var task = createDto.ToModel();
            task.Id = _nextId++;
            task.CreatedAt = DateTime.UtcNow;
            _taskStore.Tasks.Add(task);
            return task.ToReadDto();
        }

        public async Task<bool> UpdateAsync(int id, TaskUpdateDto updateDto)
        {
            await Task.CompletedTask;
            var existingTask = _taskStore.Tasks.FirstOrDefault(item => item.Id == id);
            if (existingTask is null)
            {
                return false;
            }

            ApplyUpdate(existingTask, updateDto);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await Task.CompletedTask;
            var task = _taskStore.Tasks.FirstOrDefault(item => item.Id == id);
            return task is not null && _taskStore.Tasks.Remove(task);
        }

        private static void ApplyUpdate(TaskItem task, TaskUpdateDto updateDto)
        {
            task.Title = updateDto.Title;
            task.Description = updateDto.Description;
            task.IsCompleted = updateDto.IsCompleted;
        }
    }
}
