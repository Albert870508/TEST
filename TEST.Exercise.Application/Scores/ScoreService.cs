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
    public class ScoreService:IScoreService
    {
        IRepository<Score> _score;
        private readonly IUnitOfWork _unitOfWork;
        public ScoreService(IRepository<Score> score,
            IUnitOfWork unitOfWork)
        {
            _score = score;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 排名接口
        /// </summary>
        /// <param name="scoreIntput"></param>
        /// <returns></returns>
        public Result<Paging<List<ScoreOutput>>> Ranking(ScoreIntput scoreIntput)
        {
            if (string.IsNullOrEmpty(scoreIntput.ExaminationId) || scoreIntput.ExaminationId == "0")
            {
                return Result<Paging<List<ScoreOutput>>>.Fail("该考试不存在");
            }
            var query = _score.GetAll().Where(s => s.ExaminationId == long.Parse(scoreIntput.ExaminationId));
            DateTime timeNow = DateTime.Now.Date;
            if (scoreIntput.DayOrWeek =="day")
            {
                query = query.Where(s => s.CreateTime > timeNow);
            }
            if (scoreIntput.DayOrWeek == "week")
            {
                query = query.Where(s => s.CreateTime > DateTimeHelper.GetWeekFirstDayMon(timeNow));
            }            
            query = query.OrderByDescending(s=>s.TotalScore).AsQueryable();
            query =query.GetPaginationData(scoreIntput.Input);
            return Result<Paging<List<ScoreOutput>>>.Success(new Paging<List<ScoreOutput>>() { 
              Total = query.ToList().Count(),
              Data = query.ToList().Select<Score, ScoreOutput>(item=> {
                  return new ScoreOutput()
                  {
                      UserId = item.UserId.ToString(),
                      TotalScore=item.TotalScore
                  };
              }).ToList()
            });
        }
    }
}
