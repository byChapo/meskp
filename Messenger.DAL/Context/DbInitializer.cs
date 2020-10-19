using System.Data.Entity;
using Messenger.DAL.Models;

namespace Messenger.DAL.Context
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<MessengerContext>
    {
        protected override void Seed(MessengerContext db)
        {
            MessageType type1 = new MessageType { Type = "Text" };
            MessageType type2 = new MessageType { Type = "Image" };
            MessageType type3 = new MessageType { Type = "File" };

            db.MessageTypes.Add(type1);
            db.MessageTypes.Add(type2);
            db.MessageTypes.Add(type3);

            db.SaveChanges();
        }
    }
}
