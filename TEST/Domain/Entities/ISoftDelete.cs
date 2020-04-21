namespace TEST.Domain.Entities
{
    /// <summary>
    /// 用于规范软删除实体。
    /// 软删除实体实际上没有被删除。
    /// 在数据库中标记为IsDeleted = true。
    /// 但无法检索到应用程序。
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 用于将实体标记为已删除。
        /// </summary>
        bool IsDeleted { get; set; }
    }
}