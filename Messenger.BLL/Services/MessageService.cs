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
    public class MessageService : IMessageService
    {
        private bool disposed = false;

        public IUnitOfWork Database { get; set; }


        public MessageService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public void DeleteMessage(int messageId)
        {
            Database.Messages.Delete(messageId);
            Database.Save();
        }

        public Task<IEnumerable<MessageDTO>> GetMessages(int chatId)
        {
            return Task.Run<IEnumerable<MessageDTO>>(() =>
            {
                var messages = Database.Chats.GetWithInclude(chatId, c => c.Messages,
                                                                 c => c.Messages.Select(m => m.Type),
                                                                 c => c.Messages.Select(m => m.Author)).Messages;
                var messagesDto = Mapper.Map<IEnumerable<Message>, List<MessageDTO>>(messages);

                return messagesDto;
            });
        }

        public Task<MessageDTO> SendMessage(MessageDTO messageDto)
        {
            return Task.Run(() => {
                var message = Mapper.Map<MessageDTO, Message>(messageDto);
                message.Author = Database.Users.GetById(messageDto.Author);
                message.Chat = Database.Chats.GetById(messageDto.ChatId);
                message.Type = Database.MessageTypes.GetWithInclude(mt => mt.Type == messageDto.Type)[0];

                Database.Messages.Create(message);
                Database.Save();

                messageDto.AuthorName = message.Author.GetFullName();

                return messageDto;
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

        ~MessageService()
        {
            Dispose(false);
        }
    }
}
