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

        // Lista przechowująca użytkowników (ConnectionId, Username)
        private static readonly List<ChatUser> _users = new List<ChatUser>();

        // Metoda broadcast, aby wysyłać wiadomości do wszystkich
        public async Task Broadcast(string username, string message)
        {
            await Clients.All.SendAsync("Broadcast", username, message);
        }

        // Metoda do dodawania użytkownika
        public async Task AddUser(string username)
        {
            var connectionId = Context.ConnectionId;
            var user = new ChatUser
            {
                Username = username,
                ConnectionId = connectionId
            };

            // Dodajemy użytkownika do listy
            _users.Add(user);

            // Informujemy wszystkich o zaktualizowanej liście użytkowników
            await Clients.All.SendAsync("UpdateUserList", _users.Select(u => u.Username).ToList());

            // Wyświetlamy informację o dołączeniu użytkownika
            await Clients.All.SendAsync("Broadcast", "[Notice]", $"{username} joined the chat.");
        }
// Metoda do wysyłania prywatnej wiadomości
        public async Task SendPrivateMessage(string toUsername, string message)
        {
            var fromUser = _users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var toUser = _users.FirstOrDefault(u => u.Username == toUsername);

            if (toUser != null)
            {
                // Jeśli odbiorca istnieje, wysyłamy mu prywatną wiadomość
                await Clients.Client(toUser.ConnectionId).SendAsync("ReceivePrivateMessage", fromUser.Username, message);
            }
        }

        // Metoda do obsługi rozłączenia
        public override async Task OnDisconnectedAsync(Exception e)
        {
            var user = _users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                _users.Remove(user);
                // Powiadamiamy wszystkich użytkowników o aktualnej liście
                await Clients.All.SendAsync("UpdateUserList", _users.Select(u => u.Username).ToList());
                await Clients.All.SendAsync("Broadcast", "[Notice]", $"{user.Username} left the chat.");
            }

            await base.OnDisconnectedAsync(e);
        }

        // Pomocnicza klasa dla użytkownika
        public class ChatUser
        {
            public string Username { get; set; }
            public string ConnectionId { get; set; }
        }
    }
}