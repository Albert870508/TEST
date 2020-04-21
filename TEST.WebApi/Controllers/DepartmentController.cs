using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TEST.Api.Result;
using TEST.Api.Route;
using TEST.Exercise.Application.Departments;
using TEST.Exercise.Application.Departments.Dto;
using TEST.Exercise.Domain.Entities;

namespace TEST.WebApi.Controllers
{
    /// <summary>
    /// 部门
    /// </summary>
    [RouteV1("Department")]
    [ApiController]    
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _iDepartmentService;

        public DepartmentController(IDepartmentService iDepartmentService)
        {
            _iDepartmentService = iDepartmentService;
        }
        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Result<List<DepartmentDto>> GetAllDepartment()
        {
            return _iDepartmentService.GetAllDepartment();
        }
    }
}