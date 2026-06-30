using LearningBasics.Data;
using LearningBasics.DTOs.Request;
using LearningBasics.DTOs.Response;
using LearningBasics.Models;
using LearningBasics.Repository.Users;
using FluentValidation;
using LearningBasics.Exceptions;

namespace LearningBasics.Services
{
    public class UserService(
        AppDbContext context, 
        IUserRepository userRepository,
        IValidator<CreateUserRequest> createUserValidator,
        ILogger<UserService> logger):IUserService
    {
       
        public async Task<IEnumerable<GetUserResponse>> GetUsersAsync()
        {
            //return await context.Users.Select(m => new GetUserResponse
            //{
            //    FirstName = m.FirstName,
            //    LastName = m.LastName,
            //    Id = m.Id,
            //    Subjects = m.Subjects,
            //    DateOfBirth = m.DateOfBirth
            //}).ToListAsync();

            userRepository = null;
            var result = await userRepository.GetAllUsers();


            return result.Select(m => new GetUserResponse
            {
                FirstName = m.FirstName,
                LastName = m.LastName,
                Id = m.Id,
                Subjects = m.Subjects,
                DateOfBirth = m.DateOfBirth
            });

        }

        public async Task<GetUserResponse> GetUserByIdAsync(int id)
        {
            //var result = await context.Users.Where(m => m.Id == id).FirstOrDefaultAsync();
            //return new GetUserResponse
            //{
            //    Id = result?.Id,
            //    FirstName = result?.FirstName,
            //    LastName= result?.LastName,
            //    Subjects= result?.Subjects,
            //    DateOfBirth = result?.DateOfBirth
            //};

            var result = await userRepository.GetUserById(id);
            return new GetUserResponse
            {
                Id = result?.Id,
                FirstName = result?.FirstName,
                LastName = result?.LastName,
                Subjects = result?.Subjects,
                DateOfBirth = result?.DateOfBirth
            };
        }

        public async Task<int> CreateUserAsync(CreateUserRequest user)
        {
            var result = await createUserValidator.ValidateAsync(user);
            if(!result.IsValid)
            {
                var errors = result.ToDictionary();
                throw new ValidationErrorException(errors);
            }
            //var result = await context.Users.AddAsync(new User
            //{
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    Subjects = user.Subjects,
            //    DateOfBirth = user.DateOfBirth
            //});
            //return await context.SaveChangesAsync();
            return await userRepository.CreateUser(new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Subjects = user.Subjects,
                DateOfBirth = user.DateOfBirth
            });
        }

        public async Task<int> UpdateUserAsync(UpdateUserRequest user)
        {
            //var foundUser = await context.Users.AsNoTracking().FirstOrDefaultAsync(m => m.Id == user.Id);
            //if (foundUser == null || foundUser.Id <= 0)
            //{
            //    return 0;
            //}
            //context.Users.Update(new User
            //{
            //    Id = user.Id,
            //    FirstName= user.FirstName,
            //    LastName = user.LastName,
            //    Subjects = user.Subjects,
            //    DateOfBirth = user.DateOfBirth

            //});
            //return await context.SaveChangesAsync();

            return await userRepository.UpdateUser(new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Subjects = user.Subjects,
                DateOfBirth = user.DateOfBirth
            });
        }


        public async Task<int> DeleteUserByIdAsync(int id)
        {
            //var foundUser = await context.Users.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            //if (foundUser == null || foundUser.Id <= 0)
            //{
            //    return 0;
            //}
            //context.Users.Remove(new User
            //{
            //    Id = id
            //});
            //return await context.SaveChangesAsync();

            return await userRepository.DeleteUser(id);
        }
    }
}
