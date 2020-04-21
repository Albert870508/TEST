using System;
using System.Collections.Generic;
using System.Text;
using TEST.Api.Result;
using TEST.Exercise.Application.Departments.Dto;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.Departments
{
    public interface IDepartmentService
    {
        /// <summary>
        /// 获取所有部门信息
        /// </summary>
        /// <returns></returns>
        Result<List<DepartmentDto>> GetAllDepartment(string departmentName = null);

        Result<Boolean> AddDepartment(Department department);

        Result<Boolean> DeleteDepartment(string departmentId);
    }
}
