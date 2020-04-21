using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TEST.Domain.Entities;
using TEST.Domain.Uow;

namespace TEST.Domain.Repositories
{
    /// <summary>
    /// 定义EFCore仓储
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class EfCoreRepository<TEntity>
        : EfCoreRepository<TEntity, long>, IRepository<TEntity>
        where TEntity : class, IEntity/*, IAggregateRoot*/
    {
        public EfCoreRepository(IRepositoryWithDbContext dbDbContext) : base(dbDbContext)
        {

        }
    }

    /// <summary>
    /// 定义EFCore仓储
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class EfCoreRepository<TEntity, TPrimaryKey>
        : Repository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>/*, IAggregateRoot<TPrimaryKey>*/
    {
        private readonly DbContext _dbContext;

        public EfCoreRepository(IRepositoryWithDbContext dbDbContext)
        {
            _dbContext = dbDbContext.GetDbContext();
        }
        public virtual DbSet<TEntity> Table => _dbContext.Set<TEntity>();

        public override IQueryable<TEntity> GetAll()
        {
            return Table.AsQueryable();
        }

        public override TEntity Insert(TEntity entity)
        {
            var newEntity = Table.Add(entity).Entity;
            //_dbContext.SaveChanges();
            return newEntity;
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

            //_dbContext.SaveChanges();

            return entity;
        }

        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);

            //_dbContext.SaveChanges();
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }
        }

        public DbContext GetDbContext()
        {
            return _dbContext;
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = _dbContext.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = _dbContext.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, ((TEntity)ent.Entity).Id)
                );

            return entry?.Entity as TEntity;
        }

        #region 后加的API接口

        /// <summary>
        /// 查询是否有符合条件的数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Any(predicate);
        }

        #endregion
    }
}