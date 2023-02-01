using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUploadViagemService
    {
        Task<IEnumerable<UploadViagem>> UploadViagens();
    }
}
