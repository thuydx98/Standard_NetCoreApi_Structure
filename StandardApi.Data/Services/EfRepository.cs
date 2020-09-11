﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardApi.Data.Services
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly IDbContext _dbContext;
        private DbSet<T> _entities;

        protected virtual DbSet<T> Entities => _entities ?? (_entities = _dbContext.Set<T>());

        public IQueryable<T> Table => Entities;
        public IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        public EfRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ValueTask<T> GetByIdAsync(object id) => Entities.FindAsync(id);

        public async Task<bool> InsertAsync(string sql, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            return await _dbContext.ExecuteSqlCommandAsync(sql, parameters) > 0;
        }

        public async Task<bool> InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Add(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> InsertAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.AddRange(entities);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.UpdateRange(entities);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entities = await Entities.FindAsync(id);
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            return await _dbContext.ExecuteSqlCommandAsync(sql, parameters) > 0;
        }

        public Task<List<Dictionary<string, object>>> ExecuteStoredProcedureListAsync(string commandText,
            params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(commandText))
            {
                throw new ArgumentNullException(nameof(commandText));
            }

            return _dbContext.ExecuteStoredProcedureListAsync(commandText, parameters);
        }

        public string TableName => _dbContext.Model.FindEntityType(typeof(T)).GetAnnotation("Relational:TableName").Value.ToString();
    }
}
