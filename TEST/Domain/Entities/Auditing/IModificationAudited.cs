namespace TEST.Domain.Entities.Auditing
{
    /// <summary>
    /// 为实例增加最后修改时间和最后修改者Id。
    /// 在更新实体 <见 cref="IEntity"/> 时更新最后修改时间和最后修改者Id。
    /// </summary>
    public interface IModificationAudited : IHasModificationTime
    {
        /// <summary>
        /// 最后修改实体的用户Id。
        /// </summary>
        long? LastModifierUserId { get; set; }
    }

    /// <summary>
    /// 为实例增加最后修改时间和最后修改者。
    /// </summary>
    /// <typeparam name="TUser">用户类型</typeparam>
    public interface IModificationAudited<TUser> : IModificationAudited
        where TUser : IEntity<long>
    {
        /// <summary>
        /// 最后修改实体的用户。
        /// </summary>
        TUser LastModifierUser { get; set; }
    }
}
