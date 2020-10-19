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
    class MessageTypeRepository : IRepository<MessageType>
    {
        private MessengerContext db;

        public MessageTypeRepository(MessengerContext context)
        {
            db = context;
        }

        public int Create(MessageType messageType)
        {
            db.MessageTypes.Add(messageType);

            return messageType.Id;
        }

        public void Delete(int id)
        {
            MessageType messageType = db.MessageTypes.Find(id);
            if (messageType != null)
                db.MessageTypes.Remove(messageType);
        }

        public IEnumerable<MessageType> GetAll()
        {
            return db.MessageTypes;
        }

        public MessageType GetById(int id)
        {
            return db.MessageTypes.Find(id);
        }
        
        public void Update(MessageType messageType)
        {
            db.Entry(messageType).State = EntityState.Modified;
        }

        public IEnumerable<MessageType> GetWithInclude(params Expression<Func<MessageType, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }

        public List<MessageType> GetWithInclude(Func<MessageType, bool> predicate, params Expression<Func<MessageType, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }

        public MessageType GetWithInclude(int id, params Expression<Func<MessageType, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.FirstOrDefault(q => q.Id == id);
        }

        public IQueryable<MessageType> Include(params Expression<Func<MessageType, object>>[] includeProperties)
        {
            IQueryable<MessageType> query = db.MessageTypes;
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
