using System;
using System.Collections.Generic;
using System.Text;
using TEST.Api.Result;
using TEST.Domain.Repositories;
using TEST.Domain.Uow;
using TEST.Exercise.Application.Users.Dto;
using TEST.Exercise.Domain.Entities;
using TEST.JWT;

namespace TEST.Exercise.Application.Users
{
    public class UserService:IUserService
    {
        private readonly IRepository<User> _user;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IRepository<User> user,
            IUnitOfWork unitOfWork)
        {
            _user = user;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 登陆接口
        /// </summary>
        /// <param name="loginCode"></param>
        /// <param name="tokenOptions"></param>
        /// <returns></returns>
        public Result<LoginOutput> Login(string weChatOpenId, TokenProvider tokenOptions)
        {
            if (_user.Any(u => u.WeChatOpenId == weChatOpenId))//如果用户存在
            {
                User userInfo = _user.FirstOrDefault(u => u.WeChatOpenId == weChatOpenId);
                if (string.IsNullOrEmpty(userInfo.UserName) ||
                    string.IsNullOrEmpty(userInfo.PhoneNumber) || string.IsNullOrEmpty(userInfo.DepartmentId))
                { //资料不完善，返回false让用户完善信息
                    return Result<LoginOutput>.Success(new LoginOutput
                    {
                        UserId = userInfo.Id.ToString(),
                        Token = tokenOptions.GenerateToken(userInfo.Id.ToString()).access_token,
                        Perfect = false
                    });
                }
                else
                {
                    return Result<LoginOutput>.Success(new LoginOutput
                    {
                        UserId = userInfo.Id.ToString(),
                        Token = tokenOptions.GenerateToken(userInfo.Id.ToString()).access_token,
                        Perfect = true
                    }); ;
                }
            }
            else //用户不存在创建用户（相当于注册）,返回false让用户完善信息
            {
                _user.Insert(new User() {WeChatOpenId =weChatOpenId });
                _unitOfWork.SaveChanges();
                string userId = (_user.FirstOrDefault(u => u.WeChatOpenId == weChatOpenId)).Id.ToString();
                return Result<LoginOutput>.Success(new LoginOutput
                {
                    UserId = userId,
                    Token = tokenOptions.GenerateToken(userId).access_token,
                    Perfect = false
                });
            }
        }
        /// <summary>
        /// 完善个人信息
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public Result<bool> ImproveInformation(long UserId, UserInput userInput)
        {

            if (!_user.Any(m=>m.Id==UserId))
            {
                return Result<Boolean>.Fail("用户不存在");
            }
            if (String.IsNullOrEmpty(userInput.UserName) || string.IsNullOrEmpty(userInput.UserPhone) || string.IsNullOrEmpty(userInput.UserDempartment))
            {
                return Result<Boolean>.Fail("不能为空");
            }
            else
            {
                _user.FirstOrDefault(UserId).UserName = userInput.UserName;
                _user.FirstOrDefault(UserId).PhoneNumber = userInput.UserPhone;
                _user.FirstOrDefault(UserId).DepartmentId = userInput.UserDempartment;
                _unitOfWork.SaveChanges();
                return Result<Boolean>.Success(true);
            }

        }
    }
}
