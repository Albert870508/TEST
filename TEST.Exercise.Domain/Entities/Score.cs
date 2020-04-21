using System;
using System.Collections.Generic;
using System.Text;
using TEST.Domain.Entities;

namespace TEST.Exercise.Domain.Entities
{
    /// <summary>
    /// 成绩表
    /// </summary>
    public class Score:Entity
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 考试编号
        /// </summary>
        public long ExaminationId { get; set; }

        /// <summary>
        /// 题目编号+用户输入答案
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 总得分
        /// </summary>
        public double TotalScore { get; set; }
    }
}
