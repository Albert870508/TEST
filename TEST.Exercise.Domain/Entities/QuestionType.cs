using System.ComponentModel.DataAnnotations;
using TEST.Domain.Entities;

namespace TEST.Exercise.Domain.Entities
{
    /// <summary>
    /// 题库类型表
    /// </summary>
    public class QuestionType : Entity
    {
        /// <summary>
        /// 类型名称（单选题/多选题/判断题）
        /// </summary>
        [Display(Name = "类型")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 类型分值
        /// </summary>
        [Display(Name = "分值")]
        [Required]
        public double Score { get; set; }

        /// <summary>
        /// 类型数量
        /// </summary>
        [Display(Name = "数量")]
        [Required]        
        public int Number { get; set; }
        
    }
    
}
