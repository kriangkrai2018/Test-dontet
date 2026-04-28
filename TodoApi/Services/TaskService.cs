using TodoApi.Dtos;
using TodoApi.Mappings;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks = new();
        private int _nextId = 1;

        public async Task<IEnumerable<TaskReadDto>> GetAllAsync()
        {
            await Task.Yield();
            return _tasks.Select(task => task.ToReadDto());
        }

        public async Task<TaskReadDto?> GetByIdAsync(int id)
        {
            await Task.Yield();
            var task = _tasks.FirstOrDefault(item => item.Id == id);
            return task?.ToReadDto();
        }

        public async Task<TaskReadDto> AddAsync(TaskCreateDto createDto)
        {
            await Task.Yield();
            var task = createDto.ToModel();
            task.Id = _nextId++;
            task.CreatedAt = DateTime.UtcNow;
            _tasks.Add(task);
            return task.ToReadDto();
        }

        public async Task<bool> UpdateAsync(int id, TaskUpdateDto updateDto)
        {
            await Task.Yield();
            var existingTask = _tasks.FirstOrDefault(item => item.Id == id);
            if (existingTask is null)
            {
                return false;
            }

            ApplyUpdate(existingTask, updateDto);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await Task.Yield();
            var task = _tasks.FirstOrDefault(item => item.Id == id);
            return task is not null && _tasks.Remove(task);
        }

        private static void ApplyUpdate(TaskItem task, TaskUpdateDto updateDto)
        {
            task.Title = updateDto.Title;
            task.Description = updateDto.Description;
            task.IsCompleted = updateDto.IsCompleted;
        }
    }
}
