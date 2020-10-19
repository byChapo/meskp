using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.DAL.Interfaces;

namespace Messenger.BLL.Interfaces
{
    public interface IService : IDisposable
    {
        IUnitOfWork Database { get; set; }
    }
}
