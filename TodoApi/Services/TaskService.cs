using TodoApi.Models;

namespace TodoApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks = new();
        private int _nextId = 1;

        public IEnumerable<TaskItem> GetAll()
        {
            return _tasks;
        }

        public TaskItem? GetById(int id)
        {
            return _tasks.FirstOrDefault(task => task.Id == id);
        }

        public TaskItem Add(TaskItem task)
        {
            task.Id = _nextId++;
            task.CreatedAt = DateTime.UtcNow;
            _tasks.Add(task);
            return task;
        }

        public bool Update(int id, TaskItem updatedTask)
        {
            var existingTask = GetById(id);
            if (existingTask is null)
            {
                return false;
            }

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.IsCompleted = updatedTask.IsCompleted;

            return true;
        }

        public bool Delete(int id)
        {
            var task = GetById(id);
            if (task is null)
            {
                return false;
            }

            return _tasks.Remove(task);
        }
    }
}
