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
    class MessageRepository : IRepository<Message>
    {
        private MessengerContext db;

        public MessageRepository(MessengerContext context)
        {
            db = context;
        }

        public int Create(Message message)
        {
            db.Messages.Add(message);

            return message.Id;
        }

        public void Delete(int id)
        {
            Message message = db.Messages.Find(id);
            if (message != null)
                db.Messages.Remove(message);
        }

        public IEnumerable<Message> GetAll()
        {
            return db.Messages.ToList();
        }

        public Message GetById(int id)
        {
            return db.Messages.Find(id);
        }

        public void Update(Message message)
        {
            db.Entry(message).State = EntityState.Modified;
        }

        public IEnumerable<Message> GetWithInclude(params Expression<Func<Message, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }

        public List<Message> GetWithInclude(Func<Message, bool> predicate, params Expression<Func<Message, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }

        public Message GetWithInclude(int id, params Expression<Func<Message, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.FirstOrDefault(q => q.Id == id);
        }

        public IQueryable<Message> Include(params Expression<Func<Message, object>>[] includeProperties)
        {
            IQueryable<Message> query = db.Messages;
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
