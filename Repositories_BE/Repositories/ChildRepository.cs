using DataObjects_BE;
using DataObjects_BE.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories_BE.Interfaces;

namespace Repositories_BE.Repositories
{

    public class ChildRepository :  GenericRepository<Child>, IChildRepository
    {
        private readonly SWP391G3DbContext _context;

        public ChildRepository(SWP391G3DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Child>> GetAllChildrenAsync()
        {
            return await _context.Childs.ToListAsync();
        }

        public async Task<Child> GetChildByIdAsync(Guid childId)
        {
            return await _context.Childs.FindAsync(childId);
        }

        public async Task<bool> CreateChildAsync(Child child)
        {
            await _context.Childs.AddAsync(child);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateChildAsync(Child child)
        {
            _context.Childs.Update(child);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteChildAsync(Guid childId)
        {
            var child = await _context.Childs.FindAsync(childId);
            if (child == null) return false;

            _context.Childs.Remove(child);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}