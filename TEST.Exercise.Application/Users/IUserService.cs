using System;
using System.Collections.Generic;
using System.Text;
using TEST.Api.Result;
using TEST.Exercise.Application.Users.Dto;
using TEST.JWT;

namespace TEST.Exercise.Application.Users
{
    public interface IUserService
    {
        /// <summary>
        /// 用户登陆接口
        /// </summary>
        /// <param name="weChatOpenId"></param>
        /// <param name="tokenOptions"></param>
        /// <returns></returns>
        Result<LoginOutput> Login(string weChatOpenId, TokenProvider tokenOptions);
    }
}
