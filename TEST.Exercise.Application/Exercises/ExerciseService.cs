using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEST.Api.Input;
using TEST.Api.Result;
using TEST.Domain.Repositories;
using TEST.Domain.Uow;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.Exercises
{
    public class ExerciseService:IExerciseService
    {
        private readonly IRepository<Question> _Question;
        private readonly IUnitOfWork _unitOfWork;
        public ExerciseService(IRepository<Question> Question,
            IUnitOfWork unitOfWork)
        {
            _Question = Question;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取题库中所有题目
        /// </summary>
        /// <returns></returns>
        public Result<List<Question>> GetExercise(string searchString)
        {
            var query = _Question.GetAll();
            if (!string.IsNullOrEmpty(searchString))
            {
               query =query.Where(a => a.QuestionTypeName == searchString);
            }
            query = query.AsQueryable();
            return Result<List<Question>>.Success(query.ToList());

        }
        /// <summary>
        /// 根据题目类型获取对应题目
        /// </summary>
        /// <param name="questionTypeId"></param>
        /// <returns></returns>
        public Result<List<Question>> GetExerciseByType(long questionTypeId)
        {
            if (_Question.GetAll().Any(q => q.QuestionTypeId == questionTypeId))
            {
                return Result<List<Question>>.Success(_Question.GetAll().Where(q => q.QuestionTypeId == questionTypeId).ToList());
            }
            else
            {
                return Result<List<Question>>.Success(new List<Question>() {new Question() });
            }
        }
        /// <summary>
        /// 批量添加题目（通过上传Excel表格批量添加）
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        public Result<Boolean> AddQuestions(List<Question> questions)
        {
            try
            {                
                foreach (Question question in questions)
                {
                    _Question.Insert(question);
                }
                _unitOfWork.SaveChanges();
                return Result<Boolean>.Success(true);
            }
            catch 
            {

                return Result<Boolean>.Fail("添加失败");
            }
        }
    }
}
