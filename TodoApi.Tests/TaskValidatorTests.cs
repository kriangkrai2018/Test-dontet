using TodoApi.Dtos;
using TodoApi.Validators;
using Xunit;

namespace TodoApi.Tests;

public class TaskValidatorTests
{
    [Fact]
    public void CreateDtoValidator_should_fail_when_title_is_empty()
    {
        var validator = new TaskCreateDtoValidator();
        var result = validator.Validate(new TaskCreateDto { Title = string.Empty });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(TaskCreateDto.Title));
    }

    [Fact]
    public void UpdateDtoValidator_should_fail_when_title_is_empty()
    {
        var validator = new TaskUpdateDtoValidator();
        var result = validator.Validate(new TaskUpdateDto { Title = string.Empty });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(TaskUpdateDto.Title));
    }
}
