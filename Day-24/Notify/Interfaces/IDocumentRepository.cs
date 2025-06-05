using Notify.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notify.Interfaces
{
    public interface IDocumentRepository : IRepository<int, Document>
    {
        Task<IEnumerable<Document>> GetByUploaderIdAsync(int userId);
    }
}
