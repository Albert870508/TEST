using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TEST
{
   public class RepositoryWithDbContext<TDbContext> : IRepositoryWithDbContext where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public RepositoryWithDbContext(TDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public DbContext GetDbContext()
        {
            return _dbContext;
        }
    }
}
