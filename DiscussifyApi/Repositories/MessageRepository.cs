using DiscussifyApi.Context;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using Dapper;

namespace DiscussifyApi.Repositories
{
    public class MessageRepository: IMessageRepository
    {
        private readonly DapperContext _context;

        public MessageRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateMessage(Message message)
        {
            var sql = "INSERT INTO [dbo].[Message] ([AnonymousId], [Content], [DateTimeCreated], [DateTimeUpdated]) " +
                      "VALUES (@AnonymousId, @Content, @DateTimeCreated, @DateTimeUpdated); " +
                      "SELECT SCOPE_IDENTITY()";

            using (var con = _context.CreateConnection()) 
            {
                return await con.ExecuteScalarAsync<int>(sql, message);
            }
        }

        public async Task<IEnumerable<MessageGetDto>> GetAllMessages(int id)
        {
            var sql = "SELECT a.Name, m.* FROM [dbo].[Room] as r " +
                "RIGHT JOIN [dbo].[Anonymous] as a ON a.RoomId = r.Id " +
                "RIGHT JOIN [dbo].[Message] as m ON m.AnonymousId = a.Id " +
                "WHERE r.Id = @id ORDER BY m.DateTimeCreated;";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryAsync<MessageGetDto>(sql, new { id });
            }
        }

        public async Task<MessageGetDto> GetMessageById(int id)
        {
            var sql = "SELECT a.Name, m.* FROM [dbo].[Message] as m " +
                      "RIGHT JOIN [dbo].[Anonymous] as a ON a.Id = m.AnonymousId " +
                      "WHERE m.Id = @Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<MessageGetDto>(sql, new { Id = id });
            }
        }

        public async Task<int> UpdateMessage(Message message)
        {
            var sql = "UPDATE [dbo].[Message] SET [Content] = @Content, [DateTimeUpdated] = @DateTimeUpdated WHERE [Id] = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteAsync(
                    sql,
                    new
                    {
                        Id = message.Id,
                        Content = message.Content,
                        DateTimeUpdated = message.DateTimeUpdated,
                    }
                );
            }
        }

        public async Task<MessageGetDto> UpvoteMessage(int id, int increment) 
        {
            var sql = "UPDATE [dbo].[Message] SET [Votes] = [Votes] + @Increment WHERE [Id] = @Id; " +
                      "SELECT a.Name, m.* FROM [dbo].[Message] as m " +
                      "RIGHT JOIN [dbo].[Anonymous] as a ON a.Id = m.AnonymousId " +
                      "WHERE m.Id = @Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<MessageGetDto>(sql, new { Id = id, Increment = increment });
            }
        }

        public async Task<MessageGetDto> DownvoteMessage(int id, int decrement)
        {
            var sql = "UPDATE [dbo].[Message] SET [Votes] = [Votes] - @Decrement WHERE [Id] = @Id; " +
                      "SELECT a.Name, m.* FROM [dbo].[Message] as m " +
                      "RIGHT JOIN [dbo].[Anonymous] as a ON a.Id = m.AnonymousId " +
                      "WHERE m.Id = @Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<MessageGetDto>(sql, new { Id = id, Decrement = decrement });
            }
        }

        public async Task<int> DeleteMessage(int id)
        {
            var sql = "DELETE FROM [dbo].[Message] WHERE [Id] = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
