using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.BLL.DTO;
using Messenger.BLL.Interfaces;
using Messenger.DAL.Interfaces;
using Messenger.DAL.Models;
using AutoMapper;

namespace Messenger.BLL.Services
{
    public class ChatService : IChatService
    {
        private bool disposed = false;

        public IUnitOfWork Database { get; set; }


        public ChatService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public void AddChatUser(UserToChatDTO utc)
        {
            var user = Database.Users.GetById(utc.UserId);
            Database.Chats.GetById(utc.ChatId).Participants.Add(user);
            Database.Save();
        }

        public void RemoveChatUser(UserToChatDTO utc)
        {
            User user = Database.Users.GetById(utc.UserId);
            Chat chat = Database.Chats.GetWithInclude(utc.ChatId,
                                                      c => c.Participants);
            chat.Participants.Remove(user);
            Database.Save();
        }

        public Task<int> CreateChat(ChatDTO chatDto)
        {
            return Task.Run(() => {
                int chatId;

                try
                {
                    User admin = Database.Users.GetById(chatDto.AdminId);
                    Chat chat = Mapper.Map<ChatDTO, Chat>(chatDto);
                    chat.Admin = admin;
                    chatId = Database.Chats.Create(chat);
                    Database.Save();
                }
                catch (Exception)
                {
                    return -1;
                }

                return chatId;
            });
        }

        public Task<bool> EditChat(ChatDTO chatDto)
        {
            return Task.Run(() => {                
                try
                {
                    var chat = Mapper.Map<ChatDTO, Chat>(chatDto);

                    Database.Chats.Update(chat);
                    Database.Save();
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            });
        }

        public Task ChangeChatUsersList(int chatId, string[] users)
        {
            return Task.Run(() =>
            {
                var chat = Database.Chats.GetById(chatId);
                List<User> participants = new List<User>();
                foreach(var index in users)
                    participants.Add(Database.Users.GetById(index));

                chat.Participants = participants;

                Database.Chats.Update(chat);
                Database.Save();
            });
        }

        public Task<IEnumerable<ChatDTO>> GetChats(string userId, bool privateOnly = false)
        {
            return Task.Run<IEnumerable<ChatDTO>>(() =>
            {
                var chats = Database.Users.GetWithInclude(userId,
                                                          u => u.Chats,
                                                          u => u.Chats.Select(c => c.Admin)).Chats;
                var chatsDto = Mapper.Map<IEnumerable<Chat>, List<ChatDTO>>(chats);
                if (privateOnly)
                    chatsDto = chatsDto.Where(c => c.IsPrivate).ToList();

                return chatsDto;
            });
        }

        public ChatDTO GetFullChat(int chatId)
        {
            Chat chat = Database.Chats.GetWithInclude(chatId, 
                                                      c => c.Admin,
                                                      c => c.Messages,
                                                      c => c.Participants);
            ChatDTO chatDTO = Mapper.Map<Chat, ChatDTO>(chat);

            return chatDTO;
        }

        public Task<IEnumerable<UserDTO>> GetChatParticipants(int chatId)
        {
            return Task.Run<IEnumerable<UserDTO>>(() =>
            {
                var users = Database.Chats.GetWithInclude(chatId, c => c.Participants).Participants;
                var usersDTO = Mapper.Map<ICollection<User>, List<UserDTO>>(users);

                return usersDTO;
            });            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Database.Dispose();
                }
                disposed = true;
            }
        }
        
        ~ChatService()
        {
            Dispose(false);
        }
    }
}
