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

        /// <summary>
        /// 用户完善个人信息
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        Result<bool> ImproveInformation(long UserId,UserInput userInput);
    }
}
