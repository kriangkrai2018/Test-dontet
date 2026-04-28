using System.Collections.Concurrent;
using System.Threading;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class InMemoryTaskStore : ITaskStore
    {
        private readonly ConcurrentDictionary<int, TaskItem> _tasks = new();
        private int _nextId;

        public IEnumerable<TaskItem> GetAll()
        {
            return _tasks.Values;
        }

        public TaskItem? GetById(int id)
        {
            _tasks.TryGetValue(id, out var task);
            return task;
        }

        public bool TryAdd(TaskItem task)
        {
            return _tasks.TryAdd(task.Id, task);
        }

        public bool TryUpdate(TaskItem task)
        {
            if (!_tasks.TryGetValue(task.Id, out var existingTask))
            {
                return false;
            }

            return _tasks.TryUpdate(task.Id, task, existingTask);
        }

        public bool TryRemove(int id)
        {
            return _tasks.TryRemove(id, out _);
        }

        public int NextId()
        {
            return Interlocked.Increment(ref _nextId);
        }
    }
}
