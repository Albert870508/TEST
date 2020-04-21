using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TEST.Exercise.Application.Exercises;
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
            #region //获取单选题分值/单选题随机出题数量 多选题分值/多选题随机出题数量 判断题分值/判断题随机出题数量 并在前台展示
            List<QuestionType> questionTypes = _exerciseTypeService.GetExerciseTypes().Data;
            ViewData["SingleScore"] = questionTypes.FirstOrDefault(m => m.Name == "单选").Score;
            ViewData["MultipleScore"] = questionTypes.FirstOrDefault(m => m.Name == "多选").Score;
            ViewData["JudgeScore"] = questionTypes.FirstOrDefault(m => m.Name == "判断").Score;
            ViewData["SingleNumber"] = questionTypes.FirstOrDefault(m => m.Name == "单选").Number;
            ViewData["MultipleNumber"] = questionTypes.FirstOrDefault(m => m.Name == "多选").Number;
            ViewData["JudgeNumber"] = questionTypes.FirstOrDefault(m => m.Name == "判断").Number;
            #endregion
            #region //获取题库中单选题总数量 多选题总数量 判断题总数量，并在前台展示
            List<Question> questions = _exerciseService.GetExercise(string.Empty).Data;
            ViewData["SingleTotalNumber"] = questions.Where(a=>a.QuestionTypeId == questionTypes.FirstOrDefault(q=>q.Name=="单选").Id).Count();
            ViewData["MultipleTotalNumber"] = questions.Where(a=>a.QuestionTypeId == questionTypes.FirstOrDefault(q=>q.Name=="多选").Id).Count();
            ViewData["JudgeTotalNumber"] = questions.Where(a=>a.QuestionTypeId == questionTypes.FirstOrDefault(q=>q.Name=="判断").Id).Count();
            #endregion
            return View();
        }
        [HttpPost]
        public int UpdateScoreAndNumber([FromBody]QuestionTypeScoreAndNumber uestionTypeScoreAndNumber)
        {
            if (uestionTypeScoreAndNumber != null)
            {
                if (_exerciseTypeService.UpdateQuestionTypeScoreAndNumber(
                    double.Parse(uestionTypeScoreAndNumber.SingleScore),double.Parse(uestionTypeScoreAndNumber.MultipleScore),
                    double.Parse(uestionTypeScoreAndNumber.JudgeScore),Int32.Parse(uestionTypeScoreAndNumber.SingleNumber),
                    Int32.Parse(uestionTypeScoreAndNumber.MultipleNumber),Int32.Parse(uestionTypeScoreAndNumber.JudgeNumber)).IsSuccess)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        
    }
}