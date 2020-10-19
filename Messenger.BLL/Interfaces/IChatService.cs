using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.BLL.DTO;

namespace Messenger.BLL.Interfaces
{
    public interface IChatService : IService
    {
        Task<IEnumerable<ChatDTO>> GetChats(string userId, bool privateOnly = false);
        ChatDTO GetFullChat(int chatId);
        Task<IEnumerable<UserDTO>> GetChatParticipants(int chatId);
        Task<bool> EditChat(ChatDTO chatDto);
        void AddChatUser(UserToChatDTO utc);
        void RemoveChatUser(UserToChatDTO utc);
        Task<int> CreateChat(ChatDTO chatDto);
        Task ChangeChatUsersList(int chatId, string[] users);
    }
}
