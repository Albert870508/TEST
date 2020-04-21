using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TEST.Domain.Repositories;
using TEST.Domain.Uow;

namespace TEST
{
    public static class TESTServiceCollectionExtensions
    {
        public static IServiceCollection AddTest<TDbContext>(this IServiceCollection services)
           where TDbContext : DbContext
        {
            services.AddScoped<IRepositoryWithDbContext, RepositoryWithDbContext<TDbContext>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
            return services;
        }
    }
}
