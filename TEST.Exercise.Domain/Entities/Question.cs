using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TEST.Domain.Entities;

namespace TEST.Exercise.Domain.Entities
{
    /// <summary>
    /// 题库表
    /// </summary>
    public class Question : Entity
    {
        /// <summary>
        /// 题目类型（单选/多选/判断）
        /// </summary>
        [Display(Name = "题目类型")]
        [Required]
        [JsonConverter(typeof(IdToStringConverter))]
        public long QuestionTypeId { get; set; }
        /// <summary>
        /// 题目内容(主题干，包括选项)
        /// </summary>
        [Display(Name = "题目内容")]
        [Required]
        public string Content { get; set; }

        ///// <summary>
        ///// 选项(选项与选项之间用###隔开)
        /////   选择题为:A:安史之乱 B:开元盛世...
        /////   判断题：正确 错误
        ///// </summary>
        //[Display(Name = "选项")]
        //[Required]
        //public string Option { get; set; }

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
        /// 添加这个字段主要是为了管理界面便于取值
        /// </summary>
        [Display(Name="题目类型")]
        [Required]
        public string QuestionTypeName { get; set; }
    }

}
