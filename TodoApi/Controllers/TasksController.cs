using Microsoft.AspNetCore.Mvc;
using TodoApi.Dtos;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskReadDto>>> GetAll()
        {
            var tasks = await _taskService.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskReadDto>> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            return task is null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskReadDto>> Create([FromBody] TaskCreateDto taskDto)
        {
            var createdTask = await _taskService.AddAsync(taskDto);
            return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskUpdateDto taskDto)
        {
            if (!await _taskService.UpdateAsync(id, taskDto))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _taskService.DeleteAsync(id))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
