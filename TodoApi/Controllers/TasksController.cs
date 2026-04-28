using Microsoft.AspNetCore.Mvc;
using TodoApi.Dtos;
using TodoApi.Mappings;
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
        public async Task<ActionResult<IEnumerable<TaskReadDto>>> GetTasks()
        {
            var tasks = await _taskService.GetAllAsync();
            return Ok(tasks.Select(task => task.ToReadDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskReadDto>> GetTask(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            return task is null ? NotFound() : Ok(task.ToReadDto());
        }

        [HttpPost]
        public async Task<ActionResult<TaskReadDto>> CreateTask([FromBody] TaskCreateDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTask = await _taskService.AddAsync(taskDto.ToModel());
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask.ToReadDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _taskService.UpdateAsync(id, taskDto.ToModel()))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            if (!await _taskService.DeleteAsync(id))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
