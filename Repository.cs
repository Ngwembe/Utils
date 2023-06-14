using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartWatchBase.Interfaces;

namespace SmartWatchBase.Data
{
   
        public class Repository : IRepository, IDisposable
        {
            private ApplicationDbContext _context;

            public Repository(ApplicationDbContext context)
            {
                _context = context;
                _context.Database.SetCommandTimeout(180);
            }

            public IQueryable<T> Query<T>() where T : class
            {
                return _context.Set<T>().AsQueryable();
            }

            public IQueryable<T> All<T>() where T : class
            {
                return _context.Set<T>().AsQueryable();
            }

            public IQueryable<T> All<T>(params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties) where T : class
            {
                DbSet<T> query = _context.Set<T>();
                foreach (var includeProperty in includeProperties)
                {
                    query.Include(includeProperty);
                }
                return query.AsQueryable();
            }

            public T Find<T>(int id) where T : class
            {
                return _context.Set<T>().Find(id);
            }

            public IQueryable<T> Search<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties) where T : class
            {
                try
                {
                    DbSet<T> query = _context.Set<T>();

                    foreach (var includeProperty in includeProperties)
                    {
                        query.Include(includeProperty);
                    }

                    return query.Where(predicate).AsQueryable();
                }
                catch
                {
                    throw;
                }
            }

            public async Task<List<T>> SearchAsync<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties) where T : class
            {
                var query = Query<T>();

                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }

                return await query.Where(predicate).ToListAsync();
            }

            public int Count<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
            {
                try
                {
                    DbSet<T> query = _context.Set<T>();

                    return query.Where(predicate).Count();
                }
                catch
                {
                    throw;
                }
            }

            public async Task<long> CountAsync<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
            {
                var query = Query<T>();

                return await query.Where(predicate).LongCountAsync();
            }

            public async Task<T> AddEntityAsync<T>(T entity) where T : class
            {
                _context.Entry(entity).State = EntityState.Detached;
                _context.Set<T>().Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }

            public async Task<T> UpdateEntityAsync<T>(T entity) where T : class
            {
                _context.Set<T>().Attach(entity);

                _context.Entry(entity).State = EntityState.Modified;
                _context.Entry(entity).State = EntityState.Detached;
                await _context.SaveChangesAsync();
                return entity;
            }

            public T AddEntity<T>(T entity) where T : class
            {
                try
                {
                    _context.Set<T>().Add(entity);
                    _context.Entry(entity).State = EntityState.Added;
                    return entity;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public T UpdateEntity<T>(T entity) where T : class
            {
                _context.Entry(entity).State = EntityState.Modified;
                return entity;
            }

            public void DeleteEntity<T>(T entity) where T : class
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }

            public async Task SaveAsync()
            {
                await _context.SaveChangesAsync();
            }

            public void Save()
            {
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            //public void SaveAll()
            //{
            //    try
            //    {
            //        _context.SaveAllChanges();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //}

            public void Detach<T>(T source) where T : class
            {
                _context.Entry(source).State = EntityState.Detached;
            }

            public IQueryable<T> CloneEntity<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties) where T : class
            {
                try
                {
                    DbSet<T> query = _context.Set<T>();

                    foreach (var includeProperty in includeProperties)
                    {
                        query.Include(includeProperty);
                    }

                    return query.Where(predicate).AsQueryable();
                }
                catch
                {
                    throw;
                }
            }

            //public IDbContextTransaction StartTransaction()
            //{
            //    var _transaction = _context.Database.BeginTransaction();
            //    return _transaction;
            //}

            //public void Commit(IDbContextTransaction transaction)
            //{
            //    transaction.Commit();
            //}

            //public void Rollback(IDbContextTransaction transaction)
            //{
            //    transaction.Rollback();
            //}

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }

                GC.SuppressFinalize(this);
            }
        }
}
