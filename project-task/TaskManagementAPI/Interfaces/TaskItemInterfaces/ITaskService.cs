using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementAPI.DTOs.TaskItems;
using TaskManagementAPI.Enums;

namespace TaskManagementAPI.Interfaces
{
    public interface ITaskService
    {
        Task<TaskItemResponseDto> CreateTaskAsync(TaskItemRequestDto dto, Guid creatorId);
        Task<TaskItemResponseDto> UpdateTaskAsync(Guid taskId, TaskItemUpdateDto dto, Guid updaterId);
        Task DeleteTaskAsync(Guid taskId, Guid updaterId);
        Task<TaskItemResponseDto> GetTaskByIdAsync(Guid taskId);
        Task<IEnumerable<TaskItemResponseDto>> GetAllTasksAsync(Guid requesterId);
        Task<IEnumerable<TaskItemResponseDto>> GetTasksByStatusAsync(TaskState status,Guid requesterId);
    }
}