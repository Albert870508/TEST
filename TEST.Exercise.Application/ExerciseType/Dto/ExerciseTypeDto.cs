using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TEST.Exercise.Application.ExerciseType.Dto
{
    public class ExerciseTypeDto
    {
        
        public long Id { get; set; }
        /// <summary>
        /// 题目大类(语文/历史/生活/影视/娱乐/其它)
        /// </summary>
        [Display(Name="类型")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 单选题分值
        /// </summary>
        [Display(Name = "单选题分值")]
        [Required]
        public long SingleScore { get; set; }

        /// <summary>
        /// 多选题分值
        /// </summary>
        [Display(Name = "多选题分值")]
        [Required]
        public long MultipleScore { get; set; }

        /// <summary>
        /// 判断题分值
        /// </summary>
        [Display(Name = "判断题分值")]
        [Required]
        public long JudgeScore { get; set; }


    }
}
