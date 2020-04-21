using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TEST
{
    public interface IRepositoryWithDbContext
    {
        DbContext GetDbContext();
    }
}
