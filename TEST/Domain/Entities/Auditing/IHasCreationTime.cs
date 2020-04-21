using System;

namespace TEST.Domain.Entities.Auditing
{
    /// <summary>
    /// 如果需要记录实体的添加时间，则实现此接口即可。
    /// 在实体 <见 cref="Entity"/> 插入到数据库时自动设置 <见 cref="CreationTime"/>时间 。
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// 实体的添加时间。
        /// </summary>
        DateTime CreationTime { get; set; }
    }
}