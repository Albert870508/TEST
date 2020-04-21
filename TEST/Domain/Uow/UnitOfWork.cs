using Microsoft.EntityFrameworkCore;
using System;

namespace TEST.Domain.Uow
{
    /// <summary>
    /// 工作单元实现
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(IRepositoryWithDbContext context)
        {
            _dbContext = context.GetDbContext() ?? throw new ArgumentNullException(nameof(context));
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
