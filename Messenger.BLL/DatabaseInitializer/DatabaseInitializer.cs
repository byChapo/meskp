using Messenger.DAL.Context;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Messenger.BLL.DatabaseInitializer
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using (MessengerContext db = new MessengerContext())
            {
                db.Database.Initialize(false);
                db.SaveChanges();
            }
        }
    }
}
