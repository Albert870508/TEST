using System;
using System.Collections.Generic;
using System.Text;

namespace TEST.Exercise.Application.AnswerRecords.Dto
{
    public class AnswerRecordDto
    {
        /// <summary>
        /// 单选题答题数量
        /// </summary>
        public int Single { get; set; }
        /// <summary>
        /// 单选题总数量
        /// </summary>
        public int SingleTotal { get; set; }
        /// <summary>
        /// 多选题答题数量
        /// </summary>
        public int Multiple { get; set; }
        /// <summary>
        /// 多选题总数量
        /// </summary>
        public int MultipleTotal { get; set; }
        /// <summary>
        /// 判断题答题数量
        /// </summary>
        public int Judge { get; set; }
        /// <summary>
        /// 判断题总数量
        /// </summary>
        public int JudgeTotal { get; set; }
        /// <summary>
        /// 填空题答题数量
        /// </summary>
        public int Completion { get; set; }
        /// <summary>
        /// 填空题总数量
        /// </summary>
        public int CompletionTotal { get; set; }
        /// <summary>
        /// 案例题答题数量
        /// </summary>
        public int Case { get; set; }
        /// <summary>
        /// 案例题总数量
        /// </summary>
        public int CaseTotal { get; set; }
        /// <summary>
        /// 简答题答题数量
        /// </summary>
        public int Simple { get; set; }
        /// <summary>
        /// 简答题总数量
        /// </summary>
        public int SimpleTotal { get; set; }
    }
}
