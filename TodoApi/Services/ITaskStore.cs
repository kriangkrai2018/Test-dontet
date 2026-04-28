using System.Collections.Generic;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITaskStore
    {
        IEnumerable<TaskItem> GetAll();
        TaskItem? GetById(int id);
        bool TryAdd(TaskItem task);
        bool TryUpdate(TaskItem task);
        bool TryRemove(int id);
        int NextId();
    }
}
