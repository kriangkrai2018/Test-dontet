using Microsoft.Extensions.Logging.Abstractions;
using TodoApi.Dtos;
using TodoApi.Models;
using TodoApi.Services;
using Xunit;

namespace TodoApi.Tests;

public class TaskServiceTests
{
    [Fact]
    public async Task AddAsync_should_assign_unique_identifiers()
    {
        var store = new InMemoryTaskStore();
        var service = new TaskService(store, NullLogger<TaskService>.Instance);

        var first = await service.AddAsync(new TaskCreateDto { Title = "first", Description = "one" });
        var second = await service.AddAsync(new TaskCreateDto { Title = "second", Description = "two" });

        Assert.Equal(1, first.Id);
        Assert.Equal(2, second.Id);
        Assert.NotEqual(first.Id, second.Id);
    }

    [Fact]
    public async Task UpdateAsync_should_return_false_when_task_missing()
    {
        var store = new InMemoryTaskStore();
        var service = new TaskService(store, NullLogger<TaskService>.Instance);

        var result = await service.UpdateAsync(999, new TaskUpdateDto { Title = "missing", Description = "none", IsCompleted = false });

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_should_update_existing_task()
    {
        var store = new InMemoryTaskStore();
        var service = new TaskService(store, NullLogger<TaskService>.Instance);
        var created = await service.AddAsync(new TaskCreateDto { Title = "start", Description = "initial", IsCompleted = false });

        var updated = await service.UpdateAsync(created.Id, new TaskUpdateDto { Title = "updated", Description = "changed", IsCompleted = true });
        var read = await service.GetByIdAsync(created.Id);

        Assert.True(updated);
        Assert.NotNull(read);
        Assert.Equal("updated", read!.Title);
        Assert.Equal("changed", read.Description);
        Assert.True(read.IsCompleted);
    }

    [Fact]
    public async Task DeleteAsync_should_remove_task()
    {
        var store = new InMemoryTaskStore();
        var service = new TaskService(store, NullLogger<TaskService>.Instance);
        var created = await service.AddAsync(new TaskCreateDto { Title = "to delete", Description = "temp", IsCompleted = false });

        var deleted = await service.DeleteAsync(created.Id);
        var read = await service.GetByIdAsync(created.Id);

        Assert.True(deleted);
        Assert.Null(read);
    }

    [Fact]
    public async Task AddAsync_should_throw_when_add_fails()
    {
        var store = new FailingAddTaskStore();
        var service = new TaskService(store, NullLogger<TaskService>.Instance);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddAsync(new TaskCreateDto { Title = "fail", Description = "nope", IsCompleted = false }));
    }

    private sealed class FailingAddTaskStore : ITaskStore
    {
        private readonly InMemoryTaskStore _inner = new();

        public IEnumerable<TaskItem> GetAll() => _inner.GetAll();
        public TaskItem? GetById(int id) => _inner.GetById(id);
        public bool TryAdd(TaskItem task) => false;
        public bool TryUpdate(TaskItem task) => _inner.TryUpdate(task);
        public bool TryRemove(int id) => _inner.TryRemove(id);
        public int NextId() => _inner.NextId();
    }
}
