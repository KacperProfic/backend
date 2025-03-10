
namespace ApplicationCore.ChatService
{
    public interface IChatUserService
    {
        void Add(string connectionId, string username);
        void RemoveByName(string username);
        string GetConnectionIdByName(string username);
        IEnumerable<(string ConnectionId, string Username)> GetAll();
    }
}