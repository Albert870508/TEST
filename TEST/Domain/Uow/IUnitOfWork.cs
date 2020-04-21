using System;
using System.Collections.Generic;
using System.Text;

namespace TEST.Domain.Uow
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        int SaveChanges();
    }
}
