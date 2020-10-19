using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Messenger.BLL.DTO
{
    public class ChatDTO
    {
        public int Id { get; set; }

        public string AdminId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        [MaxLength(16)]
        public string Title { get; set; }

        [DataType(DataType.ImageUrl)]
        public string PhotoUrl { get; set; }

        public bool IsPrivate { get; set; }
    }

    public class FullChatDTO : ChatDTO
    {
        public IEnumerable<UserDTO> Participants { get; set; }
        public IEnumerable<MessageDTO> Messages { get; set; }
    }
}
