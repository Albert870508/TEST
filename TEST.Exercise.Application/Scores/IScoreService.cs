using System;
using System.Collections.Generic;
using System.Text;
using TEST.Api.Input;
using TEST.Api.Result;
using TEST.Exercise.Application.Scores.Dto;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.Scores
{
    public interface IScoreService
    {
        /// <summary>
        ///  排名接口
        /// </summary>
        /// <param name="scoreIntput"></param>
        /// <returns></returns>
        Result<Paging<List<ScoreOutput>>> Ranking(ScoreIntput scoreIntput);
    }
}
