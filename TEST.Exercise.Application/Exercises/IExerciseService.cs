using System;
using System.Collections.Generic;
using System.Text;
using TEST.Api.Input;
using TEST.Api.Result;
using TEST.Exercise.Application.Exercises.Dto;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.Exercises
{
    public interface IExerciseService
    {
        /// <summary>
        /// 获取题库中所有题目
        /// </summary>
        /// <returns></returns>
        Result<List<QuestionsDto>> GetExercise(string searchString);
        /// <summary>
        /// 根据题目类型获取对应题目
        /// </summary>
        /// <param name="questionTypeId"></param>
        /// <returns></returns>
        Result<List<Question>> GetExerciseByType(long questionTypeId);
        /// <summary>
        /// 批量添加题目（通过上传Excel表格批量添加）
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        Result<Boolean> AddQuestions(List<Question> questions);
    }
}
