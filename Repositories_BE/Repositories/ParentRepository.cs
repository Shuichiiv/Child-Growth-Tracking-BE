using DataObjects_BE;
using DataObjects_BE.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories_BE.Interfaces;

namespace Repositories_BE.Repositories
{
    public class ParentRepository: GenericRepository<Parent>, IParentRepository
    {
        private readonly SWP391G3DbContext _context;

        public ParentRepository(SWP391G3DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Parent>> GetAllParentsAsync()
        {
            return await _context.Parents.ToListAsync();
        }

        public async Task<Parent> GetParentByIdAsync(Guid parentId)
        {
            return await _context.Parents.FindAsync(parentId);
        }

        public async Task<bool> CreateParentAsync(Parent parent)
        {
            await _context.Parents.AddAsync(parent);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateParentAsync(Parent parent)
        {
            _context.Parents.Update(parent);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteParentAsync(Guid parentId)
        {
            var parent = await _context.Parents.FindAsync(parentId);
            if (parent == null) return false;

            _context.Parents.Remove(parent);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Child>> GetAllChildrenByParentIdAsync(Guid parentId)
        {
            return await _context.Childs
                .Where(c => c.ParentId == parentId)
                .ToListAsync();
        }
        public async Task<Parent> GetParentByAccountId(Guid accountId)
        {
            return await _context.Parents
                .Include(p => p.Account)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);
        }
        
        public async Task<bool> CreateParent(Parent parent)
        {
            _context.Parents.Add(parent);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}