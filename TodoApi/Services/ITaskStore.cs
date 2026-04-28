using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITaskStore
    {
        List<TaskItem> Tasks { get; }
    }
}
