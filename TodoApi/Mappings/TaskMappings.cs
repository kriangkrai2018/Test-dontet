using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Mappings
{
    public static class TaskMappings
    {
        public static TaskReadDto ToReadDto(this TaskItem task)
        {
            return new TaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
        }

        public static TaskItem ToModel(this TaskCreateDto dto)
        {
            return new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted
            };
        }

    }
}
