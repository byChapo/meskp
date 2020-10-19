using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.BLL.DTO;

namespace Messenger.BLL.Interfaces
{
    public interface IMessageService : IService
    {
        Task<MessageDTO> SendMessage(MessageDTO message);
        Task<IEnumerable<MessageDTO>> GetMessages(int chatId);
        void DeleteMessage(int messageId);
    }
}
