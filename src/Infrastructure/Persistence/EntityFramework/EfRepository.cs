﻿using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EntityFramework
{
    /// <summary>
    /// Şirket olarak kullandığımız Repository içeriği. Tüm projelerimizde ortak.
    /// BaseEntity olarak tanımladığımız için sadece BaseEntity'den türemiş olan modelleri kullanabiliriz
    /// Hepsinde kullanılmasını istersek Class yapılması yeterlidir.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _entities;

        public EfRepository(DbContext context)
        {
            _context = context;
        }

        protected virtual DbSet<T> Entities => _entities ?? _context.Set<T>();

        public IQueryable<T> GetAll => Entities;

        public IQueryable<T> GetAllNoTracking => Entities.AsNoTracking();

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return Entities.Any(expression);
        }

        public int Commit()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                GetFullErrorTextAndRollbackEntityChanges(exception);
                return -1;
            }
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException exception)
            {
                GetFullErrorTextAndRollbackEntityChanges(exception);
                return -1;
            }
        }

        public int Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Entities.Remove(entity);
            return Commit();
        }

        public async Task<int> DeleteAsync(T entity)
        {
	        try
	        {
		        _context.Set<T>().Remove(entity);
		        return await _context.SaveChangesAsync();
	        }
	        catch
	        {
		        return -1;
	        }
        }

        public int DeleteBulk(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            Entities.RemoveRange(entities);
            return Commit();
        }

        public T Get(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            var query = Entities.Where(expression);
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).FirstOrDefault();
        }

        public T GetById(int Id)
        {
            return Entities.Find(Id);
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            var query = Entities.Where(expression);
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public int InsertBulk(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            Entities.AddRange(entities);
            return Commit();
        }

        public T InsertWithoutCommit(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Entities.Add(entity);
            return entity;
        }

        public int Remove(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            try
            {
                Entities.Remove(entity);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public int Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            foreach (var changeEntity in _context.ChangeTracker.Entries()
                    .Where(x => x.Entity is BaseEntity && x.State == EntityState.Modified)
                    .Select(x => x.Entity)
                    .Cast<BaseEntity>())
            {
                entity.UpdatedTime = DateTime.Now;
            }

            Entities.Update(entity);
            return Commit();
        }

        public int UpdateWithoutCommit(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                foreach (var changeEntity in _context.ChangeTracker.Entries()
                  .Where(x => x.Entity is BaseEntity && x.State == EntityState.Modified)
                  .Select(x => x.Entity)
                  .Cast<BaseEntity>())
                {
                    entity.UpdatedTime = DateTime.Now;
                }

                Entities.Update(entity);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            if (_context is DbContext dbContext)
            {
                var entries = dbContext.ChangeTracker.Entries()
                                        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry =>
                {
                    try
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    catch (InvalidOperationException)
                    {
                    }
                });
            }

            try
            {
                _context.SaveChanges();
                return exception.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}