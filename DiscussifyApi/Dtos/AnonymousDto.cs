namespace DiscussifyApi.Dtos
{
    public class AnonymousDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string? Name { get; set; }
        public string? RefreshToken { get; set; }
        public string? AccessToken { get; set; }
    }
}
