﻿using DataObjects_BE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories_BE.Interfaces
{
    public interface IRatingRepository:IGenericRepository<Rating>
    {
        Rating GetRatingByIdIncludeProperties(Guid id);
        List<Rating> GetListRatingActiveOfParent(Guid parentId);
    }
}
