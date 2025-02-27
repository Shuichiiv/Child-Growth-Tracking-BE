using DataObjects_BE;
using DataObjects_BE.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories_BE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories_BE.Repositories
{
    public class RatingRepository: GenericRepository<Rating>, IRatingRepository
    {
        private readonly SWP391G3DbContext _context;
        public RatingRepository(SWP391G3DbContext context): base(context)
        {
            _context = context;
        }
        public Rating GetRatingByIdIncludeProperties(Guid id)
        {
            var rating = _context.Ratings.Include(x=>x.Feedback).
                Include(x=>x.Parent).
                FirstOrDefault(x=>x.RatingId==id);
            return rating;
        }
        public List<Rating> GetListRatingActiveOfParent(Guid parentId)
        {
           var list = _context.Ratings
                .Where(x=>x.ParentId==parentId && x.IsActive==true)
                .OrderByDescending(x=>x.RatingDate)
                .ToList();
            return list;
        }
    }
}
