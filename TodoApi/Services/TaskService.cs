using TodoApi.Models;

namespace TodoApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks = new();
        private int _nextId = 1;

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            await Task.Yield();
            return _tasks;
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            await Task.Yield();
            return _tasks.FirstOrDefault(task => task.Id == id);
        }

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            await Task.Yield();
            task.Id = _nextId++;
            task.CreatedAt = DateTime.UtcNow;
            _tasks.Add(task);
            return task;
        }

        public async Task<bool> UpdateAsync(int id, TaskItem updatedTask)
        {
            var existingTask = await GetByIdAsync(id);
            if (existingTask is null)
            {
                return false;
            }

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.IsCompleted = updatedTask.IsCompleted;

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await GetByIdAsync(id);
            if (task is null)
            {
                return false;
            }

            return _tasks.Remove(task);
        }
    }
}
