using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.DAL.Models;

namespace Messenger.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Chat> Chats { get; }
        IRepository<Message> Messages { get; }
        IRepository<MessageType> MessageTypes { get; }
        IRepository<User, string> Users { get; }

        void Save();
    }
}
