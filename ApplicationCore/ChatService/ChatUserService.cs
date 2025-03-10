
using ApplicationCore.Models;


namespace ApplicationCore.ChatService
{
    public class ChatUserService : IChatUserService
    {
        private readonly List<ChatUser> _users = new List<ChatUser>();

        public void Add(string connectionId, string username)
        {
            _users.Add(new ChatUser { ConnectionId = connectionId, Username = username });
        }

        public void RemoveByName(string username)
        {
            _users.RemoveAll(u => u.Username == username);
        }

        public string GetConnectionIdByName(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username)?.ConnectionId;
        }

        public IEnumerable<(string ConnectionId, string Username)> GetAll()
        {
            return _users.Select(u => (u.ConnectionId, u.Username));
        }
    }
}