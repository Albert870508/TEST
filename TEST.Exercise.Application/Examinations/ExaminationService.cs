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
    public class ExaminationService : IExaminationService
    {
        private readonly IRepository<Examination> _examination;
        private readonly IRepository<Question> _question;
        private readonly IRepository<QuestionType> _questionType;
        private readonly IRepository<Score> _score;
        private readonly IUnitOfWork _unitOfWork;
        public ExaminationService(IRepository<Examination> examination,
            IRepository<Question> question, IRepository<QuestionType> questionType,
            IRepository<Score> score, IUnitOfWork unitOfWork)
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
            if (dayOrWeek == "day")
            {
                if (_examination.Any(e => e.StartTime >= DateTime.Now.Date && e.EndTime > DateTime.Now && e.DayOrWeek == "day"))
                {
                    examinationId = _examination.FirstOrDefault(e => e.StartTime >= DateTime.Now.Date && e.EndTime > DateTime.Now && e.DayOrWeek == "day").Id.ToString();
                }
                else
                {//如果没有考试添加一条考试记录
                    examinationId = _examination.InsertAndGetId(new Examination() { StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(1).Date, Note = string.Empty, DayOrWeek = "day" }).ToString();
                    _unitOfWork.SaveChanges();
                }
            }
            if (dayOrWeek == "week")
            {
                var a = DateTimeHelper.GetWeekFirstDayMon(DateTime.Now.Date);
                if (_examination.Any(e => e.StartTime >= a && e.EndTime > DateTime.Now && e.DayOrWeek == "week"))
                {
                    examinationId = _examination.FirstOrDefault(e => e.StartTime >= a && e.EndTime > DateTime.Now && e.DayOrWeek == "week").Id.ToString();
                }
                else
                {//如果没有考试添加一条考试记录
                    examinationId = _examination.InsertAndGetId(new Examination() { StartTime = DateTime.Now, EndTime = DateTimeHelper.GetWeekLastDaySun(DateTime.Now.Date), Note = string.Empty, DayOrWeek = "week" }).ToString();
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
        public Result<TestEndOutput> TestEnd(long userId, ExaminationIntput examinationIntput)
        {
            Score score = new Score();
            if (string.IsNullOrEmpty(examinationIntput.ExaminationId))
            {
                return Result<TestEndOutput>.Fail("该考试不存在");
            }

            var has = false;
            var thisExamination = _examination.Get(long.Parse(examinationIntput.ExaminationId));

            DateTime timeNow = DateTime.Now.Date;
            if (_score.Any(s => s.UserId == userId && s.ExaminationId == thisExamination.Id))
            {
                has = true;
                score = _score.GetAll().FirstOrDefault(s => s.UserId == userId && s.ExaminationId == thisExamination.Id);
            }

            score.UserId = userId;
            score.ExaminationId = thisExamination.Id;
            score.DayOrWeek = thisExamination.DayOrWeek;
            string scoreContent = "[";
            double totalScore = 0;
            var types = _questionType.GetAll().ToList();

            var error = 0;

            List<QuestionStringId> ErrorQuestion = new List<QuestionStringId>();


            ExaminationOutPut questionsResult = new ExaminationOutPut();
            var single = _questionType.FirstOrDefault(q => q.Name == "单选");//单选题数量
            var multiple = _questionType.FirstOrDefault(q => q.Name == "多选");//多选题数量
            var judge = _questionType.FirstOrDefault(q => q.Name == "判断");//判断题数量

            foreach (QuestionAndInputAnswer item in examinationIntput.questionAndInputAnswers)
            {
                //item.QuestionItemId.TrimEnd() + "###" + item.InputAnswer.TrimEnd() + "&&&"
                scoreContent += "{\"" + item.QuestionItemId + "\":\"" + item.InputAnswer + "\"},";
                var thisQuestion = _question.Get(long.Parse(item.QuestionItemId));
                if (item.InputAnswer.TrimEnd() == thisQuestion.Answer)
                {
                    totalScore += types.FirstOrDefault(m => m.Id == thisQuestion.QuestionTypeId).Score;

                }
                else
                {
                    ErrorQuestion.Add(new QuestionStringId()
                    {
                        StrId = thisQuestion.Id.ToString(),
                        Content = thisQuestion.Content,
                        Options = thisQuestion.Options,
                        AnswerNote = thisQuestion.AnswerNote,
                        QuestionType = thisQuestion.QuestionTypeId == single.Id ? "单选" : thisQuestion.QuestionTypeId == multiple.Id ? "多选" : "判断",
                        Answer = thisQuestion.Answer,
                        MyAnswer = item.InputAnswer
                    });
                    error++;
                }
            }
            scoreContent = scoreContent.Substring(0, scoreContent.Length - 1);
            scoreContent += "]";
            score.Content = scoreContent;
            score.TotalScore = totalScore;
            try
            {
                if (has)
                {
                    _score.Update(score);
                }
                else
                {
                    _score.Insert(score);
                }
                _unitOfWork.SaveChanges();
                return Result<TestEndOutput>.Success(new TestEndOutput()
                {
                    Toal = examinationIntput.questionAndInputAnswers.Count(),
                    Error = error,
                    Score = totalScore,
                    ErrorQuestion = ErrorQuestion
                }); ;
            }
            catch
            {
                return Result<TestEndOutput>.Fail("");
            }
        }

        /// <summary>
        /// 根据规则获取相应题目
        /// </summary>
        /// <returns></returns>
        public ExaminationOutPut GetQuestions()
        {


            ExaminationOutPut questionsResult = new ExaminationOutPut();
            var single = _questionType.FirstOrDefault(q => q.Name == "单选");//单选题数量
            var multiple = _questionType.FirstOrDefault(q => q.Name == "多选");//多选题数量
            var judge = _questionType.FirstOrDefault(q => q.Name == "判断");//判断题数量
            List<Question> questionList = _question.GetAll().Where(q => q.QuestionTypeId == single.Id).OrderBy(q => Guid.NewGuid()).Take(single.Number).ToList();
            questionList.AddRange(_question.GetAll().Where(q => q.QuestionTypeId == multiple.Id).OrderBy(q => Guid.NewGuid()).Take(multiple.Number).ToList());
            questionList.AddRange(_question.GetAll().Where(q => q.QuestionTypeId == judge.Id).OrderBy(q => Guid.NewGuid()).Take(judge.Number).ToList());

            questionsResult.QuestionItems = questionList.Select(item =>
            {
                return new QuestionStringId()
                {
                    StrId = item.Id.ToString(),
                    Content = item.Content,
                    Options = item.Options,
                    AnswerNote = item.AnswerNote,
                    QuestionType = item.QuestionTypeId == single.Id ? "单选" : item.QuestionTypeId == multiple.Id ? "多选" : "判断"
                };
            }).ToList();

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
            catch (Exception e)
            {
                return Result<bool>.Fail("添加失败:" + e.Message);
            }
        }
    }
}
