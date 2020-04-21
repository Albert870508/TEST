using Microsoft.EntityFrameworkCore;

namespace TEST.EntityFrameworkCore
{
    /// <summary>
    /// 配置数据库上下文
    /// </summary>

    public static class DbContextOptionsConfigurer
    {
        public static void Configure(DbContextOptionsBuilder dbContextOptions, string connectionString)
        {
            dbContextOptions.UseSqlServer(connectionString);
        }
    }
}
