using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.BLL.DTO;
using Messenger.BLL.Identity.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Messenger.BLL.Interfaces
{
    public interface IUserService : IService
    {
        Task<IdentityResult> RegisterUser(RegisterDTO userDto, ApplicationUserManager userManager, ApplicationSignInManager signInManager);
        Task<SignInStatus> LoginUser(LoginDTO userDto, ApplicationSignInManager signInManager);
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<IEnumerable<UserDTO>> GetUsersWithEmail(string email, string currentUserId);
        Task<UserDTO> GetFullUser(string id);
        Task<bool> UpdateUser(UserDTO userDto);
        Task<IEnumerable<UserDTO>> GetContacts(string id);
        Task<bool> AddContact(UserToUserDTO userDto);
        Task<bool> DeleteContact(UserToUserDTO userDto);
        Task<IEnumerable<ChatDTO>> GetChats(string id);
    }
}
