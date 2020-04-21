using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TEST.Api.Input;
using TEST.Api.Result;
using TEST.Api.Route;
using TEST.Exercise.Application.Scores;
using TEST.Exercise.Application.Scores.Dto;

namespace TEST.WebApi.Controllers
{
    /// <summary>
    /// 排名
    /// </summary>
    [RouteV1("Score")]
    [ApiController]
    public class ScoreController : Controller
    {
        private readonly IScoreService _iScoreService;

        public ScoreController(IScoreService iScoreService)
        {
            _iScoreService = iScoreService;
        }

        /// <summary>
        /// 排名接口
        /// </summary>
        /// <param name="examinationId">考试编号</param>
        /// <param name="dayOrWeek">周排名还是日排名（日排名：day 周排名:week）如果为空则排序所有</param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        [HttpPost]
        public Result<Paging<List<ScoreOutput>>> Ranking([FromBody]ScoreIntput scoreIntput)
        {
           return _iScoreService.Ranking(scoreIntput);
        }
    }
}