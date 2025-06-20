using TaskManagementAPI.ApiResponses;
using TaskManagementAPI.DTOs.Users;

namespace TaskManagementAPI.Interfaces
{
    public interface IUserService
    {
        Task<PagedResponse<UserResponseDto>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<ApiResponse<UserResponseDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<UserResponseDto>> CreateAsync(UserRequestDto dto, Guid createdById);
        Task<ApiResponse<UserResponseDto>> UpdateAsync(Guid id, UserUpdateDto dto, Guid updatedById);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
