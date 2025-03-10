using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChat
{
    public class BlazorChatSampleHub : Hub
    {
        public const string HubUrl = "/chat";

        
        private static readonly List<ChatUser> _users = new List<ChatUser>();

        
        public async Task Broadcast(string username, string message)
        {
            await Clients.All.SendAsync("Broadcast", username, message);
        }

        
        public async Task AddUser(string username)
        {
            var connectionId = Context.ConnectionId;
            var user = new ChatUser
            {
                Username = username,
                ConnectionId = connectionId
            };

           
            _users.Add(user);

            
            await Clients.All.SendAsync("UpdateUserList", _users.Select(u => u.Username).ToList());

          
            await Clients.All.SendAsync("Broadcast", "[Notice]", $"{username} joined the chat.");
        }

        public async Task SendPrivateMessage(string toUsername, string message)
        {
            var fromUser = _users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var toUser = _users.FirstOrDefault(u => u.Username == toUsername);

            if (toUser != null)
            {
               
                await Clients.Client(toUser.ConnectionId).SendAsync("ReceivePrivateMessage", fromUser.Username, message);
            }
        }

        
        public override async Task OnDisconnectedAsync(Exception e)
        {
            var user = _users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                _users.Remove(user);
                
                await Clients.All.SendAsync("UpdateUserList", _users.Select(u => u.Username).ToList());
                await Clients.All.SendAsync("Broadcast", "[Notice]", $"{user.Username} left the chat.");
            }

            await base.OnDisconnectedAsync(e);
        }

       
        public class ChatUser
        {
            public string Username { get; set; }
            public string ConnectionId { get; set; }
        }
    }
}