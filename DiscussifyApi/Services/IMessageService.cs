using DiscussifyApi.Dtos;
using DiscussifyApi.Models;

namespace DiscussifyApi.Services
{
    public interface IMessageService
    {
        /// <summary>
        /// Creates a new message in a room
        /// </summary>
        /// <param name="roomId">Room id</param>
        /// <param name="message">Message details</param>
        /// <returns>Returns the message model</returns>
        Task<MessageGetDto> CreateMessage(int roomId, MessageCreationDto message);

        /// <summary>
        /// Gets all messages in a room
        /// </summary>
        /// <returns>Returns all messages in a room</returns>
        Task<IEnumerable<MessageGetDto>> GetAllMessages(int id);

        /// <summary>
        /// Gets the message by id
        /// </summary>
        /// <param name="id">Message id</param>
        /// <returns>Returns the details of message with id <paramref name="id"/></returns>
        Task<MessageGetDto> GetMessageById(int id);

        /// <summary>
        /// Updates an existing message
        /// </summary>
        /// <param name="roomId">Message id</param>
        /// <param name="id">The id of the message that will be updated</param>
        /// <param name="message">New message details</param>
        /// <returns>
        /// Returns an integer (0 means that the update failed, while greater
        /// than 0 means that the message was updated successfully)
        /// </returns>
        Task<MessageGetDto> UpdateMessage(int roomId, int id, MessageUpdationDto message);

        /// <summary>
        /// Upvotes an existing message in a room
        /// </summary>
        /// <param name="id">The id of the message that will be upvoted</param>
        /// <param name="increment">The amount by which the message will be upvoted</param>
        /// <returns>Returns the message details</returns>
        Task<MessageGetDto> UpvoteMessage(int id, int increment);

        /// <summary>
        /// Downvotes an existing message in a room
        /// </summary>
        /// <param name="id">The id of the message that will be downvoted</param>
        /// <param name="decrement">The amount by which the message will be downvoted</param>
        /// <returns>Returns the message details</returns>
        Task<MessageGetDto> DownvoteMessage(int id, int decrement);

        /// <summary>
        /// Deletes an existing message in a room
        /// </summary>
        /// <param name="id">The id of the message that will be deleted</param>
        /// <returns>
        /// Returns an integer (0 means that the delete failed, while greater
        /// than 0 means that the message was deleted successfully)
        /// </returns>
        Task<int> DeleteMessage(int id);
    }
}
