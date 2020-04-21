using System;
using System.Collections.Generic;
using System.Text;
using TEST.Api.Result;
using TEST.Exercise.Application.Examinations.Dto;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.Examinations
{
    public interface IExaminationService
    {
        /// <summary>
        /// 考试/测试/练习 接口
        /// </summary>
        /// <param name="dayOrWeek"></param>
        /// <returns></returns>
        Result<ExaminationOutPut> TestStart(string dayOrWeek);
        /// <summary>
        /// 交卷接口
        /// </summary>
        /// <returns></returns>
        Result<double> TestEnd(long userId, ExaminationIntput examinationIntput);

        Result<bool> AddExamination(Examination examination);
    }
}
