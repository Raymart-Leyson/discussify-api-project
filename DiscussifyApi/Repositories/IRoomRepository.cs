﻿using DiscussifyApi.Dtos;
using DiscussifyApi.Models;

namespace DiscussifyApi.Repositories
{
    public interface IRoomRepository
    {
        /// <summary>
        /// Creates a new room
        /// </summary>
        /// <param name="room">Room details</param>
        /// <returns>Returns the id of the newly created room</returns>
        Task<int> CreateRoom(Room room);

        /// <summary>
        /// Gets the room id by code
        /// </summary>
        /// <param name="code">Room code</param>
        /// <returns>Returns the id of the room with code <paramref name="code"/></returns>
        Task<int> GetRoomIdByCode(string code);

        /// <summary>
        /// Gets all rooms
        /// </summary>
        /// <returns>Returns all rooms</returns>
        Task<IEnumerable<RoomUserDto>> GetAllRooms();

        /// <summary>
        /// Gets the room by id
        /// </summary>
        /// <param name="id">Room id</param>
        /// <returns>Returns the details of room with id <paramref name="id"/></returns>
        Task<RoomUserDto?> GetRoomById(int id);

        /// <summary>
        /// Updates an existing room
        /// </summary>
        /// <param name="room">New room details</param>
        /// <returns>
        /// Returns an integer (0 means that the update failed, while greater
        /// than 0 means that the room was updated successfully)
        /// </returns>
        Task<int> UpdateRoom(Room room);

        /// <summary>
        /// Deletes an existing room
        /// </summary>
        /// <param name="id">The id of the room that will be deleted</param>
        /// <returns>
        /// Returns an integer (0 means that the delete failed, while greater
        /// than 0 means that the room was deleted successfully)
        /// </returns>
        Task<int> DeleteRoom(int id);

        /// <summary>
        /// Get last room id
        /// </summary>
        /// <returns>last room id</returns>
        Task<int> GetLastRoomId();
    }
}
