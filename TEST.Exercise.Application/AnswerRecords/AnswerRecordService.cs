using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEST.Api.Result;
using TEST.Domain.Repositories;
using TEST.Domain.Uow;
using TEST.Exercise.Application.AnswerRecords.Dto;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.AnswerRecords
{
    public class AnswerRecordService: IAnswerRecordService
    {
        private readonly IRepository<AnswerRecord> _answerRecord;
        private readonly IRepository<QuestionType> _questionType;
        private readonly IRepository<Question> _question;
        private readonly IUnitOfWork _unitOfWork;
        public AnswerRecordService(IRepository<AnswerRecord> answerRecord,
            IRepository<QuestionType> questionType, IRepository<Question> question,
            IUnitOfWork unitOfWork)
        {
            _answerRecord = answerRecord;
            _questionType = questionType;
            _question = question;
            _unitOfWork = unitOfWork;
        }
        // <summary>
        /// 添加答题记录
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        public Result<bool> AddAnswerRecord(long userId, List<string> questions)
        {
            if (string.IsNullOrEmpty(userId.ToString()))
            {
                return Result<bool>.Fail("添加失败");
            }
            try
            {
                foreach (string question in questions)
                {
                    if (!_answerRecord.Any(a=>a.UserId==userId && a.QuestionId== long.Parse(question)))
                    {
                        _answerRecord.Insert(new AnswerRecord() { UserId = userId, QuestionId = long.Parse(question) });
                    }
                    
                }
                _unitOfWork.SaveChanges();
                return Result<bool>.Success(true);
            }
            catch 
            {
                return Result<bool>.Fail("添加失败");
            }
        }

        /// <summary>
        /// 获取某个用户各个类型试题的答题数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Result<AnswerRecordDto> GetRecordAnser(long userId)
        {
            AnswerRecordDto answerRecordDto = new AnswerRecordDto();
            if (string.IsNullOrEmpty(userId.ToString()))
            {
                return Result<AnswerRecordDto>.Fail("用户不存在");
            }
            if (!_answerRecord.Any(a => a.UserId == userId))
            {
                answerRecordDto = new AnswerRecordDto()
                {
                    Single = 0,
                    Multiple = 0,
                    Case = 0,
                    Completion = 0,
                    Judge = 0,
                    Simple = 0
                };

            }
            else
            {
                var query = _answerRecord.GetAll().Where(a => a.UserId == userId).ToList();
                query.ForEach(a =>
                {
                    string questionTypeName = _questionType.Get(_question.Get(a.QuestionId).QuestionTypeId).Name;
                    if (questionTypeName== "单选")
                    {
                        answerRecordDto.Single++;
                    }
                    if (questionTypeName == "多选")
                    {
                        answerRecordDto.Multiple++;
                    }
                    if (questionTypeName == "判断")
                    {
                        answerRecordDto.Judge++;
                    }
                    if (questionTypeName == "填空")
                    {
                        answerRecordDto.Completion++;
                    }
                    if (questionTypeName == "简答")
                    {
                        answerRecordDto.Simple++;
                    }
                    if (questionTypeName == "案例分析")
                    {
                        answerRecordDto.Case++;
                    }
                });                
            }
            #region
            answerRecordDto.SimpleTotal = _question.GetAll().Where(q => q.QuestionTypeId == _questionType.FirstOrDefault(qt => qt.Name == "单选").Id).Count();
            answerRecordDto.MultipleTotal = _question.GetAll().Where(q => q.QuestionTypeId == _questionType.FirstOrDefault(qt => qt.Name == "多选").Id).Count();
            answerRecordDto.JudgeTotal = _question.GetAll().Where(q => q.QuestionTypeId == _questionType.FirstOrDefault(qt => qt.Name == "判断").Id).Count();
            answerRecordDto.SimpleTotal = _question.GetAll().Where(q => q.QuestionTypeId == _questionType.FirstOrDefault(qt => qt.Name == "简答").Id).Count();
            answerRecordDto.CompletionTotal = _question.GetAll().Where(q => q.QuestionTypeId == _questionType.FirstOrDefault(qt => qt.Name == "填空").Id).Count();
            answerRecordDto.CaseTotal = _question.GetAll().Where(q => q.QuestionTypeId == _questionType.FirstOrDefault(qt => qt.Name == "案例分析").Id).Count();
            #endregion

            return Result<AnswerRecordDto>.Success(answerRecordDto);
        }        

    }
}
