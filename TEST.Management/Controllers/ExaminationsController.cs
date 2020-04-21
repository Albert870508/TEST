using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TEST.Exercise.Application.Examinations;
using TEST.Exercise.Domain.Entities;
using TEST.Management.Models;

namespace TEST.Management.Controllers
{
    public class ExaminationsController : Controller
    {
        private readonly IExaminationService _examinationService;
        public ExaminationsController(IExaminationService examinationService)
        {
            _examinationService = examinationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool AddExaminations([FromBody] ExaminationsViewModel examination)
        {
            return _examinationService.AddExamination(new Examination() { 
             StartTime=DateTime.Parse(examination.StartTime),
             EndTime=DateTime.Parse(examination.EndTime),Note=examination.Note
            }).IsSuccess;
        }
    }
}