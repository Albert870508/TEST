using System;
using System.Collections.Generic;
using System.Text;
using TEST.Api.Result;
using TEST.Domain.Repositories;
using TEST.Domain.Uow;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Administrator> _Administrator;
        private readonly IUnitOfWork _unitOfWork;
        public AdminService(IRepository<Administrator> Administrator,
            IUnitOfWork unitOfWork)
        {
            _Administrator = Administrator;
            _unitOfWork = unitOfWork;
        }

        public Boolean FindAdmininstrator(string userName, string password)
        {
            if (_Administrator.Any(a=>a.Name==userName && a.Password == Helper.StringTools.MD5Encrypt32(password)))
            {
                return true;
            }
            return false;
        }
    }
}
