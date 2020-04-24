using System;
using System.Collections.Generic;
using System.Text;
using TEST.Domain.Entities;

namespace TEST.Exercise.Domain.Entities
{
    /// <summary>
    /// 答题记录表
    /// </summary>
    public class AnswerRecord : Entity
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 已答试题编号
        /// </summary>
        public long QuestionId { get; set; }
    }
}
