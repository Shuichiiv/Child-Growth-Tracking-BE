﻿using DataObjects_BE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories_BE.Interfaces
{
    public interface IFeedbackRepository: IGenericRepository<Feedback>
    {
        Feedback GetFeedbackByIdIncludeProperties(Guid id);
        List<Feedback> GetFeedbacksByChildId(Guid childId);
        List<Feedback> GetFeedbackByDoctorId(Guid doctorId);
    }
}
