using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.DAL.Models
{
    public class Chat
    {
        public Chat()
        {
            Participants = new HashSet<User>();
            Messages = new HashSet<Message>();
        }

        public int Id { get; set; }
        public User Admin { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsPrivate { get; set; }
        
        public ICollection<User> Participants { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
