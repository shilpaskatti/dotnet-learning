using LearningBasics.DTOs.Request;
using LearningBasics.DTOs.Response;
using LearningBasics.Extensions;
using LearningBasics.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearningBasics.Services
{
    public interface IUserService
    {
       

        Task<IEnumerable<GetUserResponse>> GetUsersAsync();

        Task<GetUserResponse> GetUserByIdAsync(int id);

        Task<int> CreateUserAsync(CreateUserRequest user);

        Task<int> UpdateUserAsync(UpdateUserRequest user);
        Task<int> DeleteUserByIdAsync(int id);
    }
}
