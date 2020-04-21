namespace TEST.Domain.Entities.Auditing
{
    /// <summary>
    /// 为实例增加创建时间和创建者Id。
    /// 保存实例时 <见 cref="Entity"/> 时自动设置创建时间和创建者用户id。
    /// </summary>
    public interface ICreationAudited : IHasCreationTime
    {
        /// <summary>
        /// 添加实体的创建者id。
        /// </summary>
        long? CreatorUserId { get; set; }
    }

    /// <summary>
    /// 为实例增加创建时间和创建者。
    /// </summary>
    /// <typeparam name="TUser">用户类型</typeparam>
    public interface ICreationAudited<TUser> : ICreationAudited
        where TUser : IEntity<long>
    {
        /// <summary>
        /// 添加实体的创建者。
        /// </summary>
        TUser CreatorUser { get; set; }
    }
}
