using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Messenger.Web.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Messenger.Web.Controllers
{
    public class HomeController : Controller
    {
        ServiceUOW.ServiceUOW serviceUOW;

        public HomeController()
        {
            serviceUOW = ServiceUOW.ServiceUOW.GetInstance();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var model = await serviceUOW.UserService.GetFullUser(userId);
            var contacts = await serviceUOW.UserService.GetContacts(userId);
            var chats = await serviceUOW.UserService.GetChats(userId);

            return View(new ManageIndexViewModel { CurrentUser = model, Contacts = contacts, Chats = chats });
        }

        [HttpPost]
        public JsonResult UploadFile()
        {
            var data = SaveFile(Request.Files[0]);

            return Json(data);
        }

        public async Task<string> GetChatContent(int chatId)
        {
            var messages = await serviceUOW.MessageService.GetMessages(chatId);
            var chat_users = await serviceUOW.ChatService.GetChatParticipants(chatId);
            var combine = new { messages, chat_users };

            return JsonConvert.SerializeObject(combine, Formatting.Indented);
        }

        private dynamic SaveFile(HttpPostedFileBase fileContent)
        {
            string fileName = string.Empty;
            string type = string.Empty;

            if (fileContent != null && fileContent.ContentLength > 0)
            {
                var stream = fileContent.InputStream;
                string extension = fileContent.FileName.Split('.').Last();
                fileName = $"{Guid.NewGuid().ToString()}.{extension}";
                type = fileContent.ContentType.Split('/')[0].Equals("image") ? "Image" : "File";
                var path = Path.Combine(Server.MapPath(type.Equals("Image") ? "~/data/images" : "~/data/files"), fileName);
                using (var fileStream = System.IO.File.Create(path))
                {
                    stream.CopyTo(fileStream);
                }
            }

            return new { file_name = fileName, file_type = type };
        }
    }
}