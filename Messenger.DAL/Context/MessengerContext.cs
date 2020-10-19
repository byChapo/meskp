using System.Data.Entity;
using Messenger.DAL.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Messenger.DAL.Context
{
    public class MessengerContext : IdentityDbContext<User>
    {
        static MessengerContext()
        {
            Database.SetInitializer(new DbInitializer());
        }

        public MessengerContext()
            : base("MessengerDbConnection")
        { }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageType> MessageTypes { get; set; }

        
        public static MessengerContext Create()
        {
            return new MessengerContext();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin>().HasKey(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Entity<Chat>()
                        .HasMany(c => c.Participants)
                        .WithMany(u => u.Chats)
                        .Map(cu =>
                        {
                            cu.MapLeftKey("Chat_Id");
                            cu.MapRightKey("Participant_Id");
                            cu.ToTable("ChatToParticipant");
                        });

            modelBuilder.Entity<User>()
                        .HasMany(u => u.Contacts)
                        .WithMany()
                        .Map(u =>
                        {
                            u.MapLeftKey("User_Id");
                            u.MapRightKey("Contact_Id");
                            u.ToTable("UserToContact");
                        });
        }
    }
}
