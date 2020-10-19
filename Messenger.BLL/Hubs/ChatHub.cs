using Messenger.DAL.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.BLL.Services;
using Messenger.DAL.Repository;
using Messenger.BLL.DTO;

namespace Messenger.BLL.Hubs
{
    public class ChatHub : Hub
    {
        private MessageService messageService;

        public ChatHub()
        {
            messageService = new MessageService(new UnitOfWork());
        }
        
        public async Task Send(dynamic message, string roomName)
        {
            MessageDTO messageDTO = new MessageDTO
            {
                Author = message.authorId,
                Content = message.Content,
                CreatedAt = DateTime.Now,
                Type = message.Type,
                ChatId = int.Parse(roomName)
            };
            messageDTO = await messageService.SendMessage(messageDTO);
            message.CreatedAt = messageDTO.CreatedAt;
            message.Author = messageDTO.AuthorName;
            message.AuthorId = messageDTO.Author;

            Clients.Group(roomName).addChatMessage(message);
        }

        public async Task Connect(string roomName)
        {
            await Groups.Add(Context.ConnectionId, roomName);
            Clients.Group(roomName, Context.ConnectionId).addInfoMessage(Context.User.Identity.Name + " joined.");
        }

        public async Task Disconnect(string roomName)
        {
            await Groups.Remove(Context.ConnectionId, roomName);
            Clients.Group(roomName, Context.ConnectionId).addInfoMessage(Context.User.Identity.Name + " disconnected.");
        }
    }
}
