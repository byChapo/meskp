using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace Messenger.DAL.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Chats = new HashSet<Chat>();
            Messages = new HashSet<Message>();
            Contacts = new HashSet<User>();
        }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public ICollection<Chat> Chats { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<User> Contacts { get; set; }

        public string GetFullName() => $"{FirstName} {LastName}";


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            return userIdentity;
        }
    }
}
