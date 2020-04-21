using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEST.Api.Result;
using TEST.Domain.Repositories;
using TEST.Domain.Uow;
using TEST.Exercise.Application.Departments.Dto;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.Departments
{
    public class DepartmentService:IDepartmentService
    {
        private readonly IRepository<Department> _department;
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IRepository<Department> department,
            IUnitOfWork unitOfWork)
        {
            _department = department;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 获取所有部门信息
        /// </summary>
        /// <returns></returns>
        public Result<List<DepartmentDto>> GetAllDepartment(string departmentName = null)
        {
            var query = _department.GetAll();
            if (!string.IsNullOrEmpty(departmentName))
            {
                query = query.Where(d => d.Name == departmentName);
            }
            query = query.AsQueryable();            
            return Result<List<DepartmentDto>>.Success(query.ToList().Select<Department,DepartmentDto>(item=> {
                return new DepartmentDto()
                {
                    Id=item.Id.ToString(),
                    Name=item.Name
                };
            }).ToList());
        }

        public Result<Boolean> AddDepartment(Department department)
        {
            if (string.IsNullOrEmpty(department.Name))
            {
                return Result<Boolean>.Fail("添加失败");
            }
            else
            {
                try
                {
                    _department.Insert(department);
                    _unitOfWork.SaveChanges();
                    return Result<Boolean>.Success(true);
                }
                catch
                {
                    return Result<Boolean>.Fail("添加失败");
                }
            }
        }

        public Result<Boolean> DeleteDepartment(string departmentId)
        {
            if (!string.IsNullOrEmpty(departmentId))
            {
                _department.Delete(long.Parse(departmentId));
                _unitOfWork.SaveChanges();
            }
            return Result<Boolean>.Success(true);
        }
    }
}
