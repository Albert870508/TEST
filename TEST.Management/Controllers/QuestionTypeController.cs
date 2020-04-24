using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TEST.Api.Result;
using TEST.Exercise.Application.Exercises;
using TEST.Exercise.Application.Exercises.Dto;
using TEST.Exercise.Application.ExerciseType;
using TEST.Exercise.Domain.Entities;
using TEST.Management.Models;

namespace TEST.Management.Controllers
{
    public class QuestionTypeController : Controller
    {
        private readonly IExerciseTypeService _exerciseTypeService;
        private readonly IExerciseService _exerciseService;
        public QuestionTypeController(IExerciseTypeService exerciseTypeService, IExerciseService exerciseService)
        {
            _exerciseTypeService = exerciseTypeService;
            _exerciseService = exerciseService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<QuestionType> questionTypes = _exerciseTypeService.GetExerciseTypes().Data;
            #region //获取题库中单选题总数量 多选题总数量 判断题总数量，并在前台展示
            List<QuestionsDto> questions = _exerciseService.GetExercise(string.Empty).Data;
            ViewData["SingleTotalNumber"] = questions.Where(a=>a.QuestionTypeId == questionTypes.FirstOrDefault(q=>q.Name=="单选").Id.ToString()).Count();
            ViewData["MultipleTotalNumber"] = questions.Where(a=>a.QuestionTypeId == questionTypes.FirstOrDefault(q=>q.Name=="多选").Id.ToString()).Count();
            ViewData["JudgeTotalNumber"] = questions.Where(a=>a.QuestionTypeId == questionTypes.FirstOrDefault(q=>q.Name=="判断").Id.ToString()).Count();
            #endregion
            return View(questionTypes);
        }
        [HttpPost]
        public Result<bool> UpdateScoreAndNumber([FromBody]QuestionType questionType)
        {
            return _exerciseTypeService.UpdateScoreAndNumber(questionType);
        }
        
    }
}