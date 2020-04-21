using System;
using System.Collections.Generic;
using System.Text;
using TEST.Api.Input;

namespace TEST.Exercise.Application.Scores.Dto
{
    public class ScoreOutput
    {
        public string UserId { get; set; }

        public double TotalScore { get; set; }
    }
    public class ScoreIntput
    {
        /// <summary>
        /// 考试编号
        /// </summary>
        public string ExaminationId { get; set; }
        /// <summary>
        /// 周排名还是日排名（日排名：day 周排名:week）如果为空则排序所有
        /// </summary>
        public string DayOrWeek { get; set; }
        /// <summary>
        /// 输入参数（分页）
        /// </summary>
        public QueryParameters Input { get; set; }
    }
}
