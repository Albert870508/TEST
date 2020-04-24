using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TEST.Exercise.Application.Scores;
using X.PagedList;

namespace TEST.Management.Controllers
{
    public class ScoreController : Controller
    {
        private readonly IScoreService _iScoreService;
        public ScoreController(IScoreService iScoreService)
        {
            _iScoreService = iScoreService;
        }

        public IActionResult Index(string dayOrWeek,int? pageIndex)
        {
            ViewBag.DayOrWeek = dayOrWeek;
            var listPaged = _iScoreService.Ranking(new Exercise.Application.Scores.Dto.ScoreIntput()
            {
                DayOrWeek = dayOrWeek,
                PageIndex = int.Parse(pageIndex.ToString()) - 1
            }).Data.ToPagedList(pageIndex ?? 1, 10);
            if (listPaged.PageNumber != 1 && pageIndex.HasValue && pageIndex > listPaged.PageCount)
            {
                listPaged = null;
            }
            return View(listPaged);
        }
    }
}