using System.Collections.Concurrent;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITaskStore
    {
        ConcurrentDictionary<int, TaskItem> Tasks { get; }
    }
}
