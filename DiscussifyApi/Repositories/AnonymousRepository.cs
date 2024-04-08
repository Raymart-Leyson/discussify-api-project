using Dapper;
using DiscussifyApi.Context;
using DiscussifyApi.Models;
using System.Data;
using System.Xml.Linq;
using DiscussifyApi.Dtos;
using Microsoft.Extensions.Hosting;

namespace DiscussifyApi.Repositories
{
    public class AnonymousRepository : IAnonymousRepository
    {
        private readonly DapperContext _context;

        public AnonymousRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAnonymous(Anonymous anonymous)
        {
            var sql = "INSERT INTO [dbo].[Anonymous] ([RoomId], [Name], [DateTimeCreated]) " + 
                "VALUES (@RoomId, @Name, @DateTimeCreated); " + 
                "SELECT SCOPE_IDENTITY()";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, new { RoomId = anonymous.RoomId, Name = anonymous.Name, DateTimeCreated = anonymous.DateTimeCreated });
            }
        }

        public async Task<Anonymous> GetAnonymousById(int id)
        {
            var sql = "SELECT * FROM [dbo].[Anonymous] WHERE [Id] = @Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<Anonymous>(sql, new { Id = id });
            }
        }
    }
}