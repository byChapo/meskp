using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.DAL.Interfaces;
using Messenger.DAL.Context;
using Messenger.DAL.Models;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Messenger.DAL.Repository
{
    class ChatRepository : IRepository<Chat>
    {
        private MessengerContext db;

        public ChatRepository(MessengerContext context)
        {
            db = context;
        }

        public int Create(Chat chat)
        {
            db.Chats.Add(chat);
            db.SaveChanges();
            //TODO: ↑
            return chat.Id;
        }

        public void Delete(int id)
        {
            Chat chat = db.Chats.Find(id);
            if (chat != null)
                db.Chats.Remove(chat);
        }

        public IEnumerable<Chat> GetAll()
        {
            return db.Chats.ToList();
        }

        public Chat GetById(int id)
        {
            return db.Chats.Find(id);
        }

        public void Update(Chat chat)
        {
            var tempChat = GetById(chat.Id);
            tempChat.PhotoUrl = chat.PhotoUrl;
            tempChat.Title = chat.Title;

        }

        public IEnumerable<Chat> GetWithInclude(params Expression<Func<Chat, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }

        public List<Chat> GetWithInclude(Func<Chat, bool> predicate, params Expression<Func<Chat, object>>[] includeProperties)
        {
            var query = Include(includeProperties);

            return query.Where(predicate).ToList();
        }

        public Chat GetWithInclude(int id, params Expression<Func<Chat, object>>[] includeProperties)
        {
            var query = Include(includeProperties);

            return query.FirstOrDefault(q => q.Id == id);
        }

        public IQueryable<Chat> Include(params Expression<Func<Chat, object>>[] includeProperties)
        {
            IQueryable<Chat> query = db.Chats;

            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
