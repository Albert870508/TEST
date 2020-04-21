using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TEST.Api.Result;
using TEST.Api.Route;
using TEST.Exercise.Application.Examinations;
using TEST.Exercise.Application.Examinations.Dto;

namespace TEST.WebApi.Controllers
{
    /// <summary>
    /// 考试
    /// </summary>
    [RouteV1("Examination")]
    [ApiController]
    public class ExaminationController : Controller
    {
        private readonly IExaminationService _iExaminationService;

        public ExaminationController(IExaminationService iExaminationService)
        {
            _iExaminationService = iExaminationService;
        }

        /// <summary>
        /// 考试/测试/练习 接口
        /// </summary>
        /// <param name="dayOrWeek">按天还是周（天:day 周：week）</param>
        /// <returns></returns>
        [HttpGet]
        public Result<ExaminationOutPut> TestStart([FromQuery]string dayOrWeek)
        {
           return _iExaminationService.TestStart(dayOrWeek);
        }
        /// <summary>
        /// 交卷接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result<double> TestEnd([FromBody]ExaminationIntput examinationIntput)
        {
            return _iExaminationService.TestEnd(long.Parse(User.Identity.Name), examinationIntput);
        }
    }
}