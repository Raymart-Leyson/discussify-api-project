using Dapper;
using DiscussifyApi.Context;
using DiscussifyApi.Models;
using System.Data;
using System.Xml.Linq;
using DiscussifyApi.Dtos;
using Microsoft.Extensions.Hosting;

namespace DiscussifyApi.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DapperContext _context;

        public RoomRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateRoom(Room room)
        {
            var sql = "INSERT INTO [dbo].[Room] ([UserId], [Name], [Code], [DateTimeCreated], [DateTimeUpdated]) " + 
                "VALUES (@UserId, @Name, @Code, @DateTimeCreated, @DateTimeUpdated); " + 
                "SELECT SCOPE_IDENTITY()";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, new { UserId = room.UserId, Name = room.Name, Code = room.Code, DateTimeCreated = room.DateTimeCreated, DateTimeUpdated = room.DateTimeUpdated });
            }
        }

        public async  Task<int> GetRoomIdByCode(string code) 
        {
            var sql = "SELECT [Id] FROM [dbo].[Room] WHERE [Code] = @Code";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, new { Code = code });
            }
        }
        
        public async Task<IEnumerable<RoomUserDto>> GetAllRooms()
        {
            var sql = "SELECT r.Id, r.Name, r.Code, r.DateTimeCreated, r.DateTimeUpdated, u.Id FROM [dbo].[Room] as r " +
                      "LEFT JOIN [dbo].[User] as u ON r.userId = u.Id;";

            using (var connection = _context.CreateConnection())
            {
                var rooms = await connection.QueryAsync<RoomUserDto, UserDto, RoomUserDto>(sql, (room, user) =>
                {
                    room.UserId = user.Id;
                    return room;
                });

                return rooms.ToList();
            }
        }

        public async Task<RoomUserDto?> GetRoomById(int id)
        {
            var sql = "SELECT r.Id, r.Name, r.Code, r.DateTimeCreated, r.DateTimeUpdated, u.Id FROM [dbo].[Room] as r " +
                      "LEFT JOIN [dbo].[User] as u ON r.userId = u.Id " +
                      "WHERE r.Id = @Id;";

            using (var connection = _context.CreateConnection())
            {
                var rooms = await connection.QueryAsync<RoomUserDto, UserDto, RoomUserDto>(sql, (room, user) =>
                {
                    room.UserId = user.Id;

                    return room;
                }, new { Id = id });

                var result = rooms.GroupBy(room => room.Id).Select(room =>
                {
                    var groupedRoom = room.First();;

                    return groupedRoom;
                });

                return result.FirstOrDefault();
            }
        }

        public async Task<int> UpdateRoom(Room room)
        {
            var sql = "UPDATE [dbo].[Room] SET [Name] = @Name, [DateTimeUpdated] = @DateTimeUpdated WHERE [Id] = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, new { Name = room.Name, DateTimeUpdated = room.DateTimeUpdated, Id = room.Id });
            }
        }

        public async Task<int> DeleteRoom(int id)
        {
            var sql = "DELETE FROM [dbo].[Room] WHERE [Id] = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, new { Id = id });
            }
        }

        public async Task<int> GetLastRoomId()
        {
            var sql = "SELECT IDENT_CURRENT('Room')";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql);
            }
        }
    }
}

