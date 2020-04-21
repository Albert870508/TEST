using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDSLJ.MiniApp.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TEST.Api.Result;
using TEST.Api.Route;
using TEST.Exercise.Application.Users;
using TEST.Exercise.Application.Users.Dto;
using TEST.JWT;

namespace TEST.WebApi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [RouteV1("User")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly TokenProvider _tokenOption;

        public UserController(IConfiguration configuration, IUserService userService)
        {
            _userService = userService;

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("Audience:Secret").Value));
            var issuer = configuration.GetSection("Audience:Issuer").Value;
            var audience = configuration.GetSection("Audience:Audience").Value;

            _tokenOption = new TokenProvider(new TokenProviderOptions
            {
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            });
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Result<LoginOutput> Login([FromQuery]string loginCode)
        {
            
            string weChatOpenId = WeChatHelper.GetSession(loginCode).OpenId;//根据loginCode获取openid
            return _userService.Login(weChatOpenId, _tokenOption);
        }
    }
}