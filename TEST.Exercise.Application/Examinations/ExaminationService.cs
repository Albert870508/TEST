using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEST.Api.Result;
using TEST.Domain.Repositories;
using TEST.Domain.Uow;
using TEST.Exercise.Application.Examinations.Dto;
using TEST.Exercise.Domain.Entities;
using TEST.Helper;

namespace TEST.Exercise.Application.Examinations
{
    public class ExaminationService:IExaminationService
    {
        private readonly IRepository<Examination> _examination;
        private readonly IRepository<Question> _question;
        private readonly IRepository<QuestionType> _questionType;
        private readonly IRepository<Score> _score;
        private readonly IUnitOfWork _unitOfWork;
        public ExaminationService(IRepository<Examination> examination,
            IRepository<Question> question, IRepository<QuestionType> questionType,
            IRepository<Score> score,IUnitOfWork unitOfWork)
        {
            _examination = examination;
            _question = question;
            _questionType = questionType;
            _score = score;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 考试接口
        /// </summary>
        /// <param name="dayOrWeek"></param>
        /// <returns></returns>
        public Result<ExaminationOutPut> TestStart(string dayOrWeek) 
        {
            string examinationId = string.Empty;//考试编号
            if (dayOrWeek=="day")
            {
                if (_examination.Any(e => e.StartTime >= DateTime.Now.Date && e.EndTime > DateTime.Now))
                {
                    examinationId = _examination.FirstOrDefault(e => e.EndTime > DateTime.Now).Id.ToString();
                }
                else
                {//如果没有考试添加一条考试记录
                    examinationId = _examination.InsertAndGetId(new Examination() { StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(1).Date, Note = string.Empty }).ToString();
                    _unitOfWork.SaveChanges();
                }
            }
            if (dayOrWeek=="week")
            {
                if (_examination.Any(e => e.StartTime >= DateTimeHelper.GetWeekFirstDayMon(DateTime.Now.Date) && e.EndTime > DateTime.Now))
                {
                    examinationId = _examination.FirstOrDefault(e => e.EndTime > DateTime.Now).Id.ToString();
                }
                else
                {//如果没有考试添加一条考试记录
                    examinationId = _examination.InsertAndGetId(new Examination() { StartTime = DateTime.Now, EndTime = DateTimeHelper.GetWeekLastDaySun(DateTime.Now.Date), Note = string.Empty }).ToString();
                    _unitOfWork.SaveChanges();
                }
            }
            if (string.IsNullOrEmpty(dayOrWeek))
            {
                #region
                if (_examination.Any(e => e.EndTime > DateTime.Now))//当前时间小于考试结束时间代表有考试
                {
                    examinationId = _examination.FirstOrDefault(e => e.EndTime > DateTime.Now).Id.ToString();
                }
                else
                {//如果没有考试添加一条考试记录
                    examinationId = _examination.InsertAndGetId(new Examination() { StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(1).Date, Note = string.Empty }).ToString();
                    _unitOfWork.SaveChanges();
                }
                #endregion
            }

            ExaminationOutPut examinationOutPut = GetQuestions();
            examinationOutPut.ExaminationId = examinationId;
            return Result<ExaminationOutPut>.Success(examinationOutPut);
        }
        /// <summary>
        /// 交卷
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="examinationIntput"></param>
        /// <returns></returns>
        public Result<double> TestEnd(long userId, ExaminationIntput examinationIntput)
        {
            Score score = new Score();
            if (!string.IsNullOrEmpty(examinationIntput.ExaminationId))
            {
                return Result<double>.Fail("该考试不存在");
            }
            score.UserId = userId;
            score.ExaminationId = long.Parse(examinationIntput.ExaminationId);
            string scoreContent = string.Empty;
            double totalScore = 0;
            foreach (QuestionAndInputAnswer item in examinationIntput.questionAndInputAnswers)
            {
                scoreContent += item.QuestionItemId.TrimEnd() + "###" + item.InputAnswer.TrimEnd() + "&&&";
                if (item.InputAnswer.TrimEnd()==_question.Get(long.Parse(item.QuestionItemId)).Answer)
                {
                    totalScore += _questionType.Get(_question.Get(long.Parse(item.QuestionItemId)).QuestionTypeId).Score;
                }
                
            }
            score.Content = scoreContent;
            score.TotalScore = totalScore;
            try
            {
                _score.Insert(score);
                _unitOfWork.SaveChanges();
                return Result<double>.Success(totalScore);
            }
            catch 
            {
                return Result<double>.Fail("");                
            }
        }

        /// <summary>
        /// 根据规则获取相应题目
        /// </summary>
        /// <returns></returns>
        public ExaminationOutPut GetQuestions()
        {
            ExaminationOutPut questionsResult = new ExaminationOutPut();
            int singleNumber = _questionType.FirstOrDefault(q => q.Name == "单选").Number;//单选题数量
            int multipleNumber = _questionType.FirstOrDefault(q => q.Name == "多选").Number;//多选题数量
            int judgeNumber = _questionType.FirstOrDefault(q => q.Name == "判断").Number;//判断题数量
            List<Question> questionList = _question.GetAll().Where(q => q.QuestionTypeName == "单选").OrderBy(q => Guid.NewGuid()).Take(singleNumber).ToList();
            questionList.AddRange( _question.GetAll().Where(q => q.QuestionTypeName == "多选").OrderBy(q => Guid.NewGuid()).Take(multipleNumber).ToList());
            questionList.AddRange(_question.GetAll().Where(q => q.QuestionTypeName == "判断").OrderBy(q => Guid.NewGuid()).Take(judgeNumber).ToList());
            foreach (Question question in questionList)
            {
                questionsResult.QuestionItems.Add(new QuestionItem() { QuestionItemId=question.Id.ToString(),  QuestionItemContent=question.Content, QuestionItemType=question.QuestionTypeName });
            }
            return questionsResult;
        }



        public Result<bool> AddExamination(Examination examination)
        {
            try
            {
                _examination.Insert(examination);
                _unitOfWork.SaveChanges();
                return Result<bool>.Success(true);
            }
            catch(Exception e)
            {
                return Result<bool>.Fail("添加失败:"+e.Message);
            }
        }
    }
}
