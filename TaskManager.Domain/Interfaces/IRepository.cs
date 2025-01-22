﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Models;

namespace TaskManager.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        //Task<IEnumerable<TEntity>> GetAllWithPage(Pagination pagination);

        Task<long> GetAllCountAsync();

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> GetByIdAsync(long id);

        Task InsertAllAsync(IEnumerable<TEntity> entities);

        Task InsertAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task UpdateAllAsync(IEnumerable<TEntity> entities);

        Task DeleteAsync(TEntity entity);

        Task DeleteAllAsync(IEnumerable<TEntity> entities);

        Task SaveChangesAsync();
    }
}
