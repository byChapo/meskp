using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.BLL.DTO;
using Messenger.BLL.Interfaces;
using Messenger.DAL.Interfaces;
using Messenger.DAL.Models;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Messenger.BLL.Identity.Managers;
using System.Text.RegularExpressions;

namespace Messenger.BLL.Services
{
    public class UserService : IUserService
    {
        private bool disposed = false;
        
        public IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }
        
        public Task<bool> AddContact(UserToUserDTO userDto)
        {
            return Task.Run(() => {
                try
                {
                    User firstUser = Database.Users.GetById(userDto.FirstUserId);
                    User secondUser = Database.Users.GetById(userDto.SecondUserId);
                    firstUser.Contacts.Add(secondUser);
                    secondUser.Contacts.Add(firstUser);
                    Database.Save();
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            });
        }

        public Task<bool> DeleteContact(UserToUserDTO userDto)
        {
            return Task.Run(() => {
                try
                {
                    User firstUser = Database.Users.GetWithInclude(userDto.FirstUserId, u => u.Contacts);
                    User secondUser = Database.Users.GetWithInclude(userDto.SecondUserId, u => u.Contacts);
                    firstUser.Contacts.Remove(secondUser);
                    secondUser.Contacts.Remove(firstUser);
                    Database.Save();
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            });            
        }

        public Task<IEnumerable<UserDTO>> GetContacts(string id)
        {
            return Task.Run<IEnumerable<UserDTO>>(() =>
            {
                var usersDTO = new List<UserDTO>();
                if (id != null)
                {
                    var users = Database.Users.GetWithInclude(id, u => u.Contacts).Contacts;
                    usersDTO = Mapper.Map<IEnumerable<User>, List<UserDTO>>(users);
                }

                return usersDTO;
            });
        }

        public Task<UserDTO> GetFullUser(string id)
        {
            return Task.Run(() => {
                var user = Database.Users.GetById(id);
                var userDTO = Mapper.Map<User, UserDTO>(user);

                return userDTO;
            });
        }

        public Task<IEnumerable<UserDTO>> GetUsers()
        {
            return Task.Run(() =>
            {
                var users = Database.Users.GetAll();
                var usersDTO = Mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users);

                return usersDTO;
            });
        }

        public Task<IEnumerable<UserDTO>> GetUsersWithEmail(string email, string currentUserId)
        {
            return Task.Run(() =>
            {
                var users = Database.Users.GetAll();
                var usersDTO = Mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users);
                usersDTO = usersDTO.Where(u => u.Id != currentUserId);
                if (!string.IsNullOrWhiteSpace(email))
                    usersDTO = usersDTO.Where(u => Regex.IsMatch(u.Email.ToLower(), email.ToLower())).ToList();

                return usersDTO;
            });
        }

        public async Task<IdentityResult> RegisterUser(RegisterDTO userDto, ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            var user = Mapper.Map<RegisterDTO, User>(userDto);
            user.UserName = userDto.Email;

            var result = await userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return result;
        }

        public async Task<SignInStatus> LoginUser(LoginDTO userDto, ApplicationSignInManager signInManager)
        {
            var result = await signInManager.PasswordSignInAsync(userDto.Email, userDto.Password, userDto.RememberMe, shouldLockout: false);           

            return result;
        }

        public Task<IEnumerable<ChatDTO>> GetChats(string id)
        {
            return Task.Run<IEnumerable<ChatDTO>>(() =>
            {
                var chatsDTO = new List<ChatDTO>();
                if (id != null)
                {
                    var chats = Database.Users.GetWithInclude(id, u => u.Chats).Chats;
                    chatsDTO = Mapper.Map<IEnumerable<Chat>, List<ChatDTO>>(chats);
                }

                return chatsDTO;
            });
        }

        public Task<bool> UpdateUser(UserDTO userDto)
        {
            return Task.Run(() =>
            {
                try
                {
                    var user = Mapper.Map<UserDTO, User>(userDto);
                    
                    Database.Users.Update(user);
                    Database.Save();
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Database.Dispose();
                }
                disposed = true;
            }
        }

        ~UserService()
        {
            Dispose(false);
        }
    }
}
