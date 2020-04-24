using System.Collections.Generic;
using TEST.Api.Result;
using TEST.Domain.Repositories;
using TEST.Domain.Uow;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.ExerciseType
{
    public class ExerciseTypeService: IExerciseTypeService
    {
        private readonly IRepository<QuestionType> _QuestionType;
        private readonly IUnitOfWork _unitOfWork;
        public ExerciseTypeService(IRepository<QuestionType> QuestionType,
            IUnitOfWork unitOfWork)
        {
            _QuestionType = QuestionType;
            _unitOfWork = unitOfWork;
        }
        public Result<List<QuestionType>> GetExerciseTypes()
        {
            return Result<List<QuestionType>>.Success(_QuestionType.GetAllList());          
        }
        public Result<bool> UpdateScoreAndNumber(QuestionType questionType)
        {
            if (!_QuestionType.Any(q => q.Id == questionType.Id))
            {
                return Result<bool>.Fail("类型不存在");
            }
            else
            {
                try
                {
                    var thisQuestionType = _QuestionType.Get(questionType.Id);
                    thisQuestionType.Number = questionType.Number;
                    thisQuestionType.Score = questionType.Score;
                    _QuestionType.Update(thisQuestionType);
                    _unitOfWork.SaveChanges();
                    return Result<bool>.Success(true);
                }
                catch
                {

                    return Result<bool>.Fail("修改失败");
                }
            }
        }        
        /// <summary>
        /// 根据题目名称获取题目编号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Result<long> GetIdByType(string name)
        {
            if (_QuestionType.Any(q => q.Name == name))
            {
                return Result<long>.Success(_QuestionType.FirstOrDefault(q => q.Name == name).Id);
            }
            else
            {
                return Result<long>.Fail("填写有错");
            }

        }

        public Result<bool> UpdateQuestionTypeScoreAndNumber(double singleScore, double multipleScore, double judgeScore, 
            int singleNumber, int multipleNumber, int judgeNumber)
        {
            try
            {
                _QuestionType.FirstOrDefault(m => m.Name == "单选").Score = singleScore;
                _QuestionType.FirstOrDefault(m => m.Name == "多选").Score = multipleScore;
                _QuestionType.FirstOrDefault(m => m.Name == "判断").Score = judgeScore;
                _QuestionType.FirstOrDefault(m => m.Name == "单选").Number = singleNumber;
                _QuestionType.FirstOrDefault(m => m.Name == "多选").Number = multipleNumber;
                _QuestionType.FirstOrDefault(m => m.Name == "判断").Number = judgeNumber;
                _unitOfWork.SaveChanges();
                return Result<bool>.Success(true);
            }
            catch
            {

                return Result<bool>.Fail("修改失败");
            }
        }
        
    }
}
