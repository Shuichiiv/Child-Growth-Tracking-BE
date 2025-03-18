using DataObjects_BE;
using DataObjects_BE.Entities;
using DTOs_BE.UserDTOs;
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

        public async Task<DateTime?> GetChildDOB(Guid childId)
        {
            return await _context.Childs
                .Where(c => c.ChildId == childId)
                .Select(c => (DateTime?)c.DOB)
                .FirstOrDefaultAsync();
        }

        public async Task<ParentDto2> GetParentByChildIdAsync1(Guid childId)
        {
            var child = await _context.Childs
                .Include(c => c.Parent)
                .ThenInclude(p => p.Account)
                .FirstOrDefaultAsync(c => c.ChildId == childId);
            
            if (child == null || child.Parent == null) return null;

            return new ParentDto2
            {
                ParentId = child.Parent.ParentId,
                AccountId = child.Parent.AccountId,
                FirstName = child.Parent.Account.FirstName,
                LastName = child.Parent.Account.LastName,
                Email = child.Parent.Account.Email,
                PhoneNumber = child.Parent.Account.PhoneNumber
            };
        }

        public async Task<ChildDto> GetChildByIdAsync1(Guid childId)
        {
            var child = await _context.Childs
                .FirstOrDefaultAsync(c => c.ChildId == childId);

            if (child == null) return null;

            return new ChildDto
            {
                ChildId = child.ChildId,
                ParentId = child.ParentId,
                FirstName = child.FirstName,
                LastName = child.LastName,
                Gender = child.Gender,
                DOB = child.DOB,
                ImageUrl = child.ImageUrl
            };
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
        public async Task<IEnumerable<Child>> SearchChildrenAsync(Guid parentId, string keyword)
        {
            return await _context.Childs
                .Where(c => c.ParentId == parentId &&
                            (string.IsNullOrEmpty(keyword) || c.FirstName.Contains(keyword)))
                .ToListAsync();
        }
        public async Task<Child> GetChildByIdAndParentAsync(Guid childId, Guid parentId)
        {
            return await _context.Childs
                .Where(c => c.ChildId == childId && c.ParentId == parentId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteChildAsync(Guid childId)
        {
            var child = await _context.Childs.FindAsync(childId);
            if (child == null) return false;

            _context.Childs.Remove(child);
            return await _context.SaveChangesAsync() > 0;
        }
    }
    
    public class ParentDto2
    {
        public Guid ParentId { get; set; }
        public Guid AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}