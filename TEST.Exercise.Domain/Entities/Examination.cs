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
        private string dayOrWeek;

        ///// <summary>
        ///// 考试标题(这个字段在数据库中是唯一的)
        ///// </summary>
        //public string Title { get; set; }

        /// <summary>
        /// 日日学/周周练
        /// </summary>
        public string DayOrWeek
        {
            get
            {
                if (string.IsNullOrEmpty(dayOrWeek))
                    dayOrWeek = "day";
                return dayOrWeek;
            }
            set
            {
                dayOrWeek = value;
            }
        }


        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 考试备注信息
        /// </summary>
        public string Note { get; set; }
    }
}
