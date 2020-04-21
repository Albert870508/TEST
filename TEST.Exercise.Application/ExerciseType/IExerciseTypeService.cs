using System.Collections.Generic;
using TEST.Api.Result;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.ExerciseType
{
    public interface IExerciseTypeService
    {
        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        Result<List<QuestionType>> GetExerciseTypes();
        Result<bool> AddTypes(QuestionType questionType);
        Result<long> GetIdByType(string name);

        Result<bool> UpdateQuestionTypeScoreAndNumber(double singleScore,double multipleScore, double judgeScore,
            int singleNumber, int multipleNumber, int judgeNumber);

    }
}
