namespace ApplicationCore.Models
{
    public class ChatUser : User
    {
        public string ConnectionId { get; set; }
    }

    public partial class User
    {
        public string Username { get; set; }
    }
}