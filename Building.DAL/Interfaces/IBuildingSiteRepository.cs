﻿using Building.Domain.Entity;

namespace Building.DAL.Interfaces
{
    public interface IBuildingSiteRepository
    {
        Task<BuildingSite> Get(int? id);
        Task<IEnumerable<BuildingSite>> GetAll();
    }
}
