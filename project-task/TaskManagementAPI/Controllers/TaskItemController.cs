using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementAPI.ApiResponses;
using TaskManagementAPI.DTOs.TaskItems;
using TaskManagementAPI.Enums;
using TaskManagementAPI.Interfaces;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskItemController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([FromForm] TaskItemRequestDto dto)
        {
                        
            try
            {
                var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _taskService.CreateTaskAsync(dto, creatorId);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<TaskItemResponseDto>.SuccessResponse(result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<TaskItemResponseDto>.ErrorResponse(ex.Message, new Dictionary<string, List<string>>()));
            }
            catch (Exception ex)
            {
                return ErrorResponse<TaskItemResponseDto>("Failed to create task", ex);
            }
            
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager, TeamMember")]
        public async Task<IActionResult> Update(Guid id, [FromForm] TaskItemUpdateDto dto)
        {
            try
            {
                var updaterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _taskService.UpdateTaskAsync(id, dto, updaterId);
                return Ok(ApiResponse<TaskItemResponseDto>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return ErrorResponse<TaskItemResponseDto>("Failed to update task", ex);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var updaterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await _taskService.DeleteTaskAsync(id, updaterId);

                return Ok(ApiResponse<string>.SuccessResponse("Task deleted successfully"));
            }
            catch (Exception ex)
            {
                return ErrorResponse<string>("Failed to delete task", ex);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _taskService.GetTaskByIdAsync(id);
                //var requesterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                //var task = await _taskService.GetTaskByIdAsync(id, requesterId);
                //return Ok(ApiResponse<TaskItemResponseDto>.SuccessResponse(task));
                return Ok(ApiResponse<TaskItemResponseDto>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return ErrorResponse<TaskItemResponseDto>("Failed to fetch task", ex);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var userRole = User.FindFirstValue(ClaimTypes.Role)!;

                var result = await _taskService.GetAllTasksAsync(userId);

                if (!result.Any())
                {
                    string message = userRole == "Manager"
                        ? "No tasks created by you yet."
                        : "No tasks assigned to you yet.";

                    return NotFound(ApiResponse<IEnumerable<TaskItemResponseDto>>.ErrorResponse(
                        message,
                        new Dictionary<string, List<string>>()
                    ));
                }

                return Ok(ApiResponse<IEnumerable<TaskItemResponseDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return ErrorResponse<IEnumerable<TaskItemResponseDto>>("Failed to retrieve tasks", ex);
            }
        }

        [HttpGet("status/{status}")]
        [Authorize]
        public async Task<IActionResult> GetByStatus(TaskState status)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _taskService.GetTasksByStatusAsync(status,userId);
                if (!result.Any())
                {
                    return NotFound(ApiResponse<IEnumerable<TaskItemResponseDto>>.ErrorResponse("No tasks found for this status.", new Dictionary<string, List<string>>()));
                }
                return Ok(ApiResponse<IEnumerable<TaskItemResponseDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return ErrorResponse<IEnumerable<TaskItemResponseDto>>("Failed to retrieve tasks by status", ex);
            }
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery] string? title, [FromQuery] DateTime? dueDate)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _taskService.SearchTasksAsync(userId, title, dueDate);

                if (!result.Any())
                {
                    return NotFound(ApiResponse<IEnumerable<TaskItemResponseDto>>.ErrorResponse(
                        "No tasks found matching the search criteria.",
                        new Dictionary<string, List<string>>()));
                }

                return Ok(ApiResponse<IEnumerable<TaskItemResponseDto>>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                return ErrorResponse<IEnumerable<TaskItemResponseDto>>("Failed to search tasks", ex);
            }
        }


        private IActionResult ErrorResponse<T>(string message, Exception ex)
        {
            var errors = new Dictionary<string, List<string>>
            {
                { "Error", new List<string> { ex.Message } }
            };
            return BadRequest(ApiResponse<T>.ErrorResponse(message, errors));
        }
    }
}
