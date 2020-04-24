using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TEST.Exercise.Application.Exercises.Dto
{
    public class QuestionsDto
    {
        public string Id { get; set; }
        /// <summary>
        /// 题目类型（单选/多选/判断）
        /// </summary>
        [Display(Name = "题目类型")]
        [Required]
        public string QuestionTypeId { get; set; }
        [Display(Name = "题目类型")]
        public string QuestionTypeTitle { get; set; }
        /// <summary>
        /// 题目内容(主题干，包括选项)
        /// </summary>
        [Display(Name = "题目内容")]
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 多选 单选时候的选项{ A:'',B:''}
        /// </summary>
        public string Options { get; set; }

        /// <summary>
        /// 题目正确答案
        /// </summary>
        [Display(Name = "正确答案")]
        [Required]
        public string Answer { get; set; }

        /// <summary>
        /// 答案分析
        /// </summary>
        [Display(Name = "答案分析")]
        public string AnswerNote { get; set; }

        /// <summary>
        /// 考题类型（比如：习近平新时代中国特色社会主义思想、党的十九大精神/就业创业....）
        /// </summary>
        [Display(Name = "题目类型")]
        public string QuestionTypeName { get; set; }
    }
}
