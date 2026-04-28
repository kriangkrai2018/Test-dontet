using System.Collections.Concurrent;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class InMemoryTaskStore : ITaskStore
    {
        public ConcurrentDictionary<int, TaskItem> Tasks { get; } = new();
    }
}
