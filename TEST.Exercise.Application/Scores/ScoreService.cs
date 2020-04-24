using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEST.Api.Input;
using TEST.Api.Result;
using TEST.Domain.Repositories;
using TEST.Domain.Uow;
using TEST.Exercise.Application.Scores.Dto;
using TEST.Exercise.Domain.Entities;
using TEST.Helper;

namespace TEST.Exercise.Application.Scores
{
    public class ScoreService : IScoreService
    {
        IRepository<Score> _score;
        IRepository<User> _user;
        IRepository<Examination> _examination;
        private readonly IUnitOfWork _unitOfWork;
        public ScoreService(IRepository<Score> score,
            IRepository<User> user,
            IRepository<Examination> examination,
            IUnitOfWork unitOfWork)
        {
            _examination = examination;
            _user = user;
            _score = score;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 排名接口
        /// </summary>
        /// <param name="scoreIntput"></param>
        /// <returns></returns>
        public Result<List<ScoreOutput>> Ranking(ScoreIntput scoreIntput)
        {
            //if (string.IsNullOrEmpty(scoreIntput.ExaminationId) || scoreIntput.ExaminationId == "0")
            //{
            //    return Result<Paging<List<ScoreOutput>>>.Fail("该考试不存在");
            //}

            long examinationId = 0;
            if (scoreIntput.DayOrWeek == "day")
            {
                if (_examination.Any(e => e.StartTime >= DateTime.Now.Date && e.EndTime > DateTime.Now && e.DayOrWeek == "day"))
                {
                    examinationId = _examination.FirstOrDefault(e => e.StartTime >= DateTime.Now.Date && e.EndTime > DateTime.Now && e.DayOrWeek == "day").Id;
                }
            }
            else if (scoreIntput.DayOrWeek == "week")
            {
                var a = DateTimeHelper.GetWeekFirstDayMon(DateTime.Now.Date);
                if (_examination.Any(e => e.StartTime >= a && e.EndTime > DateTime.Now && e.DayOrWeek == "week"))
                {
                    examinationId = _examination.FirstOrDefault(e => e.StartTime >= a && e.EndTime > DateTime.Now && e.DayOrWeek == "week").Id;
                }
            }

            if (examinationId == 0)
            {
                return Result<List<ScoreOutput>>.Success(new List<ScoreOutput>());
            }

            var query = _score.GetAll().Where(m => m.ExaminationId == examinationId);
            DateTime timeNow = DateTime.Now.Date;

            query = query.OrderByDescending(s => s.TotalScore).AsQueryable();
            query = query.Skip(scoreIntput.PageIndex * 10).Take(10);
            var list = query.ToList();
            var data = list.Select<Score, ScoreOutput>(item =>
              {
                  var thisUser = _user.Get(item.UserId);
                  return new ScoreOutput()
                  {
                      UserName = thisUser.UserName,
                      Department = thisUser.Department,
                      TotalScore = item.TotalScore
                  };
              }).ToList();
            return Result<List<ScoreOutput>>.Success(data);
        }
    }
}
