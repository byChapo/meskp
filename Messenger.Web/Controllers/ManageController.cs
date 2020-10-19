using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Messenger.Web.Models;
using Messenger.BLL.Identity.Managers;
using Messenger.BLL.DTO;
using Newtonsoft.Json;

namespace Messenger.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        ServiceUOW.ServiceUOW serviceUOW; 

        public ManageController()
        {
            serviceUOW = ServiceUOW.ServiceUOW.GetInstance();
        }

        [HttpGet]
        public ActionResult ContactPartial()
        {
            return PartialView("ContactPartial");
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var model = await serviceUOW.UserService.GetFullUser(userId);
            var contacts = await serviceUOW.UserService.GetContacts(userId);
            var chats = await serviceUOW.ChatService.GetChats(userId);

            return View(new ManageIndexViewModel { CurrentUser = model, Contacts = contacts, Chats = chats.Where(c => c.AdminId == User.Identity.GetUserId()) });
        }

        //
        // GET: /Manage/GetUsersByEmail
        public async Task<string> GetUsersByEmail(string email)
        {
            var contacts = await serviceUOW.UserService.GetContacts(User.Identity.GetUserId());
            var users = await serviceUOW.UserService.GetUsersWithEmail(email, User.Identity.GetUserId());

            return JsonConvert.SerializeObject(users.Except(contacts), Formatting.Indented);
        }

        //
        // GET: /Manage/GetUserContacts
        public async Task<string> GetUserContacts()
        {
            var contacts = await serviceUOW.UserService.GetContacts(User.Identity.GetUserId());

            return JsonConvert.SerializeObject(contacts, Formatting.Indented);
        }

        //
        // GET: /Manage/AddContact
        [HttpPost]
        public async Task<string> AddContact(string contactId)
        {
            bool result = await serviceUOW.UserService.AddContact(new UserToUserDTO
            {
                FirstUserId = User.Identity.GetUserId(),
                SecondUserId = contactId
            });

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        //
        // GET: /Manage/DeleteContact
        [HttpPost]
        public async Task<string> DeleteContact(string contactId)
        {
            bool result = await serviceUOW.UserService.DeleteContact(new UserToUserDTO
            {
                FirstUserId = User.Identity.GetUserId(),
                SecondUserId = contactId
            });

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        //
        // GET: /Manage/GetUserContacts
        public async Task<ActionResult> UpdateUser()
        {
            var user = await serviceUOW.UserService.GetFullUser(User.Identity.GetUserId());            

            return View(user);
        }

        //
        // GET: /Manage/GetUserContacts
        [HttpPost]
        public async Task<ActionResult> UpdateUser(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                var isComplete = await serviceUOW.UserService.UpdateUser(userDTO);
                if (isComplete)
                {
                    return RedirectToAction("Index", "Manage");
                }
            }            

            return View(userDTO);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        //#endregion
    }
}