using Dapper;
using DiscussifyApi.Context;
using DiscussifyApi.Models;
using DiscussifyApi.Dtos;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace DiscussifyApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateUser(User user)
        {
            var sql = "INSERT INTO [dbo].[User] ([EmailAddress], [Password], [FirstName], [LastName],  [DateTimeCreated]) " +
                      "VALUES (@EmailAddress, @Password, @FirstName, @LastName,  @DateTimeCreated);" +
                      "SELECT SCOPE_IDENTITY();";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, user);
            }
        }

        public async Task<int> CheckIfUserExists(string emailAddress)
        {
            var sql = "SELECT * FROM [dbo].[User] WHERE [EmailAddress] = @EmailAddress;";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryFirstOrDefaultAsync<int>(sql, new { EmailAddress = emailAddress });
            }
        }

        public async Task<UserDto> GetUserByEmail(string emailAddress)
        {
            var sql = "SELECT * FROM [dbo].[User] WHERE [EmailAddress] = @EmailAddress";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<UserDto>(sql, new { EmailAddress = emailAddress });
            }
        }

        public async Task<string> CheckUserEmailPasswordExists(string email)
        {
            // get the password by email
            var sql = "SELECT [Password] FROM [dbo].[User] WHERE [EmailAddress] = @EmailAddress;";
            using (var con = _context.CreateConnection())
            {
                return await con.QueryFirstOrDefaultAsync<string>(sql, new { EmailAddress = email });
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var sql = "SELECT * FROM [dbo].[User];";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryAsync<UserDto>(sql);
            }
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var sql = "SELECT * FROM [dbo].[User] WHERE [Id] = @Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<UserDto>(sql, new { Id = id });
            }
        }

        public async Task<UserRoomDto?> GetUserRoomsById(int id)
        {
            var sql = "SELECT u.*, r.Id, r.Name, r.Code, r.DateTimeCreated, r.DateTimeUpdated FROM [dbo].[User] as u " +
                      "LEFT JOIN [dbo].[Room] as r ON u.Id = r.userId " +
                      "WHERE u.Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<UserRoomDto, RoomDto, UserRoomDto>(sql, (user, room) =>
                {

                    user.Rooms.Add(room);
                    return user;

                }, new { id });

                var result = users.GroupBy(user => user.Id).Select(user =>
                {
                    var groupedUser = user.First();
                    groupedUser.Rooms = user.SelectMany(user => user.Rooms)
                                            .Where(room => room != null)
                                            .ToList();
                    return groupedUser;
                });

                return result.SingleOrDefault();
            }
        }
    }
}

