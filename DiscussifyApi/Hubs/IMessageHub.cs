namespace DiscussifyApi.Hubs 
{
    public interface IMessageHub
    {
        Task ReceiveMessage(int roomId, object message);
    }
}