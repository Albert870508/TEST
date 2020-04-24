using System;
using System.Collections.Generic;
using System.Text;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.Examinations.Dto
{
    /// <summary>
    /// 考试类输出参数
    /// </summary>
    public class ExaminationOutPut
    {
        /// <summary>
        /// 返回考试编号
        /// </summary>
        public string ExaminationId { get; set; }
        /// <summary>
        /// 返回考试内容（题目和类型）
        /// </summary>
        public List<QuestionStringId> QuestionItems { get; set; }

    }

    public class QuestionStringId : Question
    {
        public string StrId { get; set; }

        public string QuestionType { get; set; }

        public string MyAnswer { get; set; }
    }

    public class QuestionItem
    {
        /// <summary>
        /// 返回的试题编号
        /// </summary>
        public string QuestionItemId { get; set; }
        /// <summary>
        /// 返回的试题内容
        /// </summary>
        public string QuestionItemContent { get; set; }
        /// <summary>
        /// 返回试题类型
        /// </summary>
        public string QuestionItemType { get; set; }
    }
    /// <summary>
    /// 考试类输入参数
    /// </summary>
    public class ExaminationIntput
    {
        /// <summary>
        /// 考试编号
        /// </summary>
        public string ExaminationId { get; set; }

        public List<QuestionAndInputAnswer> questionAndInputAnswers { get; set; }

    }

    public class TestEndOutput
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Toal { get; set; }

        public int Error { get; set; }

        public double Score { get; set; }

        public List<QuestionStringId> ErrorQuestion { get; set; }
    }

    public class QuestionAndInputAnswer
    {

        /// <summary>
        /// 题目编号
        /// </summary>
        public string QuestionItemId { get; set; }

        /// <summary>
        /// 用户输入的答案
        /// </summary>
        public string InputAnswer { get; set; }
    }
}
