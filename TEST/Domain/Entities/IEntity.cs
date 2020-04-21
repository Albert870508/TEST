using System;
using System.Collections.Generic;
using System.Text;

namespace TEST.Domain.Entities
{
    /// <summary>
    /// 大多数的 <见 cref="IEntity{TPrimaryKey}"/> 主键类型都使用了 (<见 cref="long"/>)长整形.
    /// </summary>
    public interface IEntity : IEntity<long>
    {

    }

    /// <summary>
    /// 定义基础实体类型的接口。系统中的所有实体都必须实现此接口。
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体的主键类型</typeparam>
    public interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 此实体的唯一标识符。
        /// </summary>
        TPrimaryKey Id { get; set; }

        /// <summary>
        /// 数据创建时间
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// 数据库最后更改时间
        /// </summary>
        DateTime UpdateTime { get; set; }
    }
}
