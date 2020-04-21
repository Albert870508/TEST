using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TEST.Domain.Entities.Snowflake;

namespace TEST.Domain.Entities
{


    /// <summary>
    /// 大多数的 <see cref="Entity{TPrimaryKey}"/> 主键类型都使用了(<see cref="long"/>)长整形。
    /// </summary>
    [Serializable]
    public abstract class Entity : Entity<long>, IEntity
    {
        #region 雪花ID

        // 集群编号 = 1
        private static long DatacenterId = 1;

        // 服务器编号 = 1
        private static long WorkerId = 1;

        // 初始化雪花算法
        private static IdWorker worker = new IdWorker(WorkerId, DatacenterId);

        #endregion

        public Entity()
        {
            // 默认ID
            this.Id = worker.NextId();
        }
    }

    /// <summary>
    /// Basic implementation of IEntity interface.
    /// An entity can inherit this class of directly implement to IEntity interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [JsonConverter(typeof(IdToStringConverter))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual TPrimaryKey Id { get; set; }

        /// <summary>
        /// 数据创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 数据库最后更改时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        
    }
}
