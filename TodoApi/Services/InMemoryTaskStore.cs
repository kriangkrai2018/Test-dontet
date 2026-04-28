using TodoApi.Models;

namespace TodoApi.Services
{
    public class InMemoryTaskStore : ITaskStore
    {
        public List<TaskItem> Tasks { get; } = new();
    }
}
