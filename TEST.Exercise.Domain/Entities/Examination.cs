using System;
using System.Collections.Generic;
using System.Text;
using TEST.Domain.Entities;

namespace TEST.Exercise.Domain.Entities
{
    /// <summary>
    /// 考试表
    /// </summary>
    public class Examination : Entity
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Note { get; set; }
    }
}
