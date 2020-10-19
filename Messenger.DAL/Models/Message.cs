using System;


namespace Messenger.DAL.Models
{
    public class Message
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Content { get; set; }
        public MessageType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public Chat Chat { get; set; }
    }
}
