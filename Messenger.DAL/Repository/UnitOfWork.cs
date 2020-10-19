using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.DAL.Context;
using Messenger.DAL.Interfaces;
using Messenger.DAL.Models;

namespace Messenger.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;

        private MessengerContext db;
        private ChatRepository _chatRepository;
        private MessageRepository _messageRepository;
        private MessageTypeRepository _messageTypeRepository;
        private UserRepository _userRepository;

        public UnitOfWork()
        {
            db = new MessengerContext();
        }

        public IRepository<Chat> Chats
        {
            get
            {
                if (_chatRepository == null)
                    _chatRepository = new ChatRepository(db);

                return _chatRepository;
            }
        }

        public IRepository<Message> Messages
        {
            get
            {
                if (_messageRepository == null)
                    _messageRepository = new MessageRepository(db);

                return _messageRepository;
            }
        }

        public IRepository<MessageType> MessageTypes
        {
            get
            {
                if (_messageTypeRepository == null)
                    _messageTypeRepository = new MessageTypeRepository(db);

                return _messageTypeRepository;
            }
        }

        public IRepository<User, string> Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(db);

                return _userRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
