using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Messenger.BLL.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Messenger.Web.Models
{
    public class ManageIndexViewModel
    {
        public UserDTO CurrentUser { get; set; }
        public IEnumerable<UserDTO> Contacts { get; set; }
        public IEnumerable<ChatDTO> Chats { get; set; }
    }

    public class ChatToUpdateViewModel
    {
        public ChatDTO Chat { get; set; }
        public IList<UserDTO> Participants { get; set; }
    }
}
