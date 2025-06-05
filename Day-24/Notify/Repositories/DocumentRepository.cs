using Microsoft.EntityFrameworkCore;
using Notify.Data;
using Notify.Interfaces;
using Notify.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notify.Repositories
{
    public class DocumentRepository : Repository<int, Document>, IDocumentRepository
    {
        public DocumentRepository(NotifyContext context) : base(context) { }

        public override async Task<Document> Get(int id)
        {
            return await _context.Documents
                .Include(d => d.UploadedBy)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public override async Task<IEnumerable<Document>> GetAll()
        {
            return await _context.Documents
                .Include(d => d.UploadedBy)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetByUploaderIdAsync(int userId)
        {
            return await _context.Documents
                .Where(d => d.UploadedById == userId)
                .ToListAsync();
        }
    }
}
