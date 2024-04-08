using DiscussifyApi.Dtos;
using DiscussifyApi.Models;

namespace DiscussifyApi.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>Returns the id of the newly created user</returns>
        Task<int> CreateUser(User user);

        /// <summary>
        /// Gets the user by email
        /// </summary>
        /// <param name="emailAddress">The email of the user</param>
        /// <returns>Returns the details of user</returns>
        Task<UserDto> GetUserByEmail(string emailAddress);

        /// <summary>
        /// Checks if the emailAddress already exists
        /// </summary>
        /// <param name="emailAddress">The emailAddress of the user</param>
        /// <returns>
        /// Returns an integer (0 means that the user already exists, while greater
        /// than 0 means that the user still doesn't exist)
        /// </returns>
        Task<int> CheckIfUserExists(string emailAddress);

        /// <summary>
        /// Checks if the user email exists
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>
        /// Returns an integer (0 means that the user doesn't exist, while greater
        /// than 0 means that the user exists)
        /// </returns>
        Task<string> CheckUserEmailPasswordExists(string email);

        /// <summary>
        /// Gets the user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Returns the details of user with id <paramref name="id"/></returns>
        Task<UserDto> GetUserById(int id);

        /// <summary>
        /// Gets all rooms of user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Returns the details of rooms with userid <paramref name="id"/></returns>
        Task<UserRoomDto?> GetUserRoomsById(int id);
    }
}