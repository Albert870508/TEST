using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TEST.Exercise.Application.Departments;
using TEST.Exercise.Domain.Entities;

namespace TEST.Management.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        public IActionResult Index(string departmentName=null)
        {
            if (departmentName!=null)
            {
                ViewBag.DepartmentName = departmentName;
            }           
            
            return View(_departmentService.GetAllDepartment(departmentName).Data);
        }
        [HttpPost]
        public bool AddDepartment([FromBody]Department department)
        {
            return _departmentService.AddDepartment(department).IsSuccess;
        }

        [HttpPost]
        public bool DeleteDepartment([FromForm]string departmentId)
        {
            return _departmentService.DeleteDepartment(departmentId).IsSuccess;
        }
    }
}