using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using TodoApi.Dtos;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Tests.Services;

public class TaskServiceTests
{
    private readonly ITaskStore _store;
    private readonly TaskService _sut;

    public TaskServiceTests()
    {
        _store = Substitute.For<ITaskStore>();
        _sut = new TaskService(_store, NullLogger<TaskService>.Instance);
    }

    // ── GetAllAsync ──────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTasks()
    {
        _store.GetAll().Returns([
            new TaskItem { Id = 1, Title = "Task A" },
            new TaskItem { Id = 2, Title = "Task B" }
        ]);

        var result = await _sut.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllAsync_WhenCancelled_ShouldThrow()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();

        await _sut.Invoking(s => s.GetAllAsync(cts.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }

    // ── GetByIdAsync ─────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_WhenFound_ShouldReturnTask()
    {
        _store.GetById(1).Returns(new TaskItem { Id = 1, Title = "Task A" });

        var result = await _sut.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Title.Should().Be("Task A");
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ShouldReturnNull()
    {
        _store.GetById(99).Returns((TaskItem?)null);

        var result = await _sut.GetByIdAsync(99);

        result.Should().BeNull();
    }

    // ── AddAsync ─────────────────────────────────────────────

    [Fact]
    public async Task AddAsync_ShouldReturnCreatedTask_WithCorrectFields()
    {
        _store.NextId().Returns(1);
        _store.TryAdd(Arg.Any<TaskItem>()).Returns(true);

        var dto = new TaskCreateDto { Title = "New Task", Description = "Desc" };
        var result = await _sut.AddAsync(dto);

        result.Id.Should().Be(1);
        result.Title.Should().Be("New Task");
        result.Description.Should().Be("Desc");
    }

    [Fact]
    public async Task AddAsync_WhenStoreFails_ShouldThrowInvalidOperationException()
    {
        _store.NextId().Returns(1);
        _store.TryAdd(Arg.Any<TaskItem>()).Returns(false);

        var dto = new TaskCreateDto { Title = "New Task" };

        await _sut.Invoking(s => s.AddAsync(dto))
            .Should().ThrowAsync<InvalidOperationException>();
    }

    // ── UpdateAsync ──────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_WhenFound_ShouldReturnTrue()
    {
        _store.GetById(1).Returns(new TaskItem { Id = 1, Title = "Old" });
        _store.TryUpdate(Arg.Any<TaskItem>()).Returns(true);

        var dto = new TaskUpdateDto { Title = "New Title", IsCompleted = true };
        var result = await _sut.UpdateAsync(1, dto);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ShouldReturnFalse()
    {
        _store.GetById(99).Returns((TaskItem?)null);

        var dto = new TaskUpdateDto { Title = "New Title" };
        var result = await _sut.UpdateAsync(99, dto);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldPreserve_CreatedAt()
    {
        var createdAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        _store.GetById(1).Returns(new TaskItem { Id = 1, Title = "Old", CreatedAt = createdAt });
        _store.TryUpdate(Arg.Is<TaskItem>(t => t.CreatedAt == createdAt)).Returns(true);

        var dto = new TaskUpdateDto { Title = "New Title" };
        await _sut.UpdateAsync(1, dto);

        _store.Received(1).TryUpdate(Arg.Is<TaskItem>(t => t.CreatedAt == createdAt));
    }

    // ── DeleteAsync ──────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_WhenFound_ShouldReturnTrue()
    {
        _store.TryRemove(1).Returns(true);

        var result = await _sut.DeleteAsync(1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ShouldReturnFalse()
    {
        _store.TryRemove(99).Returns(false);

        var result = await _sut.DeleteAsync(99);

        result.Should().BeFalse();
    }
}
