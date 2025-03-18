using DataObjects_BE;
using DataObjects_BE.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories_BE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories_BE.Repositories
{
    public class FeedbackRepository: GenericRepository<Feedback>, IFeedbackRepository
    {
        private readonly SWP391G3DbContext _context;
        public FeedbackRepository(SWP391G3DbContext context): base(context) 
        {
            _context = context;
        }
        public Feedback GetFeedbackByIdIncludeProperties(Guid id)
        {
            var feedback = _context.Feedbacks.Include(x=>x.Report).
                Include(x=>x.Doctor)
                .Include(x=> x.Ratings)
                .FirstOrDefault(x=>x.FeedbackId== id);
            return feedback;
        }
        public List<Feedback> GetFeedbacksByChildId(Guid childId)
        {
            return _context.Feedbacks
                .Include(f => f.Report) 
                .Where(f => f.Report.ChildId == childId)
                .OrderByDescending(f => f.FeedbackCreateDate)
                .ToList();
        }
    }
}
