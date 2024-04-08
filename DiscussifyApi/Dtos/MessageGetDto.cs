namespace DiscussifyApi.Dtos
{
    public class MessageGetDto
    {
        public int Id { get; set; }
        public int AnonymousId { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public int Votes { get; set; }
        public string? DateTimeCreated { get; set; }
        public string? DateTimeUpdated { get; set; }
    }
}
