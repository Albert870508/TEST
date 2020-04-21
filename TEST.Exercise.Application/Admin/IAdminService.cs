using System;
using System.Collections.Generic;
using System.Text;
using TEST.Api.Result;

namespace TEST.Exercise.Application.Admin
{
    public interface IAdminService
    {
        Boolean FindAdmininstrator(string userName, string password);
    }
}
