using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TEST.Api.Result;
using TEST.Api.Route;
using TEST.Exercise.Application.Exercises;
using TEST.Exercise.Domain.Entities;

namespace TEST.WebApi.Controllers
{
    /// <summary>
    /// 试题
    /// </summary>
    [RouteV1("Exercise")]
    [ApiController]
    public class QuestionsController : Controller
    {
        private readonly IExerciseService _iExerciseService;

        public QuestionsController(IExerciseService iExerciseService)
        {
            _iExerciseService = iExerciseService;
        }
        /// <summary>
        /// 获取所有试题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Result<List<Question>> GetAllQuestions()
        {
            return _iExerciseService.GetExercise(string.Empty);
        }

        [HttpGet]
        public Result<List<Question>> GetTestQuestion()
        {
            return Result<List<Question>>.Fail("");
        }
    }
}