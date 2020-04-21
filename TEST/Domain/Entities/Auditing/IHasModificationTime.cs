using System;

namespace TEST.Domain.Entities.Auditing
{
    /// <summary>
    /// 如果需要记录实体的最后修改时间，则实现此接口即可。
    /// 在实体 <见 cref="Entity"/> 更新到数据库时自动设置 <见 cref="LastModificationTime"/>时间 。
    /// </summary>
    public interface IHasModificationTime
    {
        /// <summary>
        /// 实体的最后更新时间。
        /// </summary>
        DateTime? LastModificationTime { get; set; }
    }
}
