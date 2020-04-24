using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEST.Api.Input;
using TEST.Api.Result;
using TEST.Domain.Repositories;
using TEST.Domain.Uow;
using TEST.Exercise.Application.Exercises.Dto;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.Exercises
{
    public class ExerciseService:IExerciseService
    {
        private readonly IRepository<Question> _Question;
        private readonly IRepository<QuestionType> _QuestionType;
        private readonly IUnitOfWork _unitOfWork;
        public ExerciseService(IRepository<Question> Question, IRepository<QuestionType> QuestionType,
            IUnitOfWork unitOfWork)
        {
            _Question = Question;
            _QuestionType = QuestionType;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取题库中所有题目
        /// </summary>
        /// <returns></returns>
        public Result<List<QuestionsDto>> GetExercise(string searchString)
        {
            var query = _Question.GetAll();
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(a => a.QuestionTypeId.ToString() == searchString);
            }
            query = query.AsQueryable();

            return Result<List<QuestionsDto>>.Success(query.ToList().Select<Question, QuestionsDto>(item => {
                return new QuestionsDto()
                {
                    Id = item.Id.ToString(),
                    QuestionTypeId = item.QuestionTypeId.ToString(),
                    QuestionTypeTitle = _QuestionType.Get(item.QuestionTypeId).Name,
                    Content = item.Content,
                    Options = item.Options,
                    Answer = item.Answer,
                    AnswerNote = item.AnswerNote,
                    QuestionTypeName = item.QuestionTypeName
                };
            }).ToList());

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
