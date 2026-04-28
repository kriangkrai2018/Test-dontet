using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITaskService
    {
        IEnumerable<TaskItem> GetAll();
        TaskItem? GetById(int id);
        TaskItem Add(TaskItem task);
        bool Update(int id, TaskItem updatedTask);
        bool Delete(int id);
    }
}
